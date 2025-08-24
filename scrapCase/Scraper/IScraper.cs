namespace scrapCase.Scraper;

public interface IScraper
{
    string? ScrapDecisionText(string caseId);
    Dictionary<string, string?>? ScrapCasesByPage(int page);

}