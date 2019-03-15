using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryGameDotNet {
    public static class ExtensionMethods {

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
