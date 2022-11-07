using System.Collections.Generic;
using static WebHash.Models.Enums.Enums;

namespace WebHash.Models.ViewModels
{
    public class FileDetailsViewModel
    {
        public string Name { get; set; }
        public int HashesCount { get; set; }
        public int SumOfCrackingTime { get; set; }
        public int UnCrackedPasswords { get; set; }
        public int CrackedPasswords { get; set; }
        public IEnumerable<HashForFileNumberViewModel> HashInFileNumberList { get; set; }
        public IEnumerable<HashInfoViewModel> HashesDetails { get; set; }
    }
}
