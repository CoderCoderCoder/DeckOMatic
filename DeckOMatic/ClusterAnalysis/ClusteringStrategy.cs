namespace DeckOMatic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        /// Generates all clusters
        /// </summary>
        /// <param name="decks">Collection of partial decks from which to generate cluster</param>
        /// <returns>List of clusters</returns>
        public List<Cluster> GenerateClusters(List<PartialDeck> decks)
        {
            var clusters = new List<Cluster>();
            var remainingDecks = decks;
            while (true)
            {
                // Generate the next cluster
                var cluster = this.GenerateCluster(remainingDecks, decks.Count);

                // If we didn't find a cluster, we're done
                if (cluster == null)
                {
                    break;
                }

                // Add the cluster to the list
                clusters.Add(cluster);

                // Filter out decks that match that cluster
                remainingDecks = remainingDecks.Where((deck) => MatchRate.Calculate(cluster, deck) < this.options.MinimumMatchRate).ToList();
            }

            return clusters;
        }

        /// <summary>
        /// Generates the cluster
        /// </summary>
        /// <param name="decks">Collection of partial decks from which to generate cluster</param>
        /// <param name="totalSampleSize">Total size of the sample</param>
        /// <returns>The cluster, or null if no cluster was detected</returns>
        private Cluster GenerateCluster(List<PartialDeck> decks, int totalSampleSize)
        {
            var cluster = new Cluster();
            while (cluster.Count < 30)
            {
                Trace.Log("Processing card #{0} for cluster...", cluster.Count + 1);

                var filteredDecks = this.deckFilter.FilterByMatchRate(cluster, decks);
                double filteredDeckPercentage = (double)filteredDecks.Count / totalSampleSize;
                Trace.Log("Filtered to {0} / {1} decks ({2:P2} of sample).", filteredDecks.Count, totalSampleSize, filteredDeckPercentage);

                // If we didn't find enough decks to be considered a cluster, bail out
                if (filteredDeckPercentage < this.options.MinimumClusterSize)
                {
                    Trace.Log("Could not find a cluster.");
                    return null;
                }

                string nextCardId = this.GetNextCardForCluster(cluster, filteredDecks);
                cluster.Add(nextCardId);
                Trace.Log("Added card: {0}", Cards.All[nextCardId].Name);
                Trace.Log(String.Empty);
            }

            return cluster;
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
