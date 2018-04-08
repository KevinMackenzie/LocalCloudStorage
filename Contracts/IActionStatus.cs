using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LCS
{
    /// <summary>
    /// The status information about a long-running action
    /// </summary>
    public interface IActionStatus : INotifyPropertyChanged
    {
        float Progress { get; }
    }
}
