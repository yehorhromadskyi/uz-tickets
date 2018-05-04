using Newtonsoft.Json;
using System.Collections.Generic;

namespace UzTickets.Models
{
    public class Luis
    {
        public class Response
        {
            [JsonProperty("query")]
            public string Query { get; set; }

            [JsonProperty("topScoringIntent")]
            public Intent TopScoringIntent { get; set; }

            [JsonProperty("intents")]
            public List<Intent> Intents { get; set; }

            [JsonProperty("entities")]
            public List<Entity> Entities { get; set; }
        }

        public class Entity
        {
            [JsonProperty("entity")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("startIndex")]
            public long StartIndex { get; set; }

            [JsonProperty("endIndex")]
            public long EndIndex { get; set; }

            [JsonProperty("score")]
            public double Score { get; set; }
        }

        public class Intent
        {
            [JsonProperty("intent")]
            public string IntentIntent { get; set; }

            [JsonProperty("score")]
            public double Score { get; set; }
        }
    }
}
