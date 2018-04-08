using System;
using System.Collections.Generic;
using System.Text;

namespace LCS.CSI
{
    internal interface IItemDeltaBuffer
    {
        /// <summary>
        /// Submit a delta to be processed
        /// </summary>
        /// <param name="itemDelta"></param>
        /// <returns>the ticket number to get callback info</returns>
        long Enqueue(IItemDelta itemDelta);
        bool TryGetDeltaResponse(long ticket, out ItemDeltaResponse response);
    }
}
