namespace DeckOMatic
{
    using FetchOBotApi;
    using Newtonsoft.Json;

    /// <summary>
    /// Extension of the FetchOBot Game class to include a few extra properties
    /// </summary>
    public class ExtendedGame : Game
    {
        [JsonProperty(PropertyName = "user_hash")]
        public string UserHash { get; set; }

        [JsonProperty(PropertyName = "region")]
        public BattleNetRegion Region { get; set; }
    }
}
