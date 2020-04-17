namespace UnityUtilities {
    namespace Random {

        /// <summary>
        /// Contains methods for shuffling arrays.
        /// </summary>
        /// <typeparam name="T">Type of array.</typeparam>
        public static class Shuffle<T> {

            /// <summary>
            /// Shuffle array only once. Implements Richard Durstenfeld's version
            /// of Fisher-Yates algorithm.
            /// </summary>
            /// <param name="arrayToShuffle">Array to be shuffled.</param>
            /// <returns></returns>
            public static T[] SimpleShuffle(T[] arrayToShuffle) {
                return SimpleShuffle(arrayToShuffle, 1);
            }

            /// <summary>
            /// Shuffles array specified number of times. Implements Richard
            /// Durstenfeld's version of Fisher-Yates algorithm.
            /// </summary>
            /// <param name="arrayToShuffle">Array to be shuffled.</param>
            /// <param name="iterations">Number of times to shuffle.</param>
            /// <returns></returns>
            public static T[] SimpleShuffle(T[] arrayToShuffle, int iterations) {

                System.Random r = new System.Random();
                int listLength;
                T[] finalShuffledArray;

                for (int iteration = 0; iteration < iterations; iteration++) {

                    listLength = arrayToShuffle.Length;
                    finalShuffledArray = new T[listLength];

                    for (int i = 0; i < arrayToShuffle.Length; i++) {
                        int indexToMove = r.Next(0, listLength - 1);
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
                T temp = elm1;
                elm1 = elm2;
                elm2 = temp;
            }
        }
    }
}