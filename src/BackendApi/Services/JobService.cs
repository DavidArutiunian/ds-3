using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NATS.Client;
using System.Text;

namespace BackendApi.Services
{
    public class JobService : Job.JobBase, IDisposable
    {
        private readonly static Dictionary<string, string> _jobs = new Dictionary<string, string>();
        private readonly ILogger<JobService> _logger;
        private readonly IConnection _connection;

        public JobService(ILogger<JobService> logger)
        {
            _logger = logger;
            string uri = "nats://" + Environment.GetEnvironmentVariable("NATS_HOST");
            _connection = new ConnectionFactory().CreateConnection(uri);
        }

        public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            string id = Guid.NewGuid().ToString();
            var resp = new RegisterResponse { Id = id };
            _jobs[id] = request.Description;
            PublishJobCreated(id);
            return Task.FromResult(resp);
        }

        private void PublishJobCreated(string id)
        {
            string message = $"JobCreated|{id}";
            byte[] payload = Encoding.Default.GetBytes(message);
            _connection.Publish("events", payload);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
