using System;
using static WebHash.Models.Enums.Enums;

namespace WebHash.DataModels
{
    public class Hash
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OriginalString { get; set; }
        public string Result { get; set; }
        public int CrackingTime { get; set; } //In seconds. Only used when cracking one hash
        public HashType HashType { get; set; }
    }
}
