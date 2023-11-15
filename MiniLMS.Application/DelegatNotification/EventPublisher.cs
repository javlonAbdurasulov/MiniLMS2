using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.DelegatNotification
{
    public class EventPublisher
    {
        public event EmailEventHandler MyEvent;

        public void RaiseEvent(string information)
        {
            EmailEvent args = new EmailEvent { Information = information };
            OnMyEvent(args);
        }

        protected virtual void OnMyEvent(EmailEvent e)
        {
            MyEvent?.Invoke(this, e);
        }
    }
}
