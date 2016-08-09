namespace DeckOMatic
{
    using System;
    using System.Collections.Generic;
    using HearthDb;

    /// <summary>
    /// Representation of a diff between two CardSets
    /// </summary>
    public class CardSetDiff
    {
        /// <summary>
        /// Private constructor
        /// </summary>
        public CardSetDiff(CardSet removed, CardSet added)
        {
            this.Removed = removed;
            this.Added = added;
        }

        /// <summary>
        /// Cards removed
        /// </summary>
        public CardSet Removed { get; private set; }

        /// <summary>
        /// Cards added
        /// </summary>
        public CardSet Added { get; private set; }

        /// <summary>
        /// Override the ToString method
        /// </summary>
        public override string ToString()
        {
            var differences = new List<string>();
            foreach (string cardId in this.Removed.RawCounts.Keys)
            {
                differences.Add(String.Format("-{0} ({1})", Cards.All[cardId].Name, this.Removed.RawCounts[cardId]));
            }

            foreach (string cardId in this.Added.RawCounts.Keys)
            {
                differences.Add(String.Format("+{0} ({1})", Cards.All[cardId].Name, this.Added.RawCounts[cardId]));
            }

            return String.Join("; ", differences);
        }
    }
}
