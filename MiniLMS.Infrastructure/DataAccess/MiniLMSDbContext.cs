using Microsoft.EntityFrameworkCore;
using MiniLMS.Domain.Entities;

namespace MiniLMS.Infrastructure.DataAccess;
public class MiniLMSDbContext : DbContext
{
    public MiniLMSDbContext()
    {

    }

    public MiniLMSDbContext(DbContextOptions<MiniLMSDbContext> options)
        : base(options)
    {

    }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
}
