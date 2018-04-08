using System;
using System.Collections.Generic;
using System.Text;

namespace LCS.CSI
{
    internal interface IItemDeltaTarget
    {
        ItemDeltaResponse HandleDelta(IItemDelta itemDelta);
    }
}
