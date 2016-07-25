namespace DeckOMatic
{
    using System;
    using Newtonsoft.Json;

    public class CollectOBotDataSet
    {
        [JsonProperty(PropertyName = "range_start")]
        public DateTime RangeStart { get; set; }

        [JsonProperty(PropertyName = "range_end")]
        public DateTime RangeEnd { get; set; }

        [JsonProperty(PropertyName = "unique_users")]
        public int UniqueUsers { get; set; }

        [JsonProperty(PropertyName = "total_games")]
        public int TotalGames { get; set; }

        [JsonProperty(PropertyName = "games")]
        public ExtendedGame[] Games { get; set; }
    }
}
