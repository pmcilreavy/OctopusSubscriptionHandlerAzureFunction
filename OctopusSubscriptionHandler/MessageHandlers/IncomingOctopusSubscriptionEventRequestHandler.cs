using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Newtonsoft.Json;
using OctopusSubscriptionHandler.Exceptions;
using OctopusSubscriptionHandler.Messages;
using OctopusSubscriptionHandler.Models.Octopus;
using OctopusSubscriptionHandler.Utils;

namespace OctopusSubscriptionHandler.MessageHandlers
{
    public class IncomingOctopusSubscriptionEventRequestHandler : IRequestHandler<IncomingOctopusSubscriptionEventRequest, bool>
    {
        private readonly IMediator _mediator;

        public IncomingOctopusSubscriptionEventRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(IncomingOctopusSubscriptionEventRequest notification,
            CancellationToken cancellationToken)
        {
            if (notification?.Request == null)
            {
                throw new ArgumentNullException(nameof(notification.Request));
            }

            ValidateRequest(notification.Request);

            var incomingJson = await ValidateAndRetrieveIncomingJson(notification.Request);
            var octopusSubscriptionEvent = GetEventObject(incomingJson);
            var requestId = notification.Request.GetCorrelationId();
            var clientIp = GetClientIp(notification.Request);

            await _mediator.Publish(new NewOctopusSubscriptionEventReceived(requestId, clientIp, octopusSubscriptionEvent, notification.Logger), cancellationToken);

            return true;
        }

        private static async Task<string> ValidateAndRetrieveIncomingJson(HttpRequestMessage request)
        {
            var incomingJson = await request.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(incomingJson))
            {
                throw new BadRequestException("Incoming content string was null or empty!");
            }

            return incomingJson;
        }

        private static void ValidateRequest(HttpRequestMessage request)
        {
            if (request?.Content == null)
            {
                throw new BadRequestException("The request contained no content!");
            }

            var requestHeaders = request.Headers.ToDictionary(o => o.Key, kp => kp.Value);

            if (requestHeaders.TryGetValue("SUBSCRIPTION_EVENT_TOKEN", out var values))
            {
                var token = values?.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(token) && token == Util.GetEnvironmentVariable("TOKEN"))
                {
                    return;
                }
            }

            throw new BadRequestException("Incoming request did not contain a valid event token.");
        }

        private static OctopusEvent GetEventObject(string incomingJson)
        {
            OctopusEvent octopusSubscriptionEvent = null;

            try
            {
                octopusSubscriptionEvent = JsonConvert.DeserializeObject<OctopusEvent>(incomingJson);
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Failed to deserialize incoming json.", ex);
            }

            return octopusSubscriptionEvent;
        }

        private static IPAddress GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress.Parse(((HttpContextWrapper)request?.Properties["MS_HttpContext"])?.Request?.UserHostAddress);
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                var prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return IPAddress.Parse(prop.Address);
            }

            return null;
        }
    }
}