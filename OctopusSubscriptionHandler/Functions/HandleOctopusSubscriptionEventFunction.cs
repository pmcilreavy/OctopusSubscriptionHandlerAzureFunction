using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AzureFunctions.Autofac;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using OctopusSubscriptionHandler.Core.Messages;
using OctopusSubscriptionHandler.Core.Models.Octopus;
using OctopusSubscriptionHandler.Core.Utility;
using OctopusSubscriptionHandler.Exceptions;
using Serilog;
using Serilog.Sinks.AzureWebJobsTraceWriter;

namespace OctopusSubscriptionHandler.Functions
{
    [DependencyInjectionConfig(typeof(IoC))]
    public static class HandleOctopusSubscriptionEventFunction
    {
        [FunctionName("HandleOctopusSubscriptionEventFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequestMessage req,
            TraceWriter traceWriter,
            [Inject] IMediator mediator)
        {
            try
            {
                ConfigureSerilog(req, traceWriter);

                Log.Information("Function triggered.");

                ValidateRequest(req);

                var incomingJson = await ValidateAndRetrieveIncomingJson(req);
                var octopusSubscriptionEvent = GetEventObject(incomingJson);

                await mediator.Publish(new NewOctopusSubscriptionEventReceived(octopusSubscriptionEvent), CancellationToken.None);

                Log.Information("Done.");

                return req.CreateResponse(HttpStatusCode.OK, ":)");
            }
            catch (BadRequestException ex)
            {
                Log.Error("Bad request", ex);

                return req.CreateResponse(HttpStatusCode.BadRequest, $":( '{ex.Message}'");
            }
            catch (Exception ex)
            {
                Log.Error("General exception", ex);

                return req.CreateResponse(HttpStatusCode.InternalServerError, $":( '{ex.Message}'");
            }
        }

        private static void ConfigureSerilog(
            HttpRequestMessage req,
            TraceWriter traceWriter)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.With()
                .Enrich.WithProperty("RequestId", req?.GetCorrelationId() ?? Guid.NewGuid())
                .Enrich.WithProperty("ClientIpAddress", GetClientIpAddress(req))
                .WriteTo.TraceWriter(traceWriter, outputTemplate: "{Timestamp}: [{Level}] \"{Message}\" {Properties}{NewLine}{Exception}")
                .CreateLogger();
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

        public static IPAddress GetClientIpAddress(HttpRequestMessage request)
        {
            if (request != null)
            {
                return null;
            }

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress.Parse((((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress));
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return IPAddress.Parse(prop.Address);
            }

            return null;
        }
    }
}