using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LCS.CSI
{
    internal class ItemDeltaBuffer : IItemDeltaBuffer
    {
        #region Private Fields
        private ConcurrentDictionary<long, ItemDeltaResponse> _responses;
        private ConcurrentQueue<IItemDelta> _queue;
        private long _ticketCounter;
        private IItemDeltaTarget _target;
        #endregion

        public ItemDeltaBuffer(IItemDeltaTarget target)
        {
            _target = target;
        }

        /// <summary>
        /// Submit a delta to be processed
        /// </summary>
        /// <param name="itemDelta"></param>
        /// <returns>the ticket number to get callback info</returns>
        public long Enqueue(IItemDelta itemDelta)
        {
            _queue.Enqueue(itemDelta);
            return Interlocked.Increment(ref _ticketCounter);
        }

        public bool TryGetDeltaResponse(long ticket, out ItemDeltaResponse response)
        {
            return _responses.TryGetValue(ticket, out response);
        }
    }
}
