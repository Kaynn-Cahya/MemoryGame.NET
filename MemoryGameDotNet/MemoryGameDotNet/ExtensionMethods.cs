using System.Collections.Generic;

namespace MemoryGameDotNet {
    public static class ExtensionMethods {

        /// <summary>
        /// Comnpares between the card's number.
        /// </summary>
        /// <param name="card"></param>
        /// <param name="target"></param>
        /// <returns>True if this card matches the target card.</returns>
        public static bool Matches(this Card card, Card target) {
            return card.CardNumber == target.CardNumber;
        }

        internal static int GetCount<T>(this IEnumerable<T> enumerable) {
            int i = 0;
            foreach (var item in enumerable) {
                ++i;
            }
            return i;
        }

        internal static List<T> ToList<T>(this IEnumerable<T> enumerable) {
            List<T> result = new List<T>();

            foreach (var item in enumerable) {
                result.Add(item);
            }

            return result;
        }
    }
}
