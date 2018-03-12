#r "Newtonsoft.Json"

using System.Net;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IAsyncCollector<dynamic> activityDocument, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");  
    dynamic data = await req.Content.ReadAsAsync<object>();
    dynamic activity = JObject.Parse(data.ToString());
    if (activity.id == null || activity.kotoId == null || activity.timestamp == null)
    {
        log.Info($"Request doesn't have needed key. Request body {data.ToString()}");
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass valid data in request body.");
    }
    activityDocument.AddAsync(activity);
    log.Info($"Added data {activity.ToString()}");
    return req.CreateResponse(HttpStatusCode.OK);
}