namespace DeckOMatic
{
    using System;

    public class Cluster
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Cluster()
        {
            this.Cards = new CardSet();
        }

        /// <summary>
        /// Cards included in the cluster
        /// </summary>
        public CardSet Cards { get; private set; }
    }
}
