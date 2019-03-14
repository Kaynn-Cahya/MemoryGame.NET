using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryGameDotNet {
    [Serializable]
    public struct Card {
        /// <summary>
        /// Type Identifier for this card. (Used to match other cards of similar number)
        /// </summary>
        public int CardNumber { get; private set; }

        /// <summary>
        /// True if the card was previously matched by a player.
        /// </summary>
        public bool IsMatched { get; internal set; }

        public Card(int cardNumber, bool isMatched = false) {
            CardNumber = cardNumber;
            IsMatched = isMatched;
        }
    }
}
