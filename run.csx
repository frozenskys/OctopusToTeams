#r "Newtonsoft.Json"

using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("Webhook was triggered!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    log.Info(jsonContent);

    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    if (data.Payload?.Event?.Message == null) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Payload.Event.Message missing from body."
        });
    }

    var message = data.Payload.Event.Message;
    var occurred = data.Payload.Event.Occurred;

    log.Info("* " + message);
    log.Info("* " + occurred);

    // make call to Teams WebHook Url, which is stored in the app settings
    // https://outlook.office365.com/webhook/cb67..4b/IncomingWebhook/93...
    var appKey = "TeamsWebHookUrl";
    var webHookUrl = ConfigurationManager.AppSettings[appKey];
    log.Info($"App Setting. {appKey}: {webHookUrl}");

    // Payload content at https://dev.outlook.com/Connectors/GetStarted
    var body = new { text = $"{message} {occurred}" };
    using (var client = new HttpClient())
    {
        await client.PostAsJsonAsync(webHookUrl, body);
        log.Info("Sent the JSON payload to Teams WebHook!");
    }

    return req.CreateResponse(HttpStatusCode.OK);
}