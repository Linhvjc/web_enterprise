using WebEnterprise.Models.Entities;
using WebEnterprise.ViewModels.Contribution;

namespace WebEnterprise.Repositories.Abstraction
{
    public interface IContributionRepository : IGenericRepository<Contribution>
    {
        Task<List<GetContributionModel>> GetAllContributions(int megazineId);
        Task<DetailContribution> GetContributionWithRelevant(int id);
        Task<List<GetContributionStudent>> GetAllContributionStudents(string userId);
        Task<List<GetContributionModel>> SearchContribution(int megazineId, string? query);
        Task<List<StatisticContribution>> GetContributionsWithout();
        Task<List<StatisticContribution>> GetContributionsWithout14();
        Task<List<GetContributionModel>> SearchContribution(string semester);
    }
}
