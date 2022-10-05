using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace WebHash.DataModels
{
    public class Context : DbContext
    {
        public DbSet<File> Files { get; set; }

        public DbSet<Hash> Hashes { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
    }
}
