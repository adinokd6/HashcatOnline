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

        public IList<Hash> Hashes { get; set; }

    }

}
