using System;
using NATS.Client;
using Subscriber;

namespace JobLogger
{
    class Program
    {
        private static bool running = true;

        static void Main(string[] args)
        {
            var subscriber = new SubscriberService();

            string uri = "nats://" + Environment.GetEnvironmentVariable("NATS_HOST");
            using (IConnection connection = new ConnectionFactory().CreateConnection(uri))
            {
                subscriber.Run(connection);

                Console.WriteLine("JobLogger service is started");

                Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
                {
                    e.Cancel = true;
                    Program.running = false;
                };

                while (running) { }

                Console.WriteLine("JobLogger service is shut down");
            }
        }
    }
}
