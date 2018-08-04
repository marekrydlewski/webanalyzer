using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public PageAnalyzerService(ILogger<PageAnalyzerService> logger)
        {
            _logger = logger;
        }

        public GetMetadataStatsResult GetMetadataStats(GetMetadataStatsArgs args)
        {
            _logger.LogInformation("Getting metadata stats of {uri} site", args.UriAddress);
            HtmlWeb web = new HtmlWeb
            {
                CaptureRedirect = true,
                PreRequest = request =>
                {
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    return true;
                }
            };

            HtmlDocument htmlDoc = null;
            try
            {
                htmlDoc = web.Load(args.UriAddress);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Loading site {uri} unsuccessful", args.UriAddress);
                return new GetMetadataStatsResult
                {
                    KeyOccurrences = null
                };
            }

            var keys = htmlDoc.DocumentNode?
                        .SelectSingleNode("//meta[@name='keywords']")?
                        .Attributes["content"]?.Value
                        .Split(',').ToList();
            if (keys == null)
            {
                _logger.LogWarning("No keywords in site {uri}", args.UriAddress);
                return new GetMetadataStatsResult
                {
                    KeyOccurrences = null
                };
            }

            var texts = htmlDoc.DocumentNode?
                .SelectNodes("//body//*[not(self::script or self::noscript or self::style)]/text()[not(normalize-space(.)=\"\")]")?
                .Select(w => w.InnerText).ToList() ?? new List<string>();

            Dictionary<string, int> keyOccurences = keys.Distinct().ToDictionary(x => x, x => 0);

            foreach (var key in keys)
            {
                foreach(var text in texts)
                {
                    keyOccurences[key] += Regex.Matches(text, key, RegexOptions.IgnoreCase).Count;
                }
            }

            return new GetMetadataStatsResult {
                KeyOccurrences = keyOccurences
            };
        }
    }
}
