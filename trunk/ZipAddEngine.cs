using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;

namespace netCommander
{
    [Flags()]
    public enum ArchiveAddOptions
    {
        None=0,
        SupressErrors=0x1,
        Recursive=0x2,
        SaveAttributes=0x4,
        NeverRewrite=0x8,
        RewriteIfSourceNewer=0x10,
        RewriteAlways=0x20
    }
}
