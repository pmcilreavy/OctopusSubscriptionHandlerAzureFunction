# Octopus Subscription Handler Azure Function
An Azure function that can handle [Octopus Deploy subscription](https://octopus.com/docs/administration/subscriptions) event messages and forward them on to a Slack channel.

Octopus Deploy generates an "event" when things happen (e.g. a failed or successful deployment). You can nominate a url that Octopus will POST the json data of the subscription event to. The json is in a propietary format which can't be understood easily by the typical places you'd want to send such data to (e.g. Slack). 

You can [use an intermediary like Zapier](https://octopus.com/blog/subscriptions) to do the transform but it's a bit of a hassle and the free tier is limited to sending only every 15 minutes. 

This Azure function can be configured as the receiver of the Octopus event and will in-turn parse the json, re-format it into the Slack format and forward on to a nominated Slack web hook url. 

![Slack Screenshot](https://github.com/pmcilreavy/OctopusSubscriptionHandlerAzureFunction/blob/master/slack_screenshot.png)

I needed a Slack connector but it would be trivial to implement other connectors.
