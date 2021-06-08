using System.Collections.Generic;
using System.Linq;

namespace UnityUtilities.Random {
	/// <summary>
	/// Contains methods for shuffling arrays.
	/// </summary>
	/// <typeparam name="T">Type of array.</typeparam>
	public static class Shuffle<T> {
		/// <summary>
		/// Shuffles array specified number of times. Implements Richard
		/// Durstenfeld's version of Fisher-Yates shuffle algorithm.
		/// </summary>
		/// <param name="array">Array to be shuffled.</param>
		/// <param name="iterations">Number of times to shuffle.</param>
		/// <returns></returns>
		public static IEnumerable<T> SimpleShuffle(IEnumerable<T> array, int iterations = 1) {
			var r = new System.Random();
			var arrayToShuffle = array.ToArray();

			for (var iteration = 0; iteration < iterations; iteration++) {
				var listLength = arrayToShuffle.Length;
				var finalShuffledArray = new T[listLength];

				for (var i = 0; i < arrayToShuffle.Length; i++) {
					var indexToMove = r.Next(0, listLength - 1);
					Swap(ref arrayToShuffle[indexToMove], ref arrayToShuffle[listLength - 1]);
					finalShuffledArray[i] = arrayToShuffle[listLength - 1];
					listLength--;
				}

				arrayToShuffle = finalShuffledArray;
			}

			return arrayToShuffle;
		}

		/// <summary>
		/// Swaps two elements in an array.
		/// </summary>
		/// <param name="elm1">Ref of first element.</param>
		/// <param name="elm2">Ref of second element</param>
		private static void Swap(ref T elm1, ref T elm2) {
			var temp = elm1;
			elm1 = elm2;
			elm2 = temp;
		}
	}
}