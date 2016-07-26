namespace DeckOMatic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using HearthDb;

    /// <summary>
    /// A CardSet is a collections of card IDs.  It may have duplicates.
    /// </summary>
    public class CardSet : IEnumerable<string>
    {
        private Dictionary<string, int> cards = new Dictionary<string, int>();

        /// <summary>
        /// Constructor
        /// </summary>
        public CardSet()
        {
            this.Count = 0;
        }

        /// <summary>
        /// Count of cards in the set
        /// </summary>
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// Add a card
        /// </summary>
        /// <param name="cardId">Card ID</param>
        public void Add(string cardId)
        {
            this.Add(cardId, 1);
        }

        /// <summary>
        /// Add cards
        /// </summary>
        /// <param name="cardId">Card ID</param>
        /// <param name="count">Number to add</param>
        private void Add(string cardId, int count)
        {
            if (!this.cards.ContainsKey(cardId))
            {
                this.cards.Add(cardId, count);
            }
            else
            {
                this.cards[cardId] += count;
            }

            this.Count += count;
        }

        /// <summary>
        /// Returns an enumerator of the cards in this set
        /// </summary>
        public IEnumerator<string> GetEnumerator()
        {
            foreach (string cardId in this.cards.Keys)
            {
                for (int i = 0; i < this.cards[cardId]; i++)
                {
                    yield return cardId;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator of the cards in this set
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns the intersection of two CardSets
        /// </summary>
        public CardSet Intersect(CardSet otherSet)
        {
            var intersection = new CardSet();
            foreach (string cardId in this.cards.Keys)
            {
                if (otherSet.cards.ContainsKey(cardId))
                {
                    intersection.Add(cardId, Math.Min(this.cards[cardId], otherSet.cards[cardId]));
                }
            }

            return intersection;
        }

        /// <summary>
        /// Returns a CardSet that contains the members of this set that do not appear in the other set
        /// </summary>
        public CardSet Except(CardSet otherSet)
        {
            var difference = new CardSet();
            foreach (string cardId in this.cards.Keys)
            {
                if (otherSet.cards.ContainsKey(cardId))
                {
                    if (otherSet.cards[cardId] < this.cards[cardId])
                    {
                        // The other set has less copies of this card, so include the difference
                        difference.Add(cardId, this.cards[cardId] - otherSet.cards[cardId]);
                    }
                }
                else
                {
                    // The other set has no copies of this card, so include all of them
                    difference.Add(cardId, this.cards[cardId]);
                }
            }

            return difference;
        }

        /// <summary>
        /// Override the ToString method
        /// </summary>
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (string cardId in this.cards.Keys)
            {
                s.AppendFormat("{0} ({1});", Cards.All[cardId].Name, this.cards[cardId]);
            }

            // Remove the extra semicolon at the end
            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
    }
}
