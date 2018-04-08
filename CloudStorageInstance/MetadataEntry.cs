using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LCS.CSI
{
    internal class MetadataEntry : ViewModelBase, IItemHandle
    {
        #region Private Fields
        private ItemErrorState _errorState;
        private IActionStatus _actionStatus;
        #endregion

        public MetadataEntry(string id, ItemProperties properties)
        {
            Properties = properties;
            Id = id;
        }

        public ItemProperties Properties { get; }
        public Dictionary<long, IItemDelta> PendingDeltas { get; }

        public IActionStatus ActionStatus
        {
            get => _actionStatus;
            set
            {
                if (_actionStatus == value) return;
                _actionStatus = value;
                OnPropertyChanged();
            }
        }

        #region IItemHandle
        public string Id { get; set; }

        IItemProperties IItemHandle.Properties => Properties;

        public ItemErrorState ErrorState
        {
            get => _errorState;
            set
            {
                if (_errorState == value) return;
                _errorState = value;
                OnPropertyChanged();
            }
        }

        IActionStatus IItemHandle.ActionStatus => _actionStatus;

        #endregion
    }
}
