#r "Newtonsoft.Json"

using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

// Version 0.1.0
public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
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
    var appKey = "TeamsWebHookUrl";
    var webHookUrl = ConfigurationManager.AppSettings[appKey];
    var body = new { text = $"{message} {occurred}" };

    using (var client = new HttpClient())
    {
        await client.PostAsJsonAsync(webHookUrl, body);
    }

    return req.CreateResponse(HttpStatusCode.OK);
}