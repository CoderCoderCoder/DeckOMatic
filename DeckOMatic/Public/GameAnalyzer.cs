﻿namespace DeckOMatic
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
        /// <returns>DeckOMaticDefinition object</returns>
        public DeckOMaticDefinition Run(List<Game> games)
        {
            var decks = new DeckCollection(games);
            var cluster = new Cluster();
            var clusteringStrategy = new ClusteringStrategy();
            var warriorDecks = decks.GetDecksForHero(Hero.Warrior);
            clusteringStrategy.GenerateCluster(cluster, warriorDecks, decks.Count);

            return null;
        }
    }
}