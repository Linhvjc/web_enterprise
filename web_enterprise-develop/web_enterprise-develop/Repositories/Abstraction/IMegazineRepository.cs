using WebEnterprise.Models.Entities;

namespace WebEnterprise.Repositories.Abstraction
{
    public interface IMegazineRepository : IGenericRepository<Megazine>
    {
        Task<List<Megazine>> GetMegazinesWithRelevant();
    }
}
