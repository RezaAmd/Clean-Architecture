using Infrastructure.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Common.Services
{
    //public class RestService : IRestService
    //{
    //    #region Constructor
    //    private readonly ILogger<RestService> logger;

    //    public RestService(ILogger<RestService> _logger)
    //    {
    //        logger = _logger;
    //    }
    //    #endregion

    //    public async Task<IRestResponse> RequestAsync(string url, Method method = Method.GET, RestConfig configs = default)
    //    {
    //        try
    //        {
    //            var client = new RestClient(url);
    //            client.Timeout = -1;
    //            var request = new RestRequest(method);
    //            if (configs != null)
    //            {
    //                #region Header
    //                if (configs.Headers != null)
    //                    foreach (var header in configs.Headers)
    //                        request.AddHeader(header.Key, header.Value);
    //                request.AddHeader("Content-Type", "application/json");
    //                // Auth
    //                if (!string.IsNullOrEmpty(configs.Authorization))
    //                    request.AddHeader("Authorization", "Bearer " + configs.Authorization);
    //                #endregion
    //                #region Parameter
    //                if (configs.Body != null)
    //                {
    //                    string bodySerialize = JsonConvert.SerializeObject(configs.Body);
    //                    request.AddParameter("application/json", bodySerialize, ParameterType.RequestBody);
    //                }
    //                foreach (var parameter in configs.Parameters)
    //                    request.AddParameter(parameter.Key, parameter.Value);
    //                #endregion
    //            }
    //            return await client.ExecuteAsync(request);
    //        }
    //        catch
    //        {
    //            logger.LogError($"Failed to request {url}");
    //            return default;
    //        }
    //    }
    //}

    //public class RestConfig
    //{
    //    #region Constructor
    //    public RestConfig(string authorization = default, object body = default, Dictionary<string, string> headers = default)
    //    {
    //        Headers = new Dictionary<string, string>();
    //        Parameters = new Dictionary<string, string>();
    //        Authorization = authorization;
    //        Body = body;
    //        Headers = headers;
    //    }
    //    #endregion
    //    public string Authorization { get; set; }
    //    public object Body { get; set; }
    //    public Dictionary<string, string> Headers { get; set; }
    //    public Dictionary<string, string> Parameters { get; set; }
    //}
}