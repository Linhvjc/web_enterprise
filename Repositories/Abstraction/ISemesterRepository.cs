using WebEnterprise.Models.Entities;

namespace WebEnterprise.Repositories.Abstraction
{
    public interface ISemesterRepository : IGenericRepository<Semester>
    {
        Task<bool> IsExisted(string name);

    }
}
