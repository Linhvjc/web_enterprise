using WebEnterprise.ViewModels.Contribution;

namespace WebEnterprise.Repositories.Abstraction
{
    public interface IContributionRepository : IGenericRepository<Contribution>
    {
        Task<List<GetContributionModel>> GetAllContributions(int megazineId);
    }
}
