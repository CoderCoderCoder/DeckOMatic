namespace DeckOMatic
{
    using System.Collections.Generic;

    public class DeckFilter
    {
        /// <summary>
        /// Filter decks to those that match at some minimum rate
        /// </summary>
        public List<DeckInfo> FilterByMatchRate(Cluster cluster, List<PartialDeck> decks)
        {
            var filteredDecks = new List<DeckInfo>();
            foreach (var deck in decks)
            {
                double matchRate = MatchRate.Calculate(cluster, deck);
                if (matchRate >= 0.75)
                {
                    filteredDecks.Add(
                        new DeckInfo
                        {
                            Deck = deck,
                            MatchRate = matchRate
                        });
                }
            }

            return filteredDecks;
        }
    }
}
