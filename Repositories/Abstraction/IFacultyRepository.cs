using WebEnterprise.Models.Entities;
using WebEnterprise.ViewModels.Faculty;

namespace WebEnterprise.Repositories.Abstraction
{
    public interface IFacultyRepository : IGenericRepository<Faculty>
    {
        Task<bool> IsExisted(string name);
        Task<List<StatisticsFaculty>> GetStatisticsFaculties();
    }
}