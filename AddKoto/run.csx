#r "Newtonsoft.Json"

using System.Net;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IAsyncCollector<dynamic> kotoDocument, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");    
    dynamic data = await req.Content.ReadAsAsync<object>();
    dynamic koto = JObject.Parse(data.ToString());
    if (koto.id == null || koto.userId == null || koto.title == null)
    {
        log.Info($"Request doesn't have needed key. Request body {data.ToString()}");
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass valid data in request body.");
    }
    kotoDocument.AddAsync(koto);
    log.Info($"Added data {koto.ToString()}");
    return req.CreateResponse(HttpStatusCode.OK);
}