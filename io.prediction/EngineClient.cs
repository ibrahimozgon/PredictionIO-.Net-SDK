using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace prediction.io
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
        private const string EngineQ = "/queries.json";

        /// <summary>
        ///     Instantiates a PredictionIO RESTful API Engine Client using default values for API URL
        ///     and default values in BaseClient.
        ///     The default API URL is http://localhost:8000.  
        /// </summary>
        public EngineClient(string accesKey) :
            base(DefaultEngineUrl, accesKey)
        { }

        /// <summary>
        ///     Instantiates a PredictionIO RESTful API Engine Client using default values in BaseClient.
        /// </summary>
        /// <param name="accesKey"></param>
        /// <param name="engineUrl"> the URL of the PredictionIO API</param>
        public EngineClient(string engineUrl, string accesKey)
            : base(engineUrl, accesKey)
        {
        }


        /// <summary>
        ///     Sends a query synchronously.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public T SendQuery<T>(Dictionary<string, object> query)
        {
            return Execute<T>(EngineQ, Method.POST, query);
        }

        /// <summary>
        ///  Sends a query asynchronously.
        /// </summary>
        /// <param name="query">dictionary </param>
        /// <returns></returns>
        public async Task<T> SendQueryAsync<T>(Dictionary<string, object> query)
        {
            return await ExecuteAsync<T>(EngineQ, Method.POST, query);
        }

        public ItemScoresRootObject Get(
            string userId,
            long count = 20,
            IEnumerable<string> categories = null,
            IEnumerable<string> blackList = null,
            IEnumerable<string> whiteList = null)
        {
            var query = new Dictionary<string, object>
            {
                {"user", userId},
                {"num", count},
            };
            if (categories != null)
                query.Add("categories", categories);
            if (blackList != null)
                query.Add("blackList", blackList);
            if (whiteList != null)
                query.Add("whiteList", whiteList);
            return SendQuery<ItemScoresRootObject>(query);
        }
        public async Task<ItemScoresRootObject> GetAsync(
            string userId,
            long count = 20,
            IEnumerable<string> categories = null,
            IEnumerable<string> blackList = null,
            IEnumerable<string> whiteList = null)
        {
            var query = new Dictionary<string, object>
            {
                {"user", userId},
                {"num", count},
            };
            if (categories != null)
                query.Add("categories", categories);
            if (blackList != null)
                query.Add("blackList", blackList);
            if (whiteList != null)
                query.Add("whiteList", whiteList);
            return await SendQueryAsync<ItemScoresRootObject>(query);
        }
    }

}