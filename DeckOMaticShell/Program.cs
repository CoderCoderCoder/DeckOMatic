namespace DeckOMaticShell
{
    using System;
    using System.IO;
    using DeckOMatic;

    public class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            // Initialize the logger
            Trace.LogWritten += (message) => Console.WriteLine(message);

            // Load games from file(s)
            string path = args[0];
            var games = Directory.Exists(path) ? CollectOBotLoader.LoadFromDirectory(path) : CollectOBotLoader.LoadFromFile(path);

            // Analyze games
            var analyzer = new GameAnalyzer();
            var definition = analyzer.Run(
                games,
                new ClusterOptions
                {
                    MinimumMatchRate = 0.75,
                });
        }
    }
}
