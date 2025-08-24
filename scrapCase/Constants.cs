namespace scrapCase;

public class Constants
{
    private static string BaseUrl => "https://tribunalsdecisions.service.gov.uk/";

    public static string PageUrl(int page) => BaseUrl +"utiac?&page=" + page;
    
    public static string CaseUrl(string caseId) => BaseUrl + "utiac/" + caseId;

    public static string DatabaseConnectionUrl => "USER ID=postgres;Password=password123;Server=localhost;Port=5432;Database=TribunalDecisions;Pooling=true";
    
    public static int PageCount = 1486;

}