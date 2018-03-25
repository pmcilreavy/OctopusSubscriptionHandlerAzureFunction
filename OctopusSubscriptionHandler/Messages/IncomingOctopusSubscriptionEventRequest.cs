using System.Net.Http;
using MediatR;

namespace OctopusSubscriptionHandler.Messages
{
    public class IncomingOctopusSubscriptionEventRequest : IRequest<bool>
    {
        public HttpRequestMessage Request { get; }

        public IncomingOctopusSubscriptionEventRequest(
            HttpRequestMessage request)
        {
            Request = request;
        }
    }
}