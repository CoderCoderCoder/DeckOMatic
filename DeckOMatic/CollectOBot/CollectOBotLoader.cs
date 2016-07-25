namespace DeckOMatic
{
    using System.Collections.Generic;
    using System.IO;
    using FetchOBotApi;
    using Newtonsoft.Json;

    public static class CollectOBotLoader
    {
        /// <summary>
        /// Load games from a single Collect-o-Bot data file
        /// </summary>
        /// <param name="filename">File name</param>
        /// <returns>List of games</returns>
        public static List<Game> LoadFromFile(string filename)
        {
            string rawJson = File.ReadAllText(filename);
            var dataSet = JsonConvert.DeserializeObject<CollectOBotDataSet>(rawJson);
            return new List<Game>(dataSet.Games);
        }

        /// <summary>
        /// Load games from all JSON files in the given directory
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <returns>List of games</returns>
        public static List<Game> LoadFromDirectory(string path)
        {
            var allGames = new List<Game>();
            var directory = new DirectoryInfo(path);
            foreach (var file in directory.GetFiles("*.json"))
            {
                allGames.AddRange(CollectOBotLoader.LoadFromFile(file.FullName));
            }

            return allGames;
        }
    }
}
