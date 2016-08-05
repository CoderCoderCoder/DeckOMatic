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
            return null;
        }
    }
}
