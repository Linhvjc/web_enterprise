using WebEnterprise.Models.Entities;

namespace WebEnterprise.Repositories.Abstraction
{
    public interface IFacultyRepository : IGenericRepository<Faculty>
    {
        Task<bool> IsExisted(string name);
    }
}