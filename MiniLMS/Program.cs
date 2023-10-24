using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MiniLMS.Application;
using MiniLMS.Application.FluentValidation;
using MiniLMS.Infrastructure;

namespace MiniLMS;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // CreateAsync services to the container.

        builder.Services.AddControllers();
        //builder.Services.AddFluentValidation(); 
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        //builder.Services.AddMvc();
        builder.Services.AddFluentValidation();
        builder.Services.AddMemoryCache();
        builder.Services.AddLazyCache();
        builder.Services.AddStackExchangeRedisCache(opt =>
        {
            string connect = builder.Configuration.
                GetConnectionString("Redis");

            opt.Configuration = connect;
        });
        builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
        //builder.Services.AddFluentValidation(opt =>
        //    opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

        //builder.Services.AddRazorPages().AddMvcOptions(opt =>
        //{

        //});

        builder.Services.AddApplicationServise();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
