#r "Newtonsoft.Json"

using System.Net;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, ICollector<Activity> activities, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");  
    dynamic data = await req.Content.ReadAsAsync<object>();
    dynamic activity = JObject.Parse(data.ToString());
    if (activity.id == null || activity.kotoId == null || activity.timestamp == null)
    {
        log.Info($"Request doesn't have needed key. Request body {data.ToString()}");
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass valid data in request body.");
    }
    activities.Add(
            new Activity() { 
                PartitionKey = "Activities", 
                RowKey = activity.id,
                kotoId = activity.kotoId,
                timestamp = activity.timestamp }
            );
    log.Info($"Added data {activity.ToString()}");
    return req.CreateResponse(HttpStatusCode.OK);
}

public class Activity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string kotoId { get; set; }
    public DateTime timestamp { get; set; }
}