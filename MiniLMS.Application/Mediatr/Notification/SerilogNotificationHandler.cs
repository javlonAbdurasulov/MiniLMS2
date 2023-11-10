using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.Mediatr.Notification
{
    public class SerilogNotificationHandler : INotificationHandler<StudentNotification>
    {
        private readonly Serilog.ILogger _seriaLog;
        public SerilogNotificationHandler(Serilog.ILogger seriaLog)
        {
            _seriaLog = seriaLog;
        }
        public Task Handle(StudentNotification notification, CancellationToken cancellationToken)
        {
            _seriaLog.Information(notification.message);
            return Task.CompletedTask;
        }
    }
}
