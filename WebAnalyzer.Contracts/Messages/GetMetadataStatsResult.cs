using System;
using System.Collections.Generic;
using System.Text;

namespace WebAnalyzer.Contracts.Messages
{
    public class GetMetadataStatsResult
    {
        public Dictionary<string, int> KeyOccurrences { get; set; }
    }
}
