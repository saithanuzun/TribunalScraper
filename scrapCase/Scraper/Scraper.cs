using System;
using HtmlAgilityPack;
using scrapCase;
using scrapCase.Extensions;

namespace scrapCase.Scraper;


public class Scraper : IScraper
{
    private readonly HtmlWeb _web;

    public Scraper(HtmlWeb web)
    {
        _web = web ?? throw new ArgumentNullException(nameof(web));
    }
    
    public string? ScrapDecisionText(string caseId)
    {
        Console.WriteLine("scarping decision text started caseId: " + caseId);

        var doc = _web.Load(Constants.CaseUrl(caseId));
        
        var nodes = doc.DocumentNode.SelectNodes("//div[@class='decision-inner']");

        if (nodes is null)
        {
            Console.WriteLine("scraping decision"+caseId+" returned null ");
            return null;
        }
        foreach (var node in nodes)
        {
            var result = node.InnerText.Trim();
            
            Console.WriteLine("text scraped!");
            
            return result;
        }
        
        Console.WriteLine("scraping decision"+caseId+" returned null ");
        return null;
    }

    public Dictionary<string, string?>? ScrapCasesByPage(int page)
    {
        if (page < 1 || page > Constants.PageCount) throw new Exception("page must be between 0 and "+ Constants.PageCount);
        
        Console.WriteLine("scraping page  id:" + page);

        var result = new Dictionary<string, string?>();

        var doc = _web.Load(Constants.PageUrl(page));

        string combinedXPath = "//tr[contains(@class,'first') or contains(@class,'last')]//a | //tr[contains(@class,'first') or contains(@class,'last')]//time";

        var nodes = doc.DocumentNode.SelectNodes(combinedXPath);

        string? currentCaseId = null;  // keep last seen caseId

        foreach (var node in nodes)
        {
            if (node.Name == "a")
            {
                currentCaseId = node.GetAttributeValue("href", string.Empty).Split('/').Last();

                if (!result.ContainsKey(currentCaseId))
                {
                    result[currentCaseId] = null; // initialize with null
                }
            }
            else if (node.Name == "time" && currentCaseId is not null)
            {
                string displayDate = node.InnerText.Trim().ToDatetime();
                result[currentCaseId] = displayDate;
            }
        }

        Console.WriteLine("page: " + page + " total of case is " + result.Count);
        return result;

    }
}