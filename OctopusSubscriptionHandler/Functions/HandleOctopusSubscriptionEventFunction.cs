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

namespace OctopusSubscriptionHandler.Functions
{
    [DependencyInjectionConfig(typeof(DIConfig))]
    public static class HandleOctopusSubscriptionEventFunction
    {
        [FunctionName("HandleOctopusSubscriptionEventFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequestMessage req,
            TraceWriter logger,
            [Inject] IMediator mediator)
        {
            try
            {
                logger.Info($"Triggered");

                var ok = await mediator.Send(new IncomingOctopusSubscriptionEventRequest(req, logger));

                logger.Info("Done :)");

                return req.CreateResponse(HttpStatusCode.OK, ":)");
            }
            catch (BadRequestException ex)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, $":( '{ex.Message}'");
            }
            catch (Exception ex)
            {
                logger.Error("Fail :(", ex);

                return req.CreateResponse(HttpStatusCode.InternalServerError, ":(");
            }
        }
    }
}