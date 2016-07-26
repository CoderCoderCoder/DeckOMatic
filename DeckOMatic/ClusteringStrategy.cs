namespace DeckOMatic
{
    using System;
    using System.Collections.Generic;

    public class ClusteringStrategy
    {
        /// <summary>
        /// Generates the cluster 
        /// </summary>
        /// <param name="cluster">Cluster to generate, possibly seeded with some initial cards</param>
        /// <param name="decks">Collection of partial decks</param>
        /// <returns>True if a cluster was successfully generated</returns>
        public bool GenerateCluster(Cluster cluster, List<PartialDeck> decks)
        {
            while (cluster.Count < 30)
            {
                string nextCardId = this.GetNextCardForCluster(cluster, decks);
                cluster.Add(nextCardId);
            }

            return true;
        }

        /// <summary>
        /// Get next card for cluster
        /// </summary>
        private string GetNextCardForCluster(Cluster cluster, List<PartialDeck> decks)
        {
            // Account for every unmatched card, weighted by its deck's weight
            var cardWeights = new Dictionary<string, double>();
            foreach (var deck in decks)
            {
                double deckWeight = this.GetDeckWeight(cluster, deck);
                var unmatchedCards = deck.Except(cluster);
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

        /// <summary>
        /// Calculate what the deck should be weighted based on how well it matches the cluster
        /// </summary>
        private double GetDeckWeight(Cluster cluster, PartialDeck deck)
        {
            if (cluster.Count == 0 || deck.Count == 0)
            {
                return 1.0;
            }

            double intersectionCount = (double)cluster.Intersect(deck).Count;
            double matchRate = intersectionCount / Math.Min(cluster.Count, deck.Count);

            // Filter out decks that match below the threshold
            if (matchRate < 0.75)
            {
                return 0.0;
            }

            return matchRate;
        }
    }
}
