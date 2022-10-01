using WebHash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebHash.IServices
{
    public interface ICsvService
    {
        public IEnumerable<string> ImportCsvFile(string fileName);
    }
}
