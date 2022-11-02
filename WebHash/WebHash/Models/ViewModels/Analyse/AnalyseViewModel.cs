using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebHash.Attributes;


namespace WebHash.Models.ViewModels.Analyse
{
    public class AnalyseViewModel
    {
        public List<string> HashTypes { get; set; }
        public List<int> HashNumbers { get; set; }
        public List<double> AverageDehashTime { get; set; }
        public List<string> BackgroundColor { get; set; }
        public List<string> BorderColor { get; set; }
    }
}