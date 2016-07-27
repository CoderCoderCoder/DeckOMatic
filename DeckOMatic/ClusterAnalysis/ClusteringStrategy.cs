namespace DeckOMatic
{
    using System;
    using System.Collections.Generic;
    using HearthDb;

    public class ClusteringStrategy
    {
        private ClusterOptions options;
        private DeckFilter deckFilter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Cluster options</param>
        public ClusteringStrategy(ClusterOptions options)
        {
            this.options = options;
            this.deckFilter = new DeckFilter(options);
        }

        /// <summary>
        /// Generates the cluster 
        /// </summary>
        /// <param name="cluster">Cluster to generate, possibly seeded with some initial cards</param>
        /// <param name="decks">Collection of partial decks from which to generate cluster</param>
        /// <param name="totalDecks">Total number of decks in sample (including those that may be associated with other clusters</param>
        /// <returns>True if a cluster was successfully generated</returns>
        public bool GenerateCluster(Cluster cluster, List<PartialDeck> decks, int totalDecks)
        {
            while (cluster.Count < 30)
            {
                Trace.Log("Processing card #{0} for cluster...", cluster.Count + 1);

                var filteredDecks = this.deckFilter.FilterByMatchRate(cluster, decks);
                Trace.Log("Filtered to {0} / {1} decks ({2:P2} of total sample).", filteredDecks.Count, decks.Count, (double)filteredDecks.Count / totalDecks);

                string nextCardId = this.GetNextCardForCluster(cluster, filteredDecks);
                cluster.Add(nextCardId);
                Trace.Log("Added card: {0}", Cards.All[nextCardId].Name);
                Trace.Log(String.Empty);
            }

            return true;
        }

        /// <summary>
        /// Get next card for cluster
        /// </summary>
        private string GetNextCardForCluster(Cluster cluster, List<DeckInfo> deckInfos)
        {
            // Account for every unmatched card, weighted by its deck's weight
            var cardWeights = new Dictionary<string, double>();
            foreach (var deckInfo in deckInfos)
            {
                // Deck weight is (match rate)^2, which gives higher priority to closer matches
                double deckWeight = deckInfo.MatchRate.Value * deckInfo.MatchRate.Value;

                var unmatchedCards = deckInfo.Deck.Except(cluster);
                foreach (string cardId in unmatchedCards)
                {
                    if (!cardWeights.ContainsKey(cardId))
                    {
                        cardWeights.Add(cardId, deckWeight);
                    }
                    else
                    {
                        cardWeights[cardId] += deckWeight;
                    }
                }
            }

            // Find the hightest weighted card
            var max = new KeyValuePair<string,double>(null, 0.0);
            foreach (var kvp in cardWeights)
            {
                if (kvp.Value > max.Value)
                {
                    max = kvp;
                }
            }

            return max.Key;
        }
    }
}
