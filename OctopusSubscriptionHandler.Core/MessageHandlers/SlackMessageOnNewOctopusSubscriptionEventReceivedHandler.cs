using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using OctopusSubscriptionHandler.Core.Messages;
using OctopusSubscriptionHandler.Core.Models.Octopus;
using OctopusSubscriptionHandler.Core.Models.Slack;
using OctopusSubscriptionHandler.Core.Utility;
using Serilog;

namespace OctopusSubscriptionHandler.Core.MessageHandlers
{
    public class SlackMessageOnNewOctopusSubscriptionEventReceivedHandler : INotificationHandler<NewOctopusSubscriptionEventReceived>
    {
        public Task Handle(NewOctopusSubscriptionEventReceived notification, CancellationToken cancellationToken)
        {
            var octopusEvent = notification.SubscriptionEventEvent;

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
            OctopusSubscriptionEvent octopusEvent)
        {
            var slackChannelName = Util.GetEnvironmentVariable("SLACK_CHANNEL_NAME");

            if (string.IsNullOrWhiteSpace(slackChannelName))
            {
                throw new InvalidOperationException("SLACK_CHANNEL_NAME is not set!");
            }

            var slackMsg = new SlackPayload
            {
                Channel = slackChannelName,
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Color = "#5C91DC",
                        Fallback = message,
                        Footer = username,
                        FooterIcon =
                            "https://emojipedia-us.s3.amazonaws.com/thumbs/240/microsoft/106/face-with-cowboy-hat_1f920.png",
                        Pretext = category,
                        Text = message,
                        SecondsSinceUnixEpoch = ToUnixTime(octopusEvent.Payload.Event.Occurred.ToUniversalTime())
                    }
                }
            };

            return slackMsg;
        }

        private static string SendToSlack(
            SlackPayload o,
            string slackHookUrl)
        {
            try
            {
                var hookJson = JsonConvert.SerializeObject(o, Formatting.None);
                Log.Information("Posting Slack json {slackMessageJson} to {SlackWebHookUrl}", hookJson, slackHookUrl);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(slackHookUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 30000;
                httpWebRequest.ReadWriteTimeout = 30000;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(hookJson);
                    streamWriter.Flush();
                }

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to send message to Slack.");
                throw;
            }
        }

        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}