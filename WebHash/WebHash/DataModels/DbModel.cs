using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace WebHash.DataModels
{
    public class Context : DbContext
    {
        DbSet<File> Files { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
    }
}
