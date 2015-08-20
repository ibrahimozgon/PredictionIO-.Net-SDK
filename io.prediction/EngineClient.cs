using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace io.prediction
{
    /// <summary>
    ///     EngineClient contains generic methods sendQuery() and sendQueryAsFuture()
    ///     for sending queries.    
    /// </summary>
    public class EngineClient : BaseClient
    {
        /// <summary>
        ///     Default Engine Url
        /// </summary>
        private const string DefaultEngineUrl = "http://localhost:8000";

        /// <summary>
        ///     Instantiates a PredictionIO RESTful API Engine Client using default values for API URL
        ///     and default values in BaseClient.
        ///     The default API URL is http://localhost:8000.  
        /// </summary>
        public EngineClient() :
            base(DefaultEngineUrl)
        { }

        /// <summary>
        ///     Instantiates a PredictionIO RESTful API Engine Client using default values in BaseClient.
        /// </summary>
        /// <param name="engineUrl"> the URL of the PredictionIO API</param>
        public EngineClient(string engineUrl)
            : base(engineUrl)
        {
        }

        /// <summary>
        ///  Sends a query asynchronously.
        /// </summary>
        /// <param name="query">dictionary </param>
        /// <returns></returns>
        public Task<HttpResponseMessage> SendQueryAsync(Dictionary<string, Object> query)
        {
            return Client.PostAsJsonAsync("/queries.json", query);
        }

        /// <summary>
        ///     Sends a query synchronously.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public T SendQuery<T>(Dictionary<string, Object> query)
        {
            return GetResult<T>(SendQueryAsync(query));
        }
    }

}