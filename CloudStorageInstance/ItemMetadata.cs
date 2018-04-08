using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace LCS.CSI
{
    //TODO: ended with the ItemDeltaBuffer.  Next step is to properly add deltas to pending dictionaries (global and per item) when they get submitted to the metadata
    // TODO: how generate conflicts based on metadata state given a submitted delta
    /// <inheritdoc />
    internal class ItemMetadata : IItemMetadata
    {
        #region Private Fields
        /// <summary>
        /// A mapping between item ids and their data
        /// </summary>
        private ConcurrentDictionary<string, MetadataEntry> _itemMetadata;

        private ConcurrentDictionary<long, IItemDelta> _pendingDeltas;
        private long _tempItemIdCounter;
        private IItemDeltaBuffer _localDeltaBuffer;
        private IItemDeltaBuffer _remoteDeltaBuffer;
        #endregion

        public ItemMetadata(IEnumerable<MetadataEntry> metadata,
            IItemDeltaBuffer localDeltaBuffer,
            IItemDeltaBuffer remoteDeltaBuffer)
        {
            _itemMetadata = new ConcurrentDictionary<string, MetadataEntry>(metadata.ToDictionary(entry => entry.Id));
            _localDeltaBuffer = localDeltaBuffer;
            _remoteDeltaBuffer = remoteDeltaBuffer;
        }

        public void SubmitDelta(IItemDelta itemDelta)
        {
            // Outline of handling this delta:
            //
            //  If there are any pending deltas for the item from the
            //      opposite source, then there is a conflict.  This conflict
            //      will cause the delta to lock up the queue
            //
            //  Resolving this conflict: choose whether to push all local 
            //      deltas or all remote ones, OR to rename the local
            //      keep the remote, and close any deltas from the local.
            //      this new local file will be seen as an event for
            //      creating a new file that is not in the metadata
            //      and will spawn a delta to upload to the remote

            //See if the item exists in the metadata
            var entry = GetItemHandleByPath(itemDelta.PreProperties.Path);

            // Item exists in the metadata
            if (entry != null)
            {
                // If there is a temp ID and the source is remote, update the ID
                if (entry.Id[0] == '_' && itemDelta.Source == DeltaSource.Remote)
                {
                    entry.Id = itemDelta.Handle.Id;
                }

                if (entry.PendingDeltas.Any(kvp => kvp.Value.Source != itemDelta.Source))
                {
                    //conflicts
                    entry.ErrorState = ItemErrorState.VersionConflict;
                }
            }
            // Item doesn't exist in the metadata
            else
            {
                // if the item doesn't exist, we can't work with info we don't have,
                //  so add the pre-delta info to the metadata and submit the delta

                if (itemDelta.Source == DeltaSource.Local)
                {
                    // If the item doesn't exist and the source is local, then apply a temp id
                    //  until the update rebounds
                    var id = "_" + Interlocked.Increment(ref _tempItemIdCounter);
                    _itemMetadata.TryAdd(id, new MetadataEntry(id, new ItemProperties(itemDelta.PreProperties)));
                }
                else
                {
                    //not in metadata, remote source, so simply add the properties
                    _itemMetadata.TryAdd(itemDelta.Handle.Id,
                        new MetadataEntry(itemDelta.Handle.Id, new ItemProperties(itemDelta.PreProperties)));
                }

                // This will NEVER be null
                entry = GetItemHandleByPath(itemDelta.PreProperties.Path);
            }

            // Submit the delta to the proper buffer
            long ticket = 0;
            switch (itemDelta.Source)
            {
                case DeltaSource.Local:
                    ticket = _remoteDeltaBuffer.Enqueue(itemDelta);
                    break;
                case DeltaSource.Remote:
                    ticket = _localDeltaBuffer.Enqueue(itemDelta);
                    break;
            }

            // Finally, add the delta to the pending delta dictionaries
            entry.PendingDeltas.Add(ticket, itemDelta);

            // TODO: this shouldn't ever fail since tickets are unique
            _pendingDeltas.TryAdd(ticket, itemDelta);
        }
        public void SubmitDeltas(IEnumerable<IItemDelta> itemDeltas)
        {
            
        }

        #region IItemMetadata
        /// <inheritdoc />
        public IItemProperties GetItemProperties(string id)
        {
            return _itemMetadata.TryGetValue(id, out var entry) ? new ItemProperties(entry.Properties) : null;
        }

        /// <inheritdoc />
        public IItemHandle GetItemHandle(string id)
        {
            return _itemMetadata.TryGetValue(id, out var entry) ? entry : null;
        }

        public MetadataEntry GetItemHandleByPath(string path)
        {
            return (from MetadataEntry entry in _itemMetadata.Values
                where entry.Properties.Path == path
                select entry).FirstOrDefault();
        }

        /// <inheritdoc />
        public string GetItemId(string path)
        {
            return GetItemHandleByPath(path)?.Id;
        }

        /// <inheritdoc />
        public event ItemDeltaHandler OnItemDelta;
        /// <inheritdoc />
        public IEnumerable<IItemDelta> ActiveDeltas => _pendingDeltas.Values;
        /// <inheritdoc />
        public IEnumerable<string> GetActiveConflicts()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ResolveConflict(string itemId, ItemConflictResolution resolution)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
