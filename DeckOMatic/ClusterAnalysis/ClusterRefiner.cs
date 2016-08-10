namespace DeckOMatic
{
    using System.Collections.Generic;

    public class ClusterRefiner
    {
        private ClusterOptions options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Cluster options</param>
        public ClusterRefiner(ClusterOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Refine clusters
        /// </summary>
        /// <param name="clusters">Collection of clusters</param>
        /// <param name="decks">Collection of partial decks used to refine clusters</param>
        /// <returns>Refined collection of clusters</returns>
        public List<Cluster> RefineClusters(List<Cluster> clusters, List<PartialDeck> decks)
        {
            Trace.Log("");
            Trace.Log("*** Refining clusters ***");

            // Group decks into clusters
            var clusterGroups = this.MatchDecksToClusters(clusters, decks);

            // Generate the next iteration for each cluster
            foreach (Cluster cluster in clusterGroups.Keys)
            {
                Trace.Log("");
                Trace.Log("Refining cluster: " + cluster.ToString());

                var matchingDecks = clusterGroups[cluster];
                var cardCounter = new CardCounter(matchingDecks);
                this.TraceCardCounts(cardCounter, matchingDecks.Count);

                Cluster newCluster = new Cluster();
                for (int i = 0; i < 30; i++)
                {
                    newCluster.Add(cardCounter.InstanceCounts[i].CardId);
                }

                var diff = cluster.Diff(newCluster);
                Trace.Log("Diff: " + diff.ToString());
            }

            return null;
        }

        /// <summary>
        /// Match decks to clusters
        /// </summary>
        private Dictionary<Cluster, List<PartialDeck>> MatchDecksToClusters(List<Cluster> clusters, List<PartialDeck> decks)
        {
            var clusterGroups = new Dictionary<Cluster, List<PartialDeck>>();
            foreach (PartialDeck deck in decks)
            {
                // Find the cluster the deck best matches
                Cluster bestCluster = null;
                double bestMatchRate = 0.0;
                foreach (Cluster cluster in clusters)
                {
                    double matchRate = MatchRate.Calculate(cluster, deck);
                    if (matchRate > bestMatchRate)
                    {
                        bestCluster = cluster;
                        bestMatchRate = matchRate;
                    }
                }

                // If it doesn't match any deck well enough, just skip it
                if (bestMatchRate < this.options.MinimumMatchRate)
                {
                    continue;
                }

                // Associate the deck with the cluster it best matches
                if (!clusterGroups.ContainsKey(bestCluster))
                {
                    clusterGroups.Add(bestCluster, new List<PartialDeck>());
                }

                clusterGroups[bestCluster].Add(deck);
            }

            return clusterGroups;
        }

        /// <summary>
        /// Trace card counts
        /// </summary>
        private void TraceCardCounts(CardCounter cardCounter, int totalCount)
        {
            int i = 1;
            foreach (var instanceCount in cardCounter.InstanceCounts)
            {
                Trace.Log(
                    "{4}: {0} #{1}: {2} ({3:P2})",
                    HearthDb.Cards.All[instanceCount.CardId].Name,
                    instanceCount.Instance,
                    instanceCount.Count,
                    (double)instanceCount.Count / (double)totalCount,
                    i++);
            }
        }
    }
}
