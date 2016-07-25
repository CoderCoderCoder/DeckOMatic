namespace DeckOMatic
{
    using System.Collections.Generic;

    public class PartialDeck
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PartialDeck()
        {
            this.Cards = new List<string>();
        }

        /// <summary>
        /// Known cards in the deck
        /// </summary>
        public List<string> Cards { get; private set; }
    }
}
