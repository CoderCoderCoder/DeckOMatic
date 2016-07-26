﻿namespace DeckOMatic
{
    using System;

    public class PartialDeck
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PartialDeck()
        {
            this.Cards = new CardSet();
        }

        /// <summary>
        /// Known cards in the deck
        /// </summary>
        public CardSet Cards { get; private set; }
    }
}
