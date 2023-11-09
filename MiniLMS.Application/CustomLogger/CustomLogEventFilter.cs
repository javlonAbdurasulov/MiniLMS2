using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.CustomLogger
{
    public class CustomLogEventFilter : ILogEventFilter
    {
        public bool IsEnabled(LogEvent logEvent)
        {
            if (logEvent.MessageTemplate.Text.StartsWith("SerialogFor") && logEvent.MessageTemplate.Text.Contains("Executed"))
            {
                return false;
            }
            return true;
        }
    }
}
