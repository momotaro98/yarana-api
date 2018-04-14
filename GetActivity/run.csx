#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Table;

public static HttpResponseMessage Run(HttpRequestMessage req, IQueryable<Activity> activities, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // Parse url query parameter
    // ex. https://yarana-api.azurewebsites.net/api/activities?kotoId=k69964bb713fd6f4f5ef6a7c7e029388
    string kotoId = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "kotoId", true) == 0)
        .Value;
    if (kotoId == null)
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a kotoId on the query string or in the request body");
    log.Info($"kotoId: {kotoId.ToString()}");

    // Query
    var query = from activity in activities
        where activity.kotoId == kotoId
        select new
        {
            id = activity.RowKey,
            kotoId = activity.kotoId,
            timestamp = activity.timestamp
        };

    // Create JSON to return
    string responseJSON = JsonConvert.SerializeObject(query);
    log.Info($"JSON to response: {responseJSON}");

    // Create response
    var response = req.CreateResponse(HttpStatusCode.OK);
    response.Content = new StringContent(responseJSON);
    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    response.Content.Headers.ContentType.CharSet = "utf-8";
    return response;
}

public class Activity : TableEntity
{
    public string kotoId { get; set; }
    public DateTime timestamp { get; set; }
}