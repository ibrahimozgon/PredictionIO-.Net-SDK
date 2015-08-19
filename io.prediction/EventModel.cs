using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace io.prediction
{
    public class EventModel
    {
        public EventModel()
        {
            EventTime = DateTime.Now;
        }
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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ItemScore
    {   
        public string Item { get; set; }
        public double Score { get; set; }
    }

    public class ItemScoresModel
    {
        public IList<ItemScore> ItemScores { get; set; }
    }

}