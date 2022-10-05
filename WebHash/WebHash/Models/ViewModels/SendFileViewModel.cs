using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebHash.Models.ViewModels
{
    public class SendFileViewModel
    {
        [Required]
        public IFormFile UploadedFile { get; set; }

        [Required]
        [MinLength(5)]
        public string FileName { get; set; }
    }
}
