namespace TribunalScraper.Scraper;

public interface IScraper
{
    string? ScrapDecisionText(string caseId);
    Dictionary<string, string?>? ScrapCasesByPage(int page);

}