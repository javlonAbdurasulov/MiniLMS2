using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniLMS.Application.Services;
using MiniLMS.Infrastructure.DataAccess;
using MiniLMS.Infrastructure.Services;

namespace MiniLMS.Infrastructure;
public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddDbContext<MiniLMSDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("MiniLMSDbConnection")));
    }
}
