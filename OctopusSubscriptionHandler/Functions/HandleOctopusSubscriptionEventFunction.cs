using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AzureFunctions.Autofac;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using OctopusSubscriptionHandler.Exceptions;
using OctopusSubscriptionHandler.Messages;
using OctopusSubscriptionHandler.Utility;
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

                var ok = await mediator.Send(new IncomingOctopusSubscriptionEventRequest(req));

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
                .Enrich.WithProperty("RequestId", req.GetCorrelationId())
                .Enrich.WithProperty("ClientIpAddress", req.GetClientIpAddress())
                .WriteTo.TraceWriter(traceWriter, outputTemplate: "{Timestamp}: [{Level}] \"{Message}\" {Properties}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}