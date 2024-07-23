using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.Mediatr.Behaiver
{
    public class LogginBehavior/*<TRequest, TResponse>*/ : IPipelineBehavior<StudentDelete, string>
    {

        private readonly Serilog.ILogger _seriaLog;
        public LogginBehavior(Serilog.ILogger logger)
        {
            _seriaLog = logger;
        }
        //public async Task<TResponse> Handle(TRequest request, 
        //    RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        //{
        //    _seriaLog.Information("Behaiver Start---------");
        //    var res = await next();
        //    _seriaLog.Information("Behaiver End---------");
        //    return res;
        //}

        public async Task<string> Handle(StudentDelete request, RequestHandlerDelegate<string> next, CancellationToken cancellationToken)
        {
            //_seriaLog.Information("Behaiver Start---------");
            await Console.Out.WriteLineAsync("Behaiver Start---------");
            var res = await next();
            //_seriaLog.Information("Behaiver End---------");
            await Console.Out.WriteLineAsync("Behaiver End---------");
            return res;
        }
    }
}
