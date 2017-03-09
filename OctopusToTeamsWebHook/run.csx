#load "microsoftTeamsMessageCard.csx"
#r "Newtonsoft.Json"

using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
 
// Version 0.3.0
public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    string jsonContent = await req.Content.ReadAsStringAsync();

    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    if (data.Payload?.Event?.Message == null) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Payload.Event.Message missing from body."
        });
    }

    var message = data.Payload.Event.Message;
    var occurred = data.Payload.Event.Occurred;
    string category = data.Payload.Event.Category;
    var s = Regex.Replace(category, "([A-Z]{1,2}|[0-9]+)", " $1").TrimStart();
    var appKey = "TeamsWebHookUrl";
    var webHookUrl = ConfigurationManager.AppSettings[appKey];

    var messageCard = new MicrosoftTeamsMessageCard {
        title = $"Octopus {s}",
        summary = $"{message}",
        potentialAction = new [] { 
                new MicrosoftTeamsMessagePotentialAction {
                    name = "View Deployment",
                    target = new []{"https://trello.com/c/1101/"}
                }
        }
    };
    
    using (var client = new HttpClient())
    {
        //await client.PostAsJsonAsync(webHookUrl, messageCard);
        log.Info(JsonConvert.SerializeObject(messageCard));
    }

    return req.CreateResponse(HttpStatusCode.OK);
}