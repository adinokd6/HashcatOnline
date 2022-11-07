using System;
using System.Collections.Generic;

namespace WebHash.Models.ViewModels
{
    public class FileInfoViewModel
    {
        public Guid Id { get; set; }
        public IEnumerable<FileViewModel> FilesForSelect { get; set; }
        public FileDetailsViewModel FileDetails { get; set; }
    }
}
