using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LCS
{
    public enum DeltaSource
    {
        Local,
        Remote
    }
    public enum DeltaType
    {
        Content,
        Delete,
        Move,
        Copy,
        Rename
    }

    public enum DeltaStatus
    {
        /// <summary>
        /// Not started yet (in queue)
        /// </summary>
        Pending,
        /// <summary>
        /// Currently being applied
        /// </summary>
        InProgress,
        /// <summary>
        /// Being applied, but paused for now
        /// </summary>
        Paused,
        /// <summary>
        /// Successfully applied (done)
        /// </summary>
        Completed,
        /// <summary>
        /// User cancelled (done)
        /// </summary>
        Cancelled,
        /// <summary>
        /// Unsuccessful/error (blocks queue)
        /// </summary>
        Error
    }

    public interface IItemDeltaExtraData
    {
    }

    public interface IItemDelta : INotifyPropertyChanged
    {
        DeltaType Type { get; }
        DeltaSource Source { get; }
        
        /// <summary>
        /// The properties of the item before this delta
        /// </summary>
        IItemProperties PreProperties { get; }
        /// <summary>
        /// The properties of the item after this delta
        /// </summary>
        IItemProperties PostProperties { get; }
        /// <summary>
        /// A handle to the item for guaranteed up-to-date property information
        /// </summary>
        IItemHandle Handle { get; }
        /// <summary>
        /// Any additional data required by the given <see cref="DeltaType"/>
        /// </summary>
        IItemDeltaExtraData ExtraData { get; }
        DeltaStatus Status { get; }
    }
}
