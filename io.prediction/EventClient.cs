using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace io.prediction
{
    /// <summary>
    //      EventClient contains the generic methods createEvent() and getEvent() for importing and
    //      accessing events, as well as helper methods such as setUser(), unsetItem() and userActionItem()
    //      for convenience. Methods with an "AsFuture" suffix are asynchronous.
    //      Multiple simultaneous asynchronous requests is made possible by the high performance backend
    /// </summary>
    public class EventClient : BaseClient
    {
        /// <summary>
        ///     Default url
        /// </summary>
        private const string DefaultEventUrl = "http://localhost:7070";

        /// <summary>
        ///     the access key that this client will use to communicate with the API
        /// </summary>
        private readonly string _accessKey;

        /// <summary>
        //      Instantiate a PredictionIO RESTful API Event Client using default values for API URL
        //      and default values in {@link BaseClient}.
        //      <p>
        //      The default API URL is http://localhost:7070.   
        /// </summary>
        /// <param name="accessKey">the access key that this client will use to communicate with the API</param>
        public EventClient(string accessKey)
            : this(accessKey, DefaultEventUrl)
        {
        }

        /// <summary>
        ///    Instantiate a PredictionIO RESTful API Event Client using default values in 
        /// </summary>
        /// <param name="accessKey">the access key that this client will use to communicate with the API</param>
        /// <param name="eventUrl"> eventURL the URL of the PredictionIO API</param>
        public EventClient(string accessKey, string eventUrl)
            : base(eventUrl)
        {
            _accessKey = accessKey;
        }

        /// <summary>
        ///     Sends an asynchronous create event request to the API.
        /// </summary>
        /// <param name="event">event an instance of {@link Event} that will be turned into a request</param>
        /// <returns>Async task result</returns>
        public Task<HttpResponseMessage> CreateEventAsync(EventModel @event)
        {
            return Client.PostAsJsonAsync("/events.json?accessKey=" + _accessKey, @event);
        }

        /// <summary>
        ///     Sends a synchronous create event request to the API.
        /// </summary>
        /// <param name="eventModel">event an instance of {@link Event} that will be turned into a request</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse CreateEvent(EventModel eventModel)
        {
            return GetResult<ApiResponse>(CreateEventAsync(eventModel));
        }

        /// <summary>
        ///     Sends an asynchronous get event request to the API.
        /// </summary>
        /// <param name="eid">eid ID of the event to get</param>
        /// <returns><see cref="Task"/></returns>
        public Task<HttpResponseMessage> GetEventAsync(string eid)
        {
            return Client.GetAsync("/events/" + eid + ".json?accessKey=" + _accessKey);
        }

        /// <summary>
        ///   Sends a synchronous get event request to the API.
        /// </summary>
        /// <param name="eid">eid ID of the event to get</param>
        /// <returns><see cref="EventModel"/></returns>
        public EventModel GetEvent(string eid)
        {
            return GetResult<EventModel>(GetEventAsync(eid));
        }

        /// <summary>
        ///     Sends a set user properties request. Implicitly creates the user if it's not already there.
        //      Properties could be empty.
        /// </summary>
        /// <param name="uid">ID of the user</param>
        /// <param name="properties">properties a map of all the properties to be associated with the user, could be empty</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> SetUserAsync(string uid, Dictionary<string, Object> properties, DateTime eventTime)
        {
            return CreateEventAsync(new EventModel
            {
                EventValue = "$set",
                EntityId = uid,
                EntityType = "user",
                EventTime = eventTime,
                Properties = properties
            });
        }


        /// <summary>
        //      Sends a set user properties request. Same as
        //      SetUserAsync(string, Dictionary, DateTime)
        //      except event time is not specified and recorded as the time when the function is called.
        /// </summary>
        /// <param name="uid">ID of the user</param>
        /// <param name="properties">properties a map of all the properties to be associated with the user, could be empty</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> SetUserAsync(string uid, Dictionary<string, Object> properties)
        {
            return SetUserAsync(uid, properties, DateTime.Now);
        }

        /// <summary>
        //      Sets properties of a user. Implicitly creates the user if it's not already there.
        //      Properties could be empty.
        /// </summary>
        /// <param name="uid"> ID of the user</param>
        /// <param name="properties"> a dictionary of all the properties to be associated with the user, could be empty</param>
        /// <param name="eventTime"> timestamp of the event</param>
        /// <returns>ID of this event<see cref="ApiResponse"/></returns>
        public ApiResponse SetUser(string uid, Dictionary<string, Object> properties, DateTime eventTime)
        {
            return GetResult<ApiResponse>(SetUserAsync(uid, properties, eventTime));
        }

        /// <summary>
        ///     Sets properties of a user. Same as SetUser(string, Dictionary, DateTime)
        //      except event time is not specified and recorded as the time when the function is called.
        /// </summary>
        /// <param name="uid"> ID of the user</param>
        /// <param name="properties"> a dictionary of all the properties to be associated with the user, could be empty</param>
        /// <returns>ID of this event<see cref="ApiResponse"/></returns>
        public ApiResponse SetUser(string uid, Dictionary<string, Object> properties)
        {
            return SetUser(uid, properties, DateTime.Now);
        }

        /// <summary>
        ///     Sends an unset user properties request. The list must not be empty.
        /// </summary>
        /// <param name="uid">ID of the user</param>
        /// <param name="properties"> a list of all the properties to unset</param>
        /// <param name="eventTime">timestamp of the event</param>
        /// <returns><see cref="Task"/></returns>
        public Task<HttpResponseMessage> UnsetUserAsync(string uid, List<string> properties,
                DateTime eventTime)
        {
            if (!properties.Any())
                throw new Exception("property list cannot be empty");
            // converts the list into a map (to empty string) before creating the event object
            Dictionary<string, object> propertyDictionary = properties.ToDictionary<string, string, object>(property => property, property => "");

            return CreateEventAsync(new EventModel
            {
                EventValue = "$unset",
                EntityType = "user",
                EntityId = uid,
                EventTime = eventTime,
                Properties = propertyDictionary
            });
        }

       
        /// <summary>
        //      Sends an unset user properties request. Same as
        //      UnsetUserAsync(string, List, DateTime)
        //      except event time is not specified and recorded as the time when the function is called.  
        /// </summary>
        /// <param name="uid">ID of the user</param>
        /// <param name="properties">a list of all the properties to unset</param>
        /// <returns><see cref="Task"/></returns>
        public Task<HttpResponseMessage> UnsetUserAsync(string uid, List<string> properties)
        {
            return UnsetUserAsync(uid, properties, DateTime.Now);
        }

        /**
         * Unsets properties of a user. The list must not be empty.
         *
         * @param uid ID of the user
         * @param properties a list of all the properties to unset
         * @param eventTime timestamp of the event
         * @return ID of this event
         */
        public ApiResponse UnsetUser(string uid, List<string> properties, DateTime eventTime)
        {
            return GetResult<ApiResponse>(UnsetUserAsync(uid, properties, eventTime));
        }

        /**
         * Unsets properties of a user. Same as {@link #unsetUser(string, List, DateTime)
         * unsetUser(string, List&lt;string&gt;, DateTime)}
         * except event time is not specified and recorded as the time when the function is called.
         */
        public ApiResponse UnsetUser(string uid, List<string> properties)
        {
            return UnsetUser(uid, properties, DateTime.Now);
        }

        /**
         * Sends a delete user request.
         *
         * @param uid ID of the user
         * @param eventTime timestamp of the event
         */
        public Task<HttpResponseMessage> DeleteUserAsFuture(string uid, DateTime eventTime)
        {
            return CreateEventAsync(
                new EventModel
                {
                    EventValue = "$delete",
                    EntityId = uid,
                    EntityType = "user",
                    EventTime = eventTime
                }
                );
        }

        /**
         * Sends a delete user request. Event time is recorded as the time when the function is called.
         *
         * @param uid ID of the user
         */
        public Task<HttpResponseMessage> DeleteUserAsFuture(string uid)
        {
            return DeleteUserAsFuture(uid, DateTime.Now);
        }

        /**
         * Deletes a user.
         *
         * @param uid ID of the user
         * @param eventTime timestamp of the event
         * @return ID of this event
         */
        public ApiResponse DeleteUser(string uid, DateTime eventTime)
        {
            return GetResult<ApiResponse>(DeleteUserAsFuture(uid, eventTime));
        }

        /**
         * Deletes a user. Event time is recorded as the time when the function is called.
         *
         * @param uid ID of the user
         * @return ID of this event
         */
        public ApiResponse DeleteUser(string uid)
        {
            return DeleteUser(uid, DateTime.Now);
        }


        /**
         * Sends a set item properties request. Implicitly creates the item if it's not already there.
         * Properties could be empty.
         *
         * @param iid ID of the item
         * @param properties a map of all the properties to be associated with the item, could be empty
         * @param eventTime timestamp of the event
         * @return ID of this event
         */
        public Task<HttpResponseMessage> SetItemAsFuture(string iid, Dictionary<string, Object> properties,
                DateTime eventTime)
        {
            return CreateEventAsync(new EventModel
            {
                EventValue = "$set",
                EntityId = iid,
                EntityType = "item",
                EventTime = eventTime,
                Properties = properties
            });
        }

        /**
         * Sends a set item properties request. Same as
         * {@link #setItemAsFuture(string, Map, DateTime)
         * setItemAsFuture(string, Map&lt;string, Object&gt;, DateTime)}
         * except event time is not specified and recorded as the time when the function is called.
         */
        public Task<HttpResponseMessage> SetItemAsFuture(string iid, Dictionary<string, Object> properties)
        {
            return SetItemAsFuture(iid, properties, DateTime.Now);
        }

        /**
         * Sets properties of a item. Implicitly creates the item if it's not already there.
         * Properties could be empty.
         *
         * @param iid ID of the item
         * @param properties a map of all the properties to be associated with the item, could be empty
         * @param eventTime timestamp of the event
         * @return ID of this event
         */
        public ApiResponse SetItem(string iid, Dictionary<string, Object> properties, DateTime eventTime)
        {
            return GetResult<ApiResponse>(SetItemAsFuture(iid, properties, eventTime));
        }

        /**
         * Sets properties of a item. Same as {@link #setItem(string, Map, DateTime)
         * setItem(string, Map&lt;string, Object&gt;, DateTime)}
         * except event time is not specified and recorded as the time when the function is called.
         */
        public ApiResponse SetItem(string iid, Dictionary<string, Object> properties)
        {
            return SetItem(iid, properties, DateTime.Now);
        }

        /**
         * Sends an unset item properties request. The list must not be empty.
         *
         * @param iid ID of the item
         * @param properties a list of all the properties to unset
         * @param eventTime timestamp of the event
         */
        public Task<HttpResponseMessage> UnsetItemAsFuture(string iid, List<string> properties,
                DateTime eventTime)
        {
            if (!properties.Any())
            {
                throw new Exception("property list cannot be empty");
            }
            // converts the list into a map (to empty string) before creating the event object
            var propertiesMap = properties.ToDictionary<string, string, object>(property => property, property => "");

            return CreateEventAsync(new EventModel
            {
                EventValue = "$unset",
                EntityType = "item",
                Properties = propertiesMap,
                EventTime = eventTime,
                EntityId = iid
            });
        }

        /**
         * Sends an unset item properties request. Same as
         * {@link #unsetItemAsFuture(string, List, DateTime)
         * unsetItemAsFuture(string, List&lt;string&gt;, DateTime)}
         * except event time is not specified and recorded as the time when the function is called.
         */
        public Task<HttpResponseMessage> UnsetItemAsFuture(string iid, List<string> properties)
        {
            return UnsetItemAsFuture(iid, properties, DateTime.Now);
        }

        /// <summary>
        ///     Unsets properties of a item. The list must not be empty.
        /// </summary>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="properties">properties a list of all the properties to unset</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse UnsetItem(string iid, List<string> properties, DateTime eventTime)
        {
            return GetResult<ApiResponse>(UnsetItemAsFuture(iid, properties, eventTime));
        }

        /// <summary>
        ///     Unsets properties of a item. The list must not be empty.
        /// </summary>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="properties">properties a list of all the properties to unset</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse UnsetItem(string iid, List<string> properties)
        {
            return UnsetItem(iid, properties, DateTime.Now);
        }

        /// <summary>
        ///     Sends a delete item request.
        /// </summary>
        /// <param name="iid"> iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="Task"/></returns>
        public Task<HttpResponseMessage> DeleteItemAsync(string iid, DateTime eventTime)
        {
            return CreateEventAsync(new EventModel
            {
                EventValue = "$delete",
                EntityType = "item",
                EventTime = eventTime,
                EntityId = iid,

            });
        }

        /// <summary>
        ///     Sends a delete item request.
        /// </summary>
        /// <param name="iid"> iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="Task"/></returns>
        public Task<HttpResponseMessage> DeleteItemAsync(string iid)
        {
            return DeleteItemAsync(iid, DateTime.Now);
        }

        /// <summary>
        ///     Deletes a item.
        /// </summary>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="eventTime">eventTime timestamp of the event</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse DeleteItem(string iid, DateTime eventTime)
        {
            return GetResult<ApiResponse>(DeleteItemAsync(iid, eventTime));
        }

        /// <summary>
        ///     Deletes a item.
        /// </summary>
        /// <param name="iid">iid ID of the item</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse DeleteItem(string iid)
        {
            return DeleteItem(iid, DateTime.Now);
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
        public Task<HttpResponseMessage> UserActionItemAsync(string action, string uid, string iid,
                Dictionary<string, Object> properties, DateTime eventTime)
        {
            return CreateEventAsync(new EventModel
            {
                EventValue = action,
                EntityType = "user",
                EventTime = eventTime,
                EntityId = uid,
                TargetEntityType = "item",
                Properties = properties,
                TargetEntityId = iid
            });
        }

        /// <summary>
        ///     Sends a user-action-on-item request.
        /// </summary>
        /// <param name="action">action name of the action performed</param>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="properties">properties a map of properties associated with this action</param>
        /// <returns><see cref="Task"/></returns>
        public Task<HttpResponseMessage> UserActionItemAsync(string action, string uid, string iid,
                Dictionary<string, Object> properties)
        {
            return UserActionItemAsync(action, uid, iid, properties, DateTime.Now);
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
                Dictionary<string, Object> properties, DateTime eventTime)
        {
            return GetResult<ApiResponse>(UserActionItemAsync(action, uid, iid, properties, eventTime));
        }

        /// <summary>
        ///     Records a user-action-on-item event.
        /// </summary>
        /// <param name="action">action name of the action performed</param>
        /// <param name="uid">uid ID of the user</param>
        /// <param name="iid">iid ID of the item</param>
        /// <param name="properties">properties a map of properties associated with this action</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public ApiResponse UserActionItem(string action, string uid, string iid,
                Dictionary<string, Object> properties)
        {
            return UserActionItem(action, uid, iid, properties, DateTime.Now);
        }
    }
}