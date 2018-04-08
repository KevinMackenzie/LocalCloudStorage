using System;
using System.Collections.Generic;
using System.Text;

namespace LCS
{
    public enum ItemType
    {
        Directory,
        File
    }
    
    /// <summary>
    /// A set of basic properties of a generic item
    /// </summary>
    public interface IItemProperties
    {
        string Path { get; }
        string Name { get; }
        ItemType Type { get; }

        string Sha1 { get; }
        ulong Size { get; }

        DateTime LastModified { get; }
    }
}
