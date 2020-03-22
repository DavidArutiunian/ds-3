using System;
using System.Text;
using NATS.Client;
using NATS.Client.Rx;
using NATS.Client.Rx.Ops;
using System.Linq;

namespace Subscriber
{
    public class SubscriberService
    {
        public void Run(IConnection connection)
        {
            var events = connection.Observe("events")
                    .Where(m => m.Data?.Any() == true)
                    .Select(m => Encoding.Default.GetString(m.Data));

            events.Subscribe(msg => Console.WriteLine(msg));
        }
    }
}
