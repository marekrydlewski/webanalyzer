using System;
using System.Collections.Generic;
using System.Text;
using WebAnalyzer.Contracts.Messages;

namespace WebAnalyzer.Contracts
{
    public interface IPageAnalyzerService
    {
        GetMetadataStatsResult GetMetadataStats(GetMetadataStatsArgs args);
    }
}
