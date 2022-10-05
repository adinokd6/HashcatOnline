using System;
using System.Collections.Generic;

namespace WebHash.DataModels
{
    public class File
    {
        public Guid Id { get; set; }

        public int Size { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool IsDeleted { get; set; }

        public IList<Hash> Hashes { get; set; }

        public bool IsResultFile { get; set; }

        public int CrackingTime { get; set; } //In seconds
        
        public int CrackedPasswords { get; set; }

        public int UnCrackedPasswords { get; set; }

    }

}
