using System;

namespace MemoryGameDotNet {
    [Serializable]
    public class BoardConfiguration {
        /// <summary>
        /// How many rows of cards there will be.
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// How many columns of cards there will be.
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// How many different types of cards will there be for matching.
        /// </summary>
        public int TypeOfCardsCount { get; set; }

        /// <summary>
        /// Use this instance to pass it as a parameter to start the game session. (Defines the game board.)
        /// </summary>
        /// <param name="rows">How many rows of cards there will be.</param>
        /// <param name="columns">How many columns of cards there will be.</param>
        /// <param name="typeOfCardsCount">How many different types of cards will there be for matching.</param>
        public BoardConfiguration(int rows = 8, int columns = 6, int typeOfCardsCount = 6) {
            Rows = rows;
            Columns = columns;
            TypeOfCardsCount = typeOfCardsCount;
        }
    }
}
