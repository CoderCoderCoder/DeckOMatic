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
            // Group decks into clusters
            var clusterGroups = this.MatchDecksToClusters(clusters, decks);

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
    }
}
