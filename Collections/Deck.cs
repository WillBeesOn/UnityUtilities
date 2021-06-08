using System;
using System.Linq;
using System.Collections.Generic;
using UnityUtilities.Random;

namespace UnityUtilities.Collections {
	/// <summary>
	/// Creates a "deck" and "discard" representation of Cards using
	/// queues for both. Type for deck must implement Card.
	/// </summary>
	public class Deck<T> {
		public int deckCount => _deckQueue.Count;
		public int discardCount => _discardQueue.Count;

		private Queue<T> _deckQueue;
		private Queue<T> _discardQueue;

		/// <summary>
		/// Create a deck with a collection of Card.
		/// </summary>
		/// <param name="initialList"></param>
		public Deck(IEnumerable<T> initialList) {
			_deckQueue = new Queue<T>(initialList);
			_discardQueue = new Queue<T>();
		}

		/// <summary>
		/// Draw a card from the top of the deck;
		/// </summary>
		/// <returns></returns>
		public T Draw() {
			return _deckQueue.Count > 0 ? _deckQueue.Dequeue() : default;
		}

		/// <summary>
		/// Draw a random card from the deck. Deck is not shuffled.
		/// </summary>
		/// <returns></returns>
		public T DrawRandom() {
			if (_deckQueue.Count <= 0) {
				return default;
			}

			var cardsList = _deckQueue.ToList();
			var randomIndex = new System.Random().Next(0, cardsList.Count - 1);
			var chosenCard = cardsList[randomIndex];

			cardsList.RemoveAt(randomIndex);
			_deckQueue = new Queue<T>(cardsList);

			return chosenCard;
		}

		/// <summary>
		/// Draws a specific card you know of from the deck. Returns
		/// default if card is not in the deck. Intended to be used
		/// in conjunction with SearchDeckForCards. Deck is not shuffled.
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public T DrawSpecificCardFromDeck(T card) {
			return DrawSpecificCardFromQueue(_deckQueue, card);
		}

		/// <summary>
		/// Draws a specific card you know of from discard. Returns
		/// default if card is not in discard. Intended to be used
		/// in conjunction with SearchDiscardForCards.
		/// Deck is not shuffled.
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public T DrawSpecificCardFromDiscard(T card) {
			return DrawSpecificCardFromQueue(_discardQueue, card);
		}

		/// <summary>
		/// Generic method to use by the above two.
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		private T DrawSpecificCardFromQueue(Queue<T> queue, T card) {
			if (queue.Count > 0) {
				List<T> cardList = queue.ToList();
				if (cardList.Remove(card)) {
					if (queue == _deckQueue) {
						_deckQueue = new Queue<T>(cardList);
					} else if (queue == _discardQueue) {
						_discardQueue = new Queue<T>(cardList);
					} else {
						return default;
					}

					return card;
				}
			}

			return default;
		}

		/// <summary>
		/// Put card at bottom of the deck. Deck is not shuffled.
		/// </summary>
		/// <param name="cardToBury"></param>
		public void Bury(T cardToBury) {
			var cardsList = _deckQueue.ToList();
			cardsList.Add(cardToBury);
			_deckQueue = new Queue<T>(cardsList);
		}

		/// <summary>
		/// Put a drawn card or a card directly from the deck
		/// into the discard pile. If the card is discarded directly
		/// from the deck, the deck is not shuffled. A card already
		/// in discard will not be added to discard.
		/// </summary>
		/// <param name="cardToDiscard"></param>
		public void Discard(T cardToDiscard) {
			List<T> cardsList = _deckQueue.ToList();
			if (cardsList.Remove(cardToDiscard)) {
				_deckQueue = new Queue<T>(cardsList);
			}

			if (!_discardQueue.Contains(cardToDiscard)) {
				_discardQueue.Enqueue(cardToDiscard);
			}
		}

		/// <summary>
		/// Shuffle all cards currently in the deck.
		/// </summary>
		public void ShuffleDeck() {
			_deckQueue = new Queue<T>(Shuffle<T>.SimpleShuffle(_deckQueue.ToArray()));
		}

		/// <summary>
		/// Shuffle a collection of Card back into the deck.
		/// </summary>
		/// <param name="cardsToShuffle"></param>
		public void ShuffleIntoDeck(IEnumerable<T> cardsToShuffle) {
			_deckQueue = new Queue<T>(_deckQueue.ToList().Concat(cardsToShuffle));
			ShuffleDeck();
		}

		/// <summary>
		/// Shuffle one Card back into the deck.
		/// </summary>
		/// <param name="cardToShuffle"></param>
		public void ShuffleIntoDeck(T cardToShuffle) {
			_deckQueue.Enqueue(cardToShuffle);
			ShuffleDeck();
		}

		/// <summary>
		/// Shuffles discard pile into deck.
		/// </summary>
		public void ShuffleDiscardIntoDeck() {
			_deckQueue = new Queue<T>(_deckQueue.ToList().Concat(_discardQueue.ToList()).ToList());
			_discardQueue.Clear();
			ShuffleDeck();
		}

		/// <summary>
		/// Searches deck for cards that match specified criteria.
		/// </summary>
		/// <param name="criteria"></param>
		/// <returns></returns>
		public T[] SearchDeckForCards(Predicate<T> matches) {
			return SearchForCardsInQueue(_deckQueue, matches);
		}

		/// <summary>
		/// Searches discard for cards that match specified criteria.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="criteria"></param>
		/// <returns></returns>
		public T[] SearchDiscardForCards(Predicate<T> matches) {
			return SearchForCardsInQueue(_discardQueue, matches);
		}

		/// <summary>
		/// Searches queue for cards that match specified criteria.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="criteria"></param>
		/// <returns></returns>
		private T[] SearchForCardsInQueue(Queue<T> queue, Predicate<T> matches) {
			return (from card in queue where matches(card) select card).ToArray();
		}
	}
}