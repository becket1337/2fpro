using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace _2fpro.ViewModel
{
    public class FilesViewModel
    {
        public PagingInfo PagingInfo { get; set; }

        public IEnumerable<FileInfo> Files { get; set; }
        public IEnumerable<DirectoryInfo> Dirs { get; set; }
        public int someLimit { get; set; }

        public string FileName { get; set; }
        [Required(ErrorMessage = "пустое поле")]
        public string DirName { get; set; }

    }
}