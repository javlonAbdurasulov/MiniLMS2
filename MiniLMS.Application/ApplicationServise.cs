using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MiniLMS.Application;

public static class ApplicationServise
{
    public static void AddApplicationServise(this IServiceCollection service)
    {
        service.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        
    }
}
