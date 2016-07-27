namespace DeckOMatic
{
    using System;

    public static class MatchRate
    {
        /// <summary>
        /// Calculate the match rate between the deck and the cluster
        /// </summary>
        public static double Calculate(Cluster cluster, PartialDeck deck)
        {
            if (cluster.Count == 0 || deck.Count == 0)
            {
                return 1.0;
            }

            double intersectionCount = (double)cluster.Intersect(deck).Count;
            double matchRate = intersectionCount / Math.Min(cluster.Count, deck.Count);
            return matchRate;
        }
    }
}
