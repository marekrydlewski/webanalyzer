using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAnalyzer.Models
{
    public class PageStatsModel
    {
        public Uri UriAddress { get; set; }

        public Dictionary<string, int> Statistics { get; set; }
    }
}
