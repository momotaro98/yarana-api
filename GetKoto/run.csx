using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IEnumerable<dynamic> documents, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // parse url query parameter
    // ex. https://yarana-api.azurewebsites.net/api/kotos?userId=d59964bb713fd6f4f5ef6a7c7e029388
    string userId = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "userId", true) == 0)
        .Value;
    log.Info($"userId: {userId.ToString()}");

    // query from documents
    IEnumerable<dynamic> docs = documents.Where(doc => doc.userId == userId);

    // TODO: create JSON to return

    foreach (var doc in docs)
    {
        log.Info(doc.ToString());
    }

    return userId == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a userId on the query string or in the request body")
        : req.CreateResponse(HttpStatusCode.OK, "Hello " + userId);
}
