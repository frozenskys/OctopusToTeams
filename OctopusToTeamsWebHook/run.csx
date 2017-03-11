#load "microsoftTeamsMessageCard.csx"
#r "Newtonsoft.Json"

using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
 
// Version 0.3.7
public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    string jsonContent = await req.Content.ReadAsStringAsync();

    dynamic data = JsonConvert.DeserializeObject(jsonContent);
    log.Info(JsonConvert.SerializeObject(data));
    
    if (data.Payload?.Event?.Message == null) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Payload.Event.Message missing from body."
        });
    }

    var appKey = "TeamsWebHookUrl";
    var webHookUrl = ConfigurationManager.AppSettings[appKey];

    var summary = data.Payload.Event.Message;
    var username = data.Payload.Event.Username;
    var occurred = data.Payload.Event.Occurred;
    string category = data.Payload.Event.Category;
    var serverUri = data.Payload.ServerUri;
    var deployment = data.Payload.Event.MessageReferences[0].ReferencedDocumentId;

    var title = Regex.Replace(category, "([A-Z]{1,2}|[0-9]+)", " $1").TrimStart();

    var messageCard = new MicrosoftTeamsMessageCard {
        title = $"Octopus {title}",
        summary = $"{summary}",
        //text = $"{summary}",
        sections = new [] {
            new MicrosoftTeamsMessageSection {
                activityTitle = title,
                activityImage = $"https://raw.githubusercontent.com/frozenskys/OctopusToTeams/master/{category}.png",
                activityText = $"{summary}",
                facts = new [] {
                    new MicrosoftTeamsMessageFacts { name ="User", value = $"{username}" },
                },
            }
        },
        potentialAction = new [] { 
                new MicrosoftTeamsMessagePotentialAction {
                    name = "View Deployment",
                    target = new []{$"{serverUri}api/deployments/{deployment}"}
                }
        }
    };
    
    var ret = "";
    using (var client = new HttpClient())
    {
        await client.PostAsJsonAsync(webHookUrl, messageCard);
        log.Info(JsonConvert.SerializeObject(messageCard, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
    }

    return req.CreateResponse(HttpStatusCode.OK, messageCard);
}