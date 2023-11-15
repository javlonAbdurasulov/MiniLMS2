using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.DelegatNotification
{
    public class EventSubscriber
    {
        public void Subscribe(EventPublisher publisher)
        {
            publisher.MyEvent += HandleMyEvent;
        }

        private void HandleMyEvent(object sender, EmailEvent e)
        {
            Console.WriteLine($"Received information: {e.Information}");
        }
    }
}
