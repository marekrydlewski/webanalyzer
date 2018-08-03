using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using WebAnalyzer.Contracts;
using WebAnalyzer.Contracts.Messages;

namespace WebAnalyzer.Core.Services
{
    public class PageAnalyzerService : IPageAnalyzerService
    {
        public GetMetadataStatsResult GetMetadataStats(GetMetadataStatsArgs args)
        {
            HtmlWeb web = new HtmlWeb
            {
                CaptureRedirect = true,
                PreRequest = request =>
                {
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    return true;
                }
            };
            var htmlDoc = web.Load(args.UriAddress);

            var keys = htmlDoc.DocumentNode?
                        .SelectSingleNode("//meta[@name='keywords']")?
                        .Attributes["content"]?.Value
                        .Split(',').ToList() ?? new List<string>();

            var texts = htmlDoc.DocumentNode?
                .SelectNodes("//body//*[not(self::script or self::style)]/text()[not(normalize-space(.)=\"\")]")?
                .Select(w => w.InnerText).ToList() ?? new List<string>();

            Dictionary<string, int> keyOccurences = keys.Distinct().ToDictionary(x => x, x => 0);

            foreach (var text in texts)
            {
                foreach(var key in keys)
                {
                    keyOccurences[key] += Regex.Matches(text, key, RegexOptions.IgnoreCase).Count;
                }
            }

            return new GetMetadataStatsResult {
                KeyOccurences = keyOccurences
            };
        }
    }
}
