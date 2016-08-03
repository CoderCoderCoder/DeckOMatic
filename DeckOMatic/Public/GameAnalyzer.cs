namespace DeckOMatic
{
    using System;
    using System.Collections.Generic;
    using FetchOBotApi;

    public class GameAnalyzer
    {
        /// <summary>
        /// Analyzes a collection of Track-o-Bot games and creates a DeckOMaticDefinition from them
        /// </summary>
        /// <param name="games">Collection of games</param>
        /// <param name="options">Cluster options</param>
        /// <returns>DeckOMaticDefinition object</returns>
        public DeckOMaticDefinition Run(List<Game> games, ClusterOptions options)
        {
            var decks = new DeckCollection(games);
            var clusteringStrategy = new ClusteringStrategy(options);
            var warriorDecks = decks.GetDecksForHero(Hero.Warrior);
            var clusters = clusteringStrategy.GenerateClusters(warriorDecks);

            return new DeckOMaticDefinition
            {
                Clusters = clusters.ToArray()
            };
        }
    }
}
