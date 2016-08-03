namespace DeckOMatic
{
    using System.Collections.Generic;

    public class ClusteringStrategy
    {
        private ClusterDetector clusterDetector;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Cluster options</param>
        public ClusteringStrategy(ClusterOptions options)
        {
            this.clusterDetector = new ClusterDetector(options);
        }

        /// <summary>
        /// Generates the set of clusters
        /// </summary>
        /// <param name="decks">Collection of partial decks from which to generate cluster</param>
        /// <returns>List of clusters</returns>
        public List<Cluster> GenerateClusters(List<PartialDeck> decks)
        {
            return this.clusterDetector.GenerateInitialClusters(decks);
        }
    }
}
