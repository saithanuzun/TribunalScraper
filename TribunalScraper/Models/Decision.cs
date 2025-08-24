namespace TribunalScraper.Models;

public class Decision
{
    public Decision(string caseId, DateTime dateTime, string decisionText)
    {
        CaseId = caseId;
        DateTime = dateTime;
        DecisionText = decisionText;
    }

    public Decision(){}

    public string CaseId { get; set; }
    public DateTime DateTime { get; set; }
    public string DecisionText { get; set; }
}