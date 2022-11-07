using static WebHash.Models.Enums.Enums;

namespace WebHash.Models.ViewModels
{
    public class HashInfoViewModel
    {
        //public string NameGiven { get; set; }
        public string OriginalString { get; set; }
        public bool IsDecrypted { get; set; }
        public string Result { get; set; }
        public int CrackingTime { get; set; }
        public string HashType { get; set; }

    }
}
