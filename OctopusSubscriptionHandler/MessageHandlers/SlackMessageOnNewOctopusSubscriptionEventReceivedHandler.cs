using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using OctopusSubscriptionHandler.Messages;
using OctopusSubscriptionHandler.Models.Octopus;
using OctopusSubscriptionHandler.Models.Slack.Models;
using OctopusSubscriptionHandler.Utils;

namespace OctopusSubscriptionHandler.MessageHandlers
{
    public class SlackMessageOnNewOctopusSubscriptionEventReceivedHandler : INotificationHandler<NewOctopusSubscriptionEventReceived>
    {
        public Task Handle(NewOctopusSubscriptionEventReceived notification, CancellationToken cancellationToken)
        {
            var octopusEvent = notification.SubscriptionEvent;

            var message = octopusEvent.Payload.Event.Message.Replace("  ", " ").Trim().TrimEnd('.') + ".";
            var category = octopusEvent.Payload.Event.Category;
            var username = octopusEvent.Payload.Event.Username.ToLowerInvariant().Trim();

            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            var splitCategory = r.Replace(category, " ");

            var slackMsg = BuildSlackPayload(message, username, splitCategory, octopusEvent);
            var url = Util.GetEnvironmentVariable("SLACK_WEBHOOK_URL");
            var response = SendToSlack(slackMsg, url);

            return Task.CompletedTask;
        }

        private static SlackPayload BuildSlackPayload(string message,
            string username,
            string category,
            OctopusEvent octopusEvent)
        {
            var slackMsg = new SlackPayload
            {
                channel = "#paul-test",
                attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        color = "#5C91DC",
                        fallback = message,
                        footer = username,
                        footer_icon =
                            "https://emojipedia-us.s3.amazonaws.com/thumbs/240/microsoft/106/face-with-cowboy-hat_1f920.png",
                        pretext = category,
                        text = message,
                        ts = ToUnixTime(octopusEvent.Payload.Event.Occurred.ToUniversalTime())
                    }
                }
            };

            return slackMsg;
        }

        private static string SendToSlack(SlackPayload o,
            string slackHookUrl)
        {
            var hookJson = JsonConvert.SerializeObject(o);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(slackHookUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(hookJson);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}