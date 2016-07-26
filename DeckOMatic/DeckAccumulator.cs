﻿namespace DeckOMatic
{
    using System.Collections.Generic;
    using FetchOBotApi;
    using HearthDb;
    using HearthDb.Enums;

    public class DeckCollection
    {
        private Dictionary<Hero, List<PartialDeck>> decks = new Dictionary<Hero, List<PartialDeck>>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="games">Set of games from which to accumulate decks</param>
        public DeckCollection(List<Game> games)
        {
            this.AccumulateDecks(games);
        }

        /// <summary>
        /// Accumulate opponent decks from a set of Track-o-Bot games
        /// </summary>
        public void AccumulateDecks(List<Game> games)
        {
            foreach (var game in games)
            {
                if (this.ShouldIncludeGame(game))
                {
                    // Create the list for that class, if necessary
                    if (!decks.ContainsKey(game.Opponent))
                    {
                        decks.Add(game.Opponent, new List<PartialDeck>());
                    }

                    decks[game.Opponent].Add(this.GetOpponentDeck(game));
                }
            }
        }

        /// <summary>
        /// Get the opponent deck
        /// </summary>
        private PartialDeck GetOpponentDeck(Game game)
        {
            var deck = new PartialDeck();
            foreach (var cardPlayed in game.CardHistory)
            {
                if (cardPlayed.Player == Player.Opponent &&
                    cardPlayed.Card.Id != CardIds.NonCollectible.Neutral.TheCoin)
                {
                    deck.Cards.Add(cardPlayed.Card.Id);
                }
            }

            return deck;
        }

        /// <summary>
        /// Determine whether a game should be included
        /// </summary>
        private bool ShouldIncludeGame(Game game)
        {
            // Filter out non-ranked
            if (game.Mode != Mode.Ranked)
            {
                return false;
            }

            // Filter out games with no history
            if (game.CardHistory.Length == 0)
            {
                return false;
            }

            // Filter out wild
            if (this.IsWild(game))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether a game is wild format
        /// </summary>
        private bool IsWild(Game game)
        {
            foreach (var cardPlayed in game.CardHistory)
            {
                var set = Cards.All[cardPlayed.Card.Id].Set;
                if (set == HearthDb.Enums.CardSet.PROMO || // Promos
                    set == HearthDb.Enums.CardSet.FP1 ||  // Nax
                    set == HearthDb.Enums.CardSet.PE1)  // GvG
                {
                    return true;
                }
            }

            return false;
        }
    }
}
