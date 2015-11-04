using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace prediction.io
{
    /// <summary>
    ///      EventClient contains the generic methods createEvent() and getEvent() for importing and
    ///      accessing events, as well as helper methods such as setUser(), unsetItem() and userActionItem()
    ///      for convenience. Methods with an "Async" suffix are asynchronous.
    ///      Multiple simultaneous asynchronous requests is made possible by the high performance backend
    /// </summary>
    public class EventClient : BaseClient
    {
        /// <summary>
        ///     Default url
        /// </summary>
        private const string DefaultEventUrl = "http://localhost:7070";
        private const string EventQ = "/events.json";

        /// <summary>
        ///      Instantiate a PredictionIO RESTful API Event Client using default values for API URL
        ///      and default values in {@link BaseClient}.
        ///      The default API URL is http://localhost:7070.   
        /// </summary>
        /// <param name="accessKey">the access key that this client will use to communicate with the API</param>
        public EventClient(string accessKey)
            : base(DefaultEventUrl, accessKey)
        {
        }

        /// <summary>
        ///    Instantiate a PredictionIO RESTful API Event Client using default values in 
        /// </summary>
        /// <param name="accessKey">the access key that this client will use to communicate with the API</param>
        /// <param name="eventUrl"> eventURL the URL of the PredictionIO API</param>
        /// <param name="timeOut"></param>
        public EventClient(string accessKey, string eventUrl, int timeOut)
            : base(eventUrl, accessKey, timeOut)
        { }

        /// <summary>
        ///     Sends an asynchronous get event request to the API.
        /// </summary>
        /// <param name="eid">eid ID of the event to get</param>
        /// <returns><see cref="Task"/></returns>
        public async Task<EventModel> GetEventAsync(string eid)
        {
            return await ExecuteAsync<EventModel>(EventQ, Method.GET, eid);
        }

        /// <summary>
        ///   Sends a synchronous get event request to the API.
        /// </summary>
        /// <param name="eid">eid ID of the event to get</param>
        /// <returns><see cref="EventModel"/></returns>
        public EventModel GetEvent(string eid)
        {
            return Execute<EventModel>(EventQ, Method.GET, eid);
        }

        /// <summary>
        ///     Sends a set user properties request. Implicitly creates the user if it's not already there.
        ///      Properties could be empty.
        /// </summary>
        /// <param name="uid">ID of the user</param>
        /// <param name="properties">properties a map of all the properties to be associated with the user, could be empty</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns></returns>
        public async Task<ApiResponse> SetUserAsync(string uid, Dictionary<string, object> properties,
            DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$set",
                EntityId = uid,
                EntityType = "user",
                EventTime = eventTime,
                Properties = properties
            };
            return await ExecuteAsync<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///      Sets properties of a user. Implicitly creates the user if it's not already there.
        ///      Properties could be empty.
        /// </summary>
        /// <param name="uid"> ID of the user</param>
        /// <param name="properties"> a dictionary of all the properties to be associated with the user, could be empty</param>
        /// <param name="eventTime"> timestamp of the event</param>
        /// <returns>ID of this event<see cref="ApiResponse"/></returns>
        public ApiResponse SetUser(string uid, Dictionary<string, object> properties = null, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$set",
                EntityId = uid,
                EntityType = "user",
                EventTime = eventTime,
                Properties = properties
            };
            return Execute<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///     Sends an unset user properties request. The list must not be empty.
        /// </summary>
        /// <param name="uid">ID of the user</param>
        /// <param name="eventTime">timestamp of the event</param>
        /// <returns><see cref="Task"/></returns>
        public async Task<ApiResponse> UnsetUserAsync(string uid,
                DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$unset",
                EntityType = "user",
                EntityId = uid,
                EventTime = eventTime,
            };
            return await ExecuteAsync<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///     Unsets properties of a user. The list must not be empty.
        /// </summary>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse UnsetUser(string uid, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$unset",
                EntityType = "user",
                EntityId = uid,
                EventTime = eventTime,
            };
            return Execute<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///     Sends a delete user request.
        /// </summary>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="Task"/></returns>
        public async Task<ApiResponse> DeleteUserAsync(string uid, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$delete",
                EntityId = uid,
                EntityType = "user",
                EventTime = eventTime
            };
            return await ExecuteAsync<ApiResponse>(EventQ, Method.POST, model);

        }

        /// <summary>
        ///     Deletes a user.
        /// </summary>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse DeleteUser(string uid, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$delete",
                EntityId = uid,
                EntityType = "user",
                EventTime = eventTime
            };
            return Execute<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///      Sends a set item properties request. Implicitly creates the item if it's not already there.
        ///      Properties could be empty.  
        /// </summary>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="properties">properties a map of all the properties to be associated with the item, could be empty</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="Task"/></returns>
        public async Task<ApiResponse> SetItemAsync(string iid, Dictionary<string, object> properties,
                DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$set",
                EntityId = iid,
                EntityType = "item",
                EventTime = eventTime,
                Properties = properties
            };
            return await ExecuteAsync<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///      Sets properties of a item. Implicitly creates the item if it's not already there.
        ///      Properties could be empty.   
        /// </summary>
        /// <param name="iid"> iid ID of the item</param>
        /// <param name="properties">properties a map of all the properties to be associated with the item, could be empty</param>
        /// <param name="eventTime"> eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse SetItem(string iid, Dictionary<string, object> properties, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$set",
                EntityId = iid,
                EntityType = "item",
                EventTime = eventTime,
                Properties = properties
            };
            return Execute<ApiResponse>(EventQ, Method.POST, model);
        }

        public ApiResponse SetItemWithCategory(string iid, IEnumerable<string> categories = null, DateTime eventTime = default(DateTime))
        {
            var properties = new Dictionary<string, object>();
            if (categories != null)
                properties.Add("categories", categories);
            return SetItem(iid, properties);
        }

        public async Task<ApiResponse> SetItemWithCategoryAsync(string iid, IEnumerable<string> categories = null, DateTime eventTime = default(DateTime))
        {
            var properties = new Dictionary<string, object>();
            if (categories != null)
                properties.Add("categories", categories);
            return await SetItemAsync(iid, properties);
        }

        /// <summary>
        ///     Sends an unset item properties request. The list must not be empty.
        /// </summary>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="Task"/></returns>
        public async Task<ApiResponse> UnsetItemAsync(string iid, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$unset",
                EntityType = "item",
                EventTime = eventTime,
                EntityId = iid
            };
            return await ExecuteAsync<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///     Unsets properties of a item. The list must not be empty.
        /// </summary>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="properties">properties a list of all the properties to unset</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse UnsetItem(string iid, List<string> properties, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$unset",
                EntityType = "item",
                EventTime = eventTime,
                EntityId = iid
            };
            return Execute<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///     Sends a delete item request.
        /// </summary>
        /// <param name="iid"> iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="Task"/></returns>
        public async Task<ApiResponse> DeleteItemAsync(string iid, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$delete",
                EntityType = "item",
                EventTime = eventTime,
                EntityId = iid,

            };
            return await ExecuteAsync<ApiResponse>(EventQ, Method.POST, model);
            //return await CreateEventAsync();
        }

        /// <summary>
        ///     Deletes a item.
        /// </summary>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse DeleteItem(string iid, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = "$delete",
                EntityType = "item",
                EventTime = eventTime,
                EntityId = iid,

            };
            return Execute<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///     Sends a user-action-on-item request.
        /// </summary>
        /// <param name="action">action name of the action performed</param>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="properties">properties a map of properties associated with this action</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="Task"/></returns>
        public async Task<ApiResponse> UserActionItemAsync(string action, string uid, string iid,
                Dictionary<string, object> properties = null, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;
            var model = new EventModel
            {
                EventValue = action,
                EntityType = "user",
                EventTime = eventTime,
                EntityId = uid,
                TargetEntityType = "item",
                Properties = properties,
                TargetEntityId = iid
            };
            return await ExecuteAsync<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///     Records a user-action-on-item event.
        /// </summary>
        /// <param name="action">action name of the action performed</param>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="properties">properties a map of properties associated with this action</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse UserActionItem(string action, string uid, string iid,
                Dictionary<string, object> properties = null, DateTime eventTime = default(DateTime))
        {
            if (eventTime == default(DateTime))
                eventTime = DateTime.Now;

            var model = new EventModel
            {
                EventValue = action,
                EntityType = "user",
                EntityId = uid,
                TargetEntityType = "item",
                TargetEntityId = iid,
                Properties = properties,
                EventTime = eventTime,
            };
            return Execute<ApiResponse>(EventQ, Method.POST, model);
        }

        /// <summary>
        ///     Records a user-view event -on-item.
        /// </summary>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse UserViewedItem(string uid, string iid, DateTime eventTime = default(DateTime))
        {
            return UserActionItem("view", uid, iid);
        }

        /// <summary>
        ///     Records a user-view event -on-item.
        /// </summary>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UserViewedItemAsync(string uid, string iid, DateTime eventTime = default(DateTime))
        {
            return await UserActionItemAsync("view", uid, iid);
        }

        /// <summary>
        ///     Records a user-buy event -on-item.
        /// </summary>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse UserBoughtItem(string uid, string iid, DateTime eventTime = default(DateTime))
        {
            return UserActionItem("buy", uid, iid);
        }

        /// <summary>
        ///     Records a user-buy event -on-item.
        /// </summary>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UserBoughtItemAsync(string uid, string iid, DateTime eventTime = default(DateTime))
        {
            return await UserActionItemAsync("buy", uid, iid);
        }
    }
}