#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Table;

public static HttpResponseMessage Run(HttpRequestMessage req, IQueryable<Koto> kotos, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // Query data and convert to User model
    var userEntitiesByQuery = from koto in kotos
        select new
        {
            id = koto.userId
        };
    // Remove duplicates becuase users have several kotos
    var users = userEntitiesByQuery.ToList().Distinct();

    // Create JSON to return
    string responseJSON = JsonConvert.SerializeObject(users);
    log.Info($"JSON to response: {responseJSON}");

    // Create response
    var response = req.CreateResponse(HttpStatusCode.OK);
    response.Content = new StringContent(responseJSON);
    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    response.Content.Headers.ContentType.CharSet = "utf-8";
    return response;
}

/* When we need more user properties, we use User entity
public class User
{
    public string id { get; set; }
}
*/

public class Koto : TableEntity  // TODO: Integrate Koto entity with GetKotos function
{
    public string userId { get; set; }
    public string title { get; set; }
}