using System;

namespace WebHash.DataModels
{
    public class File
    {
        public Guid Id { get; set; }

        public int Size { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool IsDeleted { get; set; }
    }

}
