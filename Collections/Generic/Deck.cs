using System.Linq;
using System.Collections.Generic;
using UnityUtilities.Random;

namespace UnityUtilities {
    namespace Collections {
        namespace Generic {

            /// <summary>
            /// Interface to implement a card for Deck.
            /// </summary>
            public interface Card {
                /// <summary>
                /// Determine if current Card's properties match the criteria.
                /// This function will be used when searching the deck or
                /// discard for cards that match the criteria. T is intended to
                /// be a struct whose properties and/or property values are
                /// compared to that of the Card.
                /// </summary>
                /// <param name="criteria"></param>
                /// <returns></returns>
                bool MatchesCriteria<T>(T criteria);
            }

            /// <summary>
            /// Creates a "deck" and "discard" representation of Cards using
            /// queues for both. Type for deck must implement Card.
            /// </summary>
            /// <typeparam name="Card"></typeparam>
            public class Deck<Card> {

                private Queue<Generic.Card> deckQueue;
                private Queue<Generic.Card> discardQueue;

                /// <summary>
                /// Create a deck with a collection of Card.
                /// </summary>
                /// <param name="initialList"></param>
                public Deck(IEnumerable<Generic.Card> initialList) {
                    deckQueue = new Queue<Generic.Card>(initialList);
                    discardQueue = new Queue<Generic.Card>();
                }

                /// <summary>
                /// Draw a card from the top of the deck;
                /// </summary>
                /// <returns></returns>
                public Generic.Card Draw() {
                    return deckQueue.Dequeue();
                }

                /// <summary>
                /// Draw a random card from the deck. Deck is not shuffled.
                /// </summary>
                /// <returns></returns>
                public Generic.Card DrawRandom() {
                    List<Generic.Card> cardsList = new List<Generic.Card>(deckQueue.ToArray());
                    int randomIndex = new System.Random().Next(0, cardsList.Count - 1);
                    Generic.Card chosenCard = cardsList[randomIndex];

                    cardsList.RemoveAt(randomIndex);
                    deckQueue = new Queue<Generic.Card>(cardsList);

                    return chosenCard;
                }

                /// <summary>
                /// Draws a specific card you know of from the deck.
                /// Deck is not shuffled.
                /// </summary>
                /// <param name="card"></param>
                /// <returns></returns>
                public Generic.Card DrawSpecific(Generic.Card card) {
                    List<Generic.Card> cardsList = new List<Generic.Card>(deckQueue.ToArray());
                    cardsList.Remove(card);
                    deckQueue = new Queue<Generic.Card>(cardsList);
                    return card;
                }

                /// <summary>
                /// Put card at bottom of the deck. Deck is not shuffled.
                /// </summary>
                /// <param name="cardToBury"></param>
                public void Bury(Generic.Card cardToBury) {
                    List<Generic.Card> cardsList = new List<Generic.Card>(deckQueue.ToArray());
                    cardsList.Add(cardToBury);
                    deckQueue = new Queue<Generic.Card>(cardsList);
                }

                /// <summary>
                /// Put a card into the discard pile.
                /// </summary>
                /// <param name="cardToDiscard"></param>
                public void Discard(Generic.Card cardToDiscard) {
                    discardQueue.Enqueue(cardToDiscard);
                }

                /// <summary>
                /// Shuffle all cards currently in the deck.
                /// </summary>
                public void ShuffleDeck() {
                    deckQueue = new Queue<Generic.Card>(Shuffle<Generic.Card>.SimpleShuffle(deckQueue.ToArray()));
                }

                /// <summary>
                /// Shuffle a collection of Card back into the deck.
                /// </summary>
                /// <param name="cardsToShuffle"></param>
                public void ShuffleIntoDeck(IEnumerable<Generic.Card> cardsToShuffle) {
                    foreach(Generic.Card card in cardsToShuffle) {
                        deckQueue.Enqueue(card);
                    }
                    ShuffleDeck();
                }

                /// <summary>
                /// Shuffle one Card back into the deck.
                /// </summary>
                /// <param name="cardToShuffle"></param>
                public void ShuffleIntoDeck(Generic.Card cardToShuffle) {
                    deckQueue.Enqueue(cardToShuffle);
                    ShuffleDeck();
                }

                /// <summary>
                /// Shuffles discard pile into deck.
                /// </summary>
                public void ShuffleDiscardIntoDeck() {
                    foreach(Generic.Card card in discardQueue) {
                        deckQueue.Enqueue(card);
                    }
                    discardQueue.Clear();
                    ShuffleDeck();
                }

                /// <summary>
                /// Searches deck for cards that match specified criteria.
                /// </summary>
                /// <param name="criteria"></param>
                /// <returns></returns>
                public Generic.Card[] SearchDeckForCards<T>(T criteria) {
                    return SearchForCardsInQueue(deckQueue, criteria);
                }

                /// <summary>
                /// Searches discard for cards that match specified criteria.
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="criteria"></param>
                /// <returns></returns>
                public Generic.Card[] SearchDiscardForCards<T>(T criteria) {
                    return SearchForCardsInQueue(deckQueue, criteria);
                }

                /// <summary>
                /// Searches queue for cards that match specified criteria.
                /// </summary>
                /// <param name="queue"></param>
                /// <param name="criteria"></param>
                /// <returns></returns>
                private Generic.Card[] SearchForCardsInQueue<T>(Queue<Generic.Card> queue, T criteria) {
                    return (from card in queue
                            where card.MatchesCriteria(criteria)
                            select card).ToArray();
                }

                /// <summary>
                /// Checks if deck is empty.
                /// </summary>
                /// <returns></returns>
                public bool IsDeckEmpty() {
                    return deckQueue.Count == 0;
                }

                /// <summary>
                /// Checks if discard is empty.
                /// </summary>
                /// <returns></returns>
                public bool IsDiscardEmpty() {
                    return discardQueue.Count == 0;
                }
            }
        }
    }
}