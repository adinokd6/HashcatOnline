using Coded.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coded.IServices
{
    public interface IHashService
    {
        public void Decode(Hash hash);
    }
}
