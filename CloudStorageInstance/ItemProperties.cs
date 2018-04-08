using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LCS.CSI
{
    internal class ItemProperties : IItemProperties
    {
        public ItemProperties() { }

        public ItemProperties(IItemProperties other)
        {
            Path = other.Path;
            Name = other.Name;
            Type = other.Type;
            Sha1 = other.Sha1;
            Size = other.Size;
            LastModified = other.LastModified;
        }
        public string Path { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; }
        public string Sha1 { get; set; }
        public ulong Size { get; set; }
        public DateTime LastModified { get; set; }
    }
}
