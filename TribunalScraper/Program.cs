using System.Globalization;
using System.Text.Json;
using HtmlAgilityPack;
using TribunalScraper;
using TribunalScraper.Models;
using TribunalScraper.Repository;
using TribunalScraper.Scraper;



IScraper scraper = new Scraper(new HtmlWeb());

IDecisionRepository repository = new DecisionRepository(Constants.DatabaseConnectionUrl);
        
string configPath = "/Users/saithanuzun/Desktop/git-Repositories/TribunalScraper/TribunalScraper/appsettings.json";
var configText = File.ReadAllText(configPath);
var config = JsonSerializer.Deserialize<AppConfig>(configText);

int currentPage = int.Parse(config.LastScrapedPageId);



for ( ; currentPage <= Constants.PageCount; currentPage++)
{

    var IdsAndDates = scraper.ScrapCasesByPage(currentPage);

    foreach (var id in IdsAndDates.Keys)
    {
        var text=scraper.ScrapDecisionText(id);
        
        if (text is null) continue;

        var decision = new Decision(id, DateTime.ParseExact(
            IdsAndDates[id].Trim(),              
            "yyyy-MM-dd",            
            CultureInfo.InvariantCulture
        ), text);
        
        await repository.AddDecisionAsync(decision);

    }
    
    config.LastScrapedPageId = currentPage.ToString();
    var updatedText = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(configPath, updatedText);
    Console.WriteLine("Saved LastScrapedPageId: " + config.LastScrapedPageId);
    
}
