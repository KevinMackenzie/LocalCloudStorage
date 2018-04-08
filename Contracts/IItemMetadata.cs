using System;
using System.Collections.Generic;
using System.Text;

namespace LCS
{
    public enum ItemErrorState
    {
        None,
        /// <summary>
        /// When there are both local and remote changes conflicting
        /// </summary>
        VersionConflict,
        /// <summary>
        /// When the local file is not available to be opened for reading/writing
        /// </summary>
        FileInaccessible
    }
    public enum ItemConflictResolution
    {
        KeepLocal,
        KeepRemote,
        KeepBoth
    }
    public interface IItemMetadata
    {
        /// <summary>
        /// Gets a static copy of the current state of an item
        /// </summary>
        /// <param name="id">the id of the item to take a shapshot of</param>
        /// <returns></returns>
        IItemProperties GetItemProperties(string id);
        /// <summary>
        /// Gets the realtime updating state of an item
        /// </summary>
        /// <param name="id">the id of the item to get a reference to</param>
        /// <returns></returns>
        IItemHandle GetItemHandle(string id);
        /// <summary>
        /// Get an item id from its path.  Returns null if not found.
        /// </summary>
        string GetItemId(string path);

        /// <summary>
        /// Event triggered when a new item delta sent out.
        /// </summary>
        event ItemDeltaHandler OnItemDelta;
        /// <summary>
        /// An up-to-date list of the active deltas
        /// TODO: is this required for the public interface?
        /// </summary>
        IEnumerable<IItemDelta> ActiveDeltas { get; }

        /// <summary>
        /// Gets a list of item ids for the items currently in conflict
        /// </summary>
        IEnumerable<string> GetActiveConflicts();
        /// <summary>
        /// Applies the given resolution to an item in conflict
        /// </summary>
        void ResolveConflict(string itemId, ItemConflictResolution resolution);
    }
}
