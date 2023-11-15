using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.DelegatNotification
{
    public class EmailEvent:EventArgs
    {
        public string Information { get; set; }
    }
    public delegate void EmailEventHandler(object sender, EmailEvent e);
}
