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
            return true;
        }
    }
}
