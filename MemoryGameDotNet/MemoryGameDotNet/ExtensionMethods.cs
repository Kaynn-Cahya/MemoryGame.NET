using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryGameDotNet {
    public static class ExtensionMethods {

        public static bool Matches(this Card card, Card target) {
            return card.CardNumber == target.CardNumber;
        }
    }
}
