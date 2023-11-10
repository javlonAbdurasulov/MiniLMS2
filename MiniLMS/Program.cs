using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MiniLMS.Application;
using MiniLMS.Application.Client;
using MiniLMS.Application.CustomLogger;
using MiniLMS.Application.FluentValidation;
using MiniLMS.Application.Services;
using MiniLMS.Infrastructure;
using MiniLMS.Infrastructure.Services;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace MiniLMS;
public class Program
{
    public static void Main(string[] args)
    {
        ////perfect mediatr
        var builder = WebApplication.CreateBuilder(args);

        //Log.Logger = new LoggerConfiguration()
        //    .ReadFrom.Configuration(builder.Configuration).CreateLogger();
        #region
        Logger log = new LoggerConfiguration()
            .Enrich.WithThreadId()
            .Enrich.WithCorrelationId()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            //.WriteTo.Console(outputTemplate: "[{Timestamp:MM/dd HH:mm:ss} CorrelationId:{CorrelationId} {Level:u3}]  {Message:1j}{NewLine}{Exception}")
            .WriteTo.Console(outputTemplate: "[{Timestamp:MM/dd HH:mm:ss} MachineName: {MachineName}  {Level:u3}] {Message:1j}{NewLine}{Exception}")
            //.WriteTo.Console(outputTemplate: "[{Timestamp:MM/dd HH:mm:ss} ThreadId: {ThreadId} {Level:u3}] {Message:1j}{NewLine}{Exception}")
            .WriteTo.PostgreSQL("Server=::1; Database=loggers;User Id=logger; password=123",
            "Logs",
            needAutoCreateTable:true)

            //.WriteTo.PostgreSQL
            //    (builder.Configuration.GetConnectionString("LoggersCon"),
            //     "Logs",
            //     needAutoCreateTable:true)

            //.ReadFrom.Configuration(builder.Configuration)

            ////.WriteTo.Logger(lg=>
            ////    lg.Filter.ByExcluding(
            ////            logEvent =>
            ////                logEvent.MessageTemplate.Text.StartsWith("SerialogFor")||
            ////                logEvent.MessageTemplate.Text.Contains("Executing")||
            ////                logEvent.MessageTemplate.Text.Contains("Executed")
            ////                )
            ////        )

            //.WriteTo.Console()
            .WriteTo.File("./bin/logs/javaLog-.json", rollingInterval: RollingInterval.Day)
            //.WriteTo.Telegram(botToken: "6753874929:AAEOKsXGtzt04BG5zDYLKAsXtng2sSXa6UY", chatId: "5559328968")

            ////.Filter.With<CustomLogEventFilter>()
        

            //.MinimumLevel.Warning()

            ////.MinimumLevel.Verbose()

            ////.MinimumLevel.Debug() 
            ////.MinimumLevel.Override("Serilog", LogEventLevel.Information) 

            ////.Filter.ByExcluding(logEvent => 
            ////logEvent.MessageTemplate.Text.Contains("SerialogFor") ||
            ////logEvent.MessageTemplate.Text.Contains("Request starting") ||
            ////logEvent.MessageTemplate.Text.Contains("Executed action") ||
            ////logEvent.MessageTemplate.Text.Contains("Executed endpoint") ||
            ////logEvent.MessageTemplate.Text.Contains("Request finished")
            ////    )


            //.MinimumLevel.Information()
            .CreateLogger();


        #endregion

        try
        {
            // CreateAsync services to the container.

            builder.Services.AddControllers();


            builder.Services.AddSingleton<IMynewClient, MynewClient>();
            builder.Services.AddHttpClient<IMynewClient, MynewClient>(client =>
            {
                client.BaseAddress = new Uri("https://isdayoff.ru/YYMMDD");
            });


            //builder.Services.AddHttpContextAccessor();////////
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

            //builder.Services.AddSerilog();

            builder.Services.AddSerilog(log);


            builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
            //builder.Services.AddFluentValidation(opt =>
            //    opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            //builder.Services.AddRazorPages().AddMvcOptions(opt =>
            //{

            //});

            builder.Services.AddApplicationServise();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            
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
        catch { 
        
        }
        finally
        {
            //Log.CloseAndFlush();
        }
    }
}
