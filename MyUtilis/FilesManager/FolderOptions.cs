using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileManagerLib
{
    /// <summary>
    /// Folder options.
    /// </summary>
    public  enum FolderOptions
    {
        /// <summary>
        /// Only the content of folder, without delete de main folder.
        /// </summary>
        OnlyContent = 0,
        /// <summary>
        /// Delete all and the main folder.
        /// </summary>
        IncludeMainFolder = 1
    }
}