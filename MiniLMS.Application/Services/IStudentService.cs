using MiniLMS.Application.Repositories;
using MiniLMS.Domain.Entities;

namespace MiniLMS.Application.Services;
public interface IStudentService : IRepository<Student>
{
}
