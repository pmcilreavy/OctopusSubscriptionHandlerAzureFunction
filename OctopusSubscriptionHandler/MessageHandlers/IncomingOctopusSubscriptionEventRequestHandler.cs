using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using OctopusSubscriptionHandler.Exceptions;
using OctopusSubscriptionHandler.Messages;
using OctopusSubscriptionHandler.Models.Octopus;
using OctopusSubscriptionHandler.Utility;
using Serilog;
using Serilog.Context;

namespace OctopusSubscriptionHandler.MessageHandlers
{
    public class IncomingOctopusSubscriptionEventRequestHandler : IRequestHandler<IncomingOctopusSubscriptionEventRequest, bool>
    {
        private readonly IMediator _mediator;

        public IncomingOctopusSubscriptionEventRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(
            IncomingOctopusSubscriptionEventRequest notification,
            CancellationToken cancellationToken
            )
        {
            if (notification?.Request == null)
            {
                throw new ArgumentNullException(nameof(notification.Request));
            }

            ValidateRequest(notification.Request);

            var incomingJson = await ValidateAndRetrieveIncomingJson(notification.Request);
            var octopusSubscriptionEvent = GetEventObject(incomingJson);

            await _mediator.Publish(new NewOctopusSubscriptionEventReceived(octopusSubscriptionEvent), cancellationToken);

            return true;
        }

        private static async Task<string> ValidateAndRetrieveIncomingJson(HttpRequestMessage request)
        {
            var incomingJson = await request.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(incomingJson))
            {
                throw new BadRequestException("Incoming content string was null or empty!");
            }

            Log.Information(incomingJson);

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
                    Log.Information("Subscription token matched successfully {Token}", token);
                    return;
                }

                Log.Warning("Received token did not match {Token}", token);
            }

            throw new BadRequestException("Incoming request did not contain a valid event token.");
        }

        private static OctopusSubscriptionEvent GetEventObject(string incomingJson)
        {
            try
            {
                var octopusSubscriptionEventEvent = JsonConvert.DeserializeObject<OctopusSubscriptionEvent>(incomingJson);

                return octopusSubscriptionEventEvent;
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Failed to deserialize incoming json.", ex);
            }
        }
    }
}