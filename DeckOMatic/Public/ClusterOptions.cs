namespace DeckOMatic
{
    public class ClusterOptions
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// All parameters start with reasonable default values but can be overridden to customize the clustering process.
        /// </remarks>
        public ClusterOptions()
        {
            // Default values
            this.MinimumMatchRate = 0.75;
        }

        /// <summary>
        /// Minimum match rate
        /// </summary>
        /// <remarks>
        /// When building up the cluster, this is the minimum the match rate in order to consider a deck part of the cluster.
        /// </remarks>
        public double MinimumMatchRate { get; set; }
    }
}
