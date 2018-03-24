using System.Net.Http;
using MediatR;
using Microsoft.Azure.WebJobs.Host;

namespace OctopusSubscriptionHandler.Messages
{
    public class IncomingOctopusSubscriptionEventRequest : IRequest<bool>
    {
        public HttpRequestMessage Request { get; }
        public TraceWriter Logger { get; }

        public IncomingOctopusSubscriptionEventRequest(
            HttpRequestMessage request,
            TraceWriter logger)
        {
            Request = request;
            Logger = logger;
        }
    }
}