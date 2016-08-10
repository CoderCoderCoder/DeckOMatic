namespace DeckOMaticShell
{
    using System;
    using System.IO;
    using DeckOMatic;
    using Newtonsoft.Json;

    public class Program
    {
        private static string logFile;

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            // Initialize the logger
            Program.logFile = args[1];
            Trace.LogWritten += Program.OnLogWritten;

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

            string filename = "c:\\cob\\clusters.txt";
            using (StreamWriter file = File.CreateText(filename))
            {
                var serializer = Serialization.GetSerializer(true);
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, definition);
            }

            System.Diagnostics.Process.Start(filename);
            System.Diagnostics.Process.Start(Program.logFile);
        }

        /// <summary>
        /// Write trace logs to the console and a file
        /// </summary>
        private static void OnLogWritten(string message)
        {
            Console.WriteLine(message);
            File.AppendAllText(Program.logFile, message + Environment.NewLine);
        }
    }
}
