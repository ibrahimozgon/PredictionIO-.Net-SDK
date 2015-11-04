using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace predictionIO
{
    /// <summary>
    ///     BaseClient contains code common to both EventClient EngineClient.
    /// </summary>
    public abstract class BaseClient
    {
        /// <summary>
        ///     Default Timeout
        /// </summary>
        private const int DefaultTimeout = 5000;
        private const string JsonType = "application/json";
        private const string AccessKeyName = "accessKey";

        /// <summary>
        ///  API Url
        /// </summary>
        protected readonly string ApiUrl;

        /// <summary>
        ///     HttpClient
        /// </summary>
        protected readonly RestClient Client;

        /// <summary>
        ///     the access key that this client will use to communicate with the API
        /// </summary>
        protected string AccessKey { get; }

        /// <summary>
        ///  param apiURL the URL of the PredictionIO API
        /// </summary>
        protected BaseClient(string apiUrl, string accessKey)
            : this(apiUrl, accessKey, DefaultTimeout)
        {
        }

        /// <summary>
        /// param apiURL the URL of the PredictionIO API
        ///  param timeout timeout in seconds for the connections
        /// </summary>
        protected BaseClient(string apiUrl, string accessKey, int timeout)
        {
            ApiUrl = apiUrl;
            AccessKey = accessKey;
            Client = new RestClient(apiUrl) { Timeout = timeout };
        }

        public T Execute<T>(string resource, Method method, object body)
        {
            var request = new RestRequest(AppendAccessKey(resource)) { Method = method };
            request.AddHeader("Content-Type", JsonType);
            request.AddHeader("Accept", "application/json; charset=UTF-8");
            if (body != null)
                request.AddParameter(JsonType, JsonConvert.SerializeObject(body), ParameterType.RequestBody);
            var content = Client.Execute(request).Content;
            if (string.IsNullOrWhiteSpace(content))
            {
                return default(T);
            }
            return GetResponseAsJson<T>(content);
        }

        public async Task<T> ExecuteAsync<T>(string resource, Method method, object body)
        {
            var request = new RestRequest(AppendAccessKey(resource)) { Method = method };
            request.AddHeader("Content-Type", JsonType);
            request.AddHeader("Accept", "application/json; charset=UTF-8");
            if (body != null)
                request.AddParameter(JsonType, JsonConvert.SerializeObject(body),
                    ParameterType.RequestBody);
            var tcs = new TaskCompletionSource<IRestResponse>();
            Client.ExecuteAsync(request, response =>
            {
                if (response.ErrorException != null)
                    tcs.TrySetException(response.ErrorException);
                else
                    tcs.TrySetResult(response);
            });
            var content = (await tcs.Task.ConfigureAwait(false)).Content;
            if (string.IsNullOrWhiteSpace(content))
                return default(T);
            return GetResponseAsJson<T>(content);
        }

        public bool IsAlive()
        {
            var response = Execute<ServerCheck>("/", Method.GET, null);
            return response != null && response.Status == "alive";
        }

        private string AppendAccessKey(string resource)
        {
            if (AccessKey == null)
                throw new ArgumentException("Access Key can not be NULL");

            var builder = new StringBuilder(resource);
            builder.Append('?');
            builder.Append(AccessKeyName);
            builder.Append('=');
            builder.Append(AccessKey);
            return builder.ToString();
        }

        private static T GetResponseAsJson<T>(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch
            {
                throw new Exception(content);
            }
        }

        private class ServerCheck
        {
            public string Status { get; set; }
        }
    }
}