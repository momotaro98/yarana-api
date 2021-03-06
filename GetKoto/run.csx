#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Table;

public static HttpResponseMessage Run(HttpRequestMessage req, IQueryable<Koto> kotos, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // Parse url query parameter
    // ex. https://yarana-api.azurewebsites.net/api/kotos?userId=d59964bb713fd6f4f5ef6a7c7e029388
    string userId = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "userId", true) == 0)
        .Value;
    if (userId == null)
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a userId on the query string or in the request body");
    log.Info($"userId: {userId.ToString()}");

    // Query data
    var query = from koto in kotos
        where koto.userId == userId
        select new
        {
            id = koto.RowKey,
            userId = koto.userId,
            title = koto.title,
            pushDisabled = koto.pushDisabled
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

public class Koto : TableEntity
{
    public string userId { get; set; }
    public string title { get; set; }
    public bool pushDisabled { get; set; }
}