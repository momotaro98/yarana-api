#r "Newtonsoft.Json"

using System.Net;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, ICollector<Koto> kotos, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");    
    dynamic data = await req.Content.ReadAsAsync<object>();
    dynamic koto = JObject.Parse(data.ToString());
    if (koto.id == null || koto.userId == null || koto.title == null)
    {
        log.Info($"Request doesn't have needed key. Request body {data.ToString()}");
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass valid data in request body.");
    }
    kotos.Add(
            new Koto() { 
                PartitionKey = "Kotos", 
                RowKey = koto.id,
                userId = koto.userId,
                title = koto.title }
            );
    log.Info($"Added data {koto.ToString()}");
    return req.CreateResponse(HttpStatusCode.OK);
}

public class Koto
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string userId { get; set; }
    public string title { get; set; }
}