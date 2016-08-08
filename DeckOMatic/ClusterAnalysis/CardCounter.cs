namespace DeckOMatic
{
    using System.Collections.Generic;

    public class CardCounter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="decks">Decks from which to count cards</param>
        public CardCounter(List<PartialDeck> decks)
        {
            this.InstanceCounts = this.GetInstanceCounts(decks);
        }

        /// <summary>
        /// Card instance counts
        /// </summary>
        public List<InstanceCount> InstanceCounts
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the raw instance counts from the decks
        /// </summary>
        private List<InstanceCount> GetInstanceCounts(List<PartialDeck> decks)
        {
            // Count cards from every deck
            var rawCounts = new Dictionary<string, List<InstanceCount>>();
            foreach (var deck in decks)
            {
                foreach (string cardId in deck.RawCounts.Keys)
                {
                    // Add an entry for the card, if necessary
                    if (!rawCounts.ContainsKey(cardId))
                    {
                        rawCounts.Add(cardId, new List<InstanceCount>());
                    }

                    // Update the appropriate entry for each instance of the card
                    var instanceCounts = rawCounts[cardId];
                    for (int i = 0; i < deck.RawCounts[cardId]; i++)
                    {
                        if (i >= instanceCounts.Count)
                        {
                            instanceCounts.Add(new InstanceCount()
                                {
                                    CardId = cardId,
                                    Instance = i + 1,
                                    Count = 1
                                });
                        }
                        else
                        {
                            instanceCounts[i].Count++;
                        }
                    }
                }
            }

            // Convert to a list
            var listOfInstanceCounts = new List<InstanceCount>();
            foreach (var instanceCounts in rawCounts.Values)
            {
                listOfInstanceCounts.AddRange(instanceCounts);
            }

            // Sort the list, most frequent to least
            listOfInstanceCounts.Sort((a, b) => { return b.Count.CompareTo(a.Count); });
            return listOfInstanceCounts;
        }

        /// <summary>
        /// Helper class to keep count of a particular instance of a card
        /// </summary>
        public class InstanceCount
        {
            public string CardId { get; set; }
            public int Instance { get; set; }
            public int Count { get; set; }
        }
    }
}
