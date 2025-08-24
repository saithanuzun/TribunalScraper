using scrapCase.Models;

namespace scrapCase.Repository;

public interface IDecisionRepository
{
    Task<Decision> GetByIdAsync(string id);
    Task AddDecisionAsync(Decision decision);
    Task<List<Decision>> GetAllDecisionsAsync();
}