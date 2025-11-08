using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;
using QWellApp.Models;
using QWellApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.ViewModels.Common
{
    public interface ISummaryViewModel
    {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        int StartTime { get; set; }
        int EndTime { get; set; }

        Task LoadAllSummaries();
    }
}
