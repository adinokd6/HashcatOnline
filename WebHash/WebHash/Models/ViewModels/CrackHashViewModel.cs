using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WebHash.Models.Enums.Enums;

namespace WebHash.Models.ViewModels
{
    public class CrackHashViewModel
    {
        [Display(Name = "Type in your hash to crack")]
        public string InputValue { get; set; }

        public bool HashFromInput { get; set; }

        public Guid HashFromFileId { get; set; }

        [Display(Name = "Output value")]
        public Tuple<string, string> OutputValue { get; set; }

        [Display(Name = "Choose hash mode")]
        public HashType HashType { get; set; }

        [Display(Name = "Choose attack mode")]
        public AttackMethod AttackMethod { get; set; }

        [Display(Name = "Choose first dictionary for this attack method")]
        public string Dictionary1 { get; set; }

        [Display(Name = "Choose second dictionary for this attack method")]
        public string Dictionary2 { get; set; }

        public IEnumerable<FileViewModel> Files { get; set; }
        public IEnumerable<HashViewModel> Hashes { get; set; }

    }
}