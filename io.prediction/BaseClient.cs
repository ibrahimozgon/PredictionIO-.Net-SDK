using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace io.prediction
{
    public abstract class BaseClient : IDisposable
    {
        protected static readonly string DefaultApiVersion = "";
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(5);

        // HTTP status code
        protected const int HttpOk = 200;
        protected const int HttpCreated = 201;

        // API Url
        protected readonly string ApiUrl;

        protected readonly HttpClient Client;

        /// <summary>
        ///  param apiURL the URL of the PredictionIO API
        /// </summary>
        protected BaseClient(string apiUrl)
            : this(apiUrl, DefaultTimeout)
        {
        }

        /// <summary>
        /// param apiURL the URL of the PredictionIO API
        //  param timeout timeout in seconds for the connections
        /// </summary>
        protected BaseClient(string apiUrl, TimeSpan timeout)
        {
            ApiUrl = apiUrl;
            // Async HTTP client config
            Client = new HttpClient
            {
                BaseAddress = new Uri(apiUrl),
                Timeout = timeout,
                //MaxResponseContentBufferSize = threadLimit
            };
        }

        /// <summary>
        ///     Gets query result from a previously sent asynchronous request.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected T GetResult<T>(Task<HttpResponseMessage> response)
        {
            if (!response.Result.IsSuccessStatusCode)
                throw new Exception(response.Result.Content.ReadAsStringAsync().Result);

            var result = JsonConvert
                .DeserializeObject<T>(response.Result.Content.ReadAsStringAsync().Result);
            return result;
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}