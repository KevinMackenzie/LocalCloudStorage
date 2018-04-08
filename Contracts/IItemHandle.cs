using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LCS
{
    /// <summary>
    /// A handle to an item.  Essentially a pointer to an
    ///     item in the ItemMetadata
    /// </summary>
    public interface IItemHandle : INotifyPropertyChanged
    {
        string Id { get; }
        IItemProperties Properties { get; }
        ItemErrorState ErrorState { get; }
        IActionStatus ActionStatus { get; }
    }
}
