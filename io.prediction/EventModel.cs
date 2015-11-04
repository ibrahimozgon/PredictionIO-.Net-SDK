using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace predictionIO
{
    /// <summary>
    ///     Model to send data to event server
    /// </summary>
    public partial class EventModel
    {
        // mandatory fields
        [JsonProperty(PropertyName = "event")]
        public string EventValue { get; set; }
        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }
        [JsonProperty(PropertyName = "entityId")]
        public string EntityId { get; set; }

        // optional fields
        [JsonProperty(PropertyName = "targetEntityType")]
        public string TargetEntityType { get; set; }
        [JsonProperty(PropertyName = "targetEntityId")]
        public string TargetEntityId { get; set; }
        [JsonProperty(PropertyName = "properties")]
        public Dictionary<string, object> Properties { get; set; }
        [JsonProperty(PropertyName = "eventTime")]
        public DateTime EventTime { get; set; }
    }
}