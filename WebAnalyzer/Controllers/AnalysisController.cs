using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAnalyzer.Contracts;
using WebAnalyzer.Contracts.Messages;
using WebAnalyzer.Models;

namespace WebAnalyzer.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly IPageAnalyzerService _pageAnalyzer;

        public AnalysisController(IPageAnalyzerService pageAnalyzer)
        {
            _pageAnalyzer = pageAnalyzer;
        }

        public IActionResult Index(PageStatsModel model)
        {
            if (model.UriAddress != null)
            {
                var statsResult = _pageAnalyzer.GetMetadataStats(new GetMetadataStatsArgs { UriAddress = model.UriAddress});
                model.KeyOccurrences = statsResult.KeyOccurrences;
                model.Performed = true;
            }
            return View(model);
        }
    }
}