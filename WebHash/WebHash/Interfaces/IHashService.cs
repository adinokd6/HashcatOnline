using WebHash.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebHash.Models.ViewModels.Analyse;

namespace WebHash.IServices
{
    public interface IHashService
    {
        void Decode(CrackHashViewModel hash);
    }
}
