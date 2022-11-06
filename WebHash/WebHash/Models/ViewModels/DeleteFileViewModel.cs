using System;
using System.Collections.Generic;

namespace WebHash.Models.ViewModels
{
    public class DeleteFileViewModel
    {
        public Guid Id { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
    }
}
