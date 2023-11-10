using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.Mediatr.Notification
{
    public class StudentNotification:INotification
    {
        public string message{ get; set; }
    }
}
