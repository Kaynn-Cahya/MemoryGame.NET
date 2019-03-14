using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryGameDotNet {
    [Serializable]
    public class Player {

        /// <summary>
        /// Set this to identify the player. (Does not affect the library)
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The number of pairs the player has matched.
        /// </summary>
        public int Score { get; internal set; }

        public Player(string identifier = "") {
            Identifier = identifier;
            Score = 0;
        }
    }
}
