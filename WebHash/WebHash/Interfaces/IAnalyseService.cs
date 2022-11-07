using System;
using System.Collections.Generic;
using WebHash.Models.ViewModels;
using WebHash.Models.ViewModels.Analyse;

namespace WebHash.Interfaces
{
    public interface IAnalyseService
    {
        AnalyseViewModel GetAnalyseData();
        FileDetailsViewModel GetDetailsForFile(Guid id);
        IEnumerable<FileViewModel> GetAllFiles();
    }
}
