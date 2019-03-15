using System;
using System.Collections.Generic;

namespace MemoryGameDotNet {
    public class CardBoard {

        private Card[,] board;

        public int RowCount {
            get {
                return board.GetLength(1);
            }
        }

        public int ColumnCount {
            get {
                return board.GetLength(0);
            }
        }

        public Card this[int x,int y] {
            get {
                return board[x, y];
            }
        }

        internal void LoadBoard(Card[,] inputBoard) {
            if (inputBoard.Length % 2 != 0) {
                throw new InvalidBoardSizeException("Game Board is of invalid size. The total number of elements in the game board should be even.");
            }

            if (!BoardHasPairsOfCards(inputBoard)) {
                throw new ArgumentException("Not all cards in the input board have a matching pair.", "inputBoard");
            }

            board = inputBoard;
        }

        private bool BoardHasPairsOfCards(Card[,] inputBoard) {
            Dictionary<int, int> cardTypeCountPair = new Dictionary<int, int>();

            foreach (var card in inputBoard) {
                if (!cardTypeCountPair.ContainsValue(card.CardNumber)) {
                    cardTypeCountPair.Add(card.CardNumber, 1);
                } else {
                    ++cardTypeCountPair[card.CardNumber];
                }
            }

            foreach (var cardCount in cardTypeCountPair.Values) {
                if (cardCount % 2 != 0) {
                    return false;
                }
            }

            return true;
        }

        internal void CreateBoard(int xSize, int ySize, int typeOfCardsCount) {
            if ((xSize * ySize) % 2 != 0) {
                throw new InvalidBoardSizeException("Game Board is of invalid size. The total number of elements in the game board should be even.");
            }

            if (typeOfCardsCount <= 0) {
                throw new ArgumentOutOfRangeException("typeOfCardsCount", "There must be at least more than 1 type of card for the board!");
            } 

            board = new Card[xSize, ySize];
            FillBoardWithCards(typeOfCardsCount);
            
        }

        private void FillBoardWithCards(int typeOfCardsCount) {

            List<Point> validCardPlacementLocations = GetBoardPositionAsList();

            Random rand = new Random();

            // While there is a valid place to put cards.
            while (validCardPlacementLocations.Count > 0) {
                int cardTypeToPlace = rand.Next(0, typeOfCardsCount);

                // Place card in pairs
                for (int i = 0; i < 2; ++i) {
                    Point cardPlacementPoint = validCardPlacementLocations[rand.Next(0, validCardPlacementLocations.Count)];
                    board[cardPlacementPoint.X, cardPlacementPoint.Y] = new Card(cardTypeToPlace);

                    validCardPlacementLocations.Remove(cardPlacementPoint);
                }
            }
        }

        private List<Point> GetBoardPositionAsList() {
            List<Point> boardLocations = new List<Point>();
            for (int x = 0; x < board.GetLength(0); ++x) {
                for (int y = 0; y < board.GetLength(1); ++y) {
                    boardLocations.Add(new Point(x, y));
                }
            }

            return boardLocations;
        }

        #region Util

        internal void ModifyCardIsMatched(int x, int y, bool isMatched) {
            int cardNo = board[x, y].CardNumber;
            board[x, y] = new Card(cardNo, isMatched);
        }

        public Card[,] GetBoard() {
            return (Card[,]) board.Clone();
        }

        #endregion
    }
}
