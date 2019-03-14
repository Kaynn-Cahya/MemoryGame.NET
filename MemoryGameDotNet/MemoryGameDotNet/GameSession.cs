using System;
using System.Collections;
using System.Collections.Generic;

namespace MemoryGameDotNet {
    public class GameSession {

        public delegate void OnGameOver();

        /// <summary>
        /// Which player, by number ordering, is having their turn now.
        /// </summary>
        public int CurrentPlayerNo { get; private set; }

        /// <summary>
        /// Get the current player that is having their turn now.
        /// </summary>
        public Player CurrentPlayer {
            get {
                try {
                    return players[CurrentPlayerNo];
                } catch (IndexOutOfRangeException) {
                    throw new InvalidOperationException("The current game session is not running. Start the game first.");
                }
            }
        }

        public bool GameRunning { get; private set; }

        public CardBoard GameBoard { get; private set; }

        private List<Player> players;

        #region Delegate
        private OnGameOver onGameOverDelegate;

        public void AddOnGameOverCallback(OnGameOver m) {
            onGameOverDelegate += m;
        }

        public void RemoveOnGameOverCallback(OnGameOver m) {
            onGameOverDelegate -= m;
        }

        #endregion

        public GameSession() {
            GameBoard = new CardBoard();
            players = new List<Player>();
            GameRunning = false;
        }

        public void StartGame(int xSize, int ySize, int typeOfCardsCount, int noOfPlayers = 1) {
            if (noOfPlayers < 0) {
                throw new ArgumentException("There must be at least 1 player playing.", "noOfPlayers");
            }

            GameBoard.CreateBoard(xSize, ySize, typeOfCardsCount);

            InitalizePlayers(noOfPlayers);
            GameRunning = true;
        }

        private void InitalizePlayers(int noOfPlayers) {
            players = new List<Player>();

            for (int i = 0; i < noOfPlayers; ++i) {
                Player player = new Player();
                players.Add(player);
            }

            CurrentPlayerNo = 0;
        }

        public void StartGame(int xSize, int ySize, int typeOfCardsCount, List<Player> playersPlaying) {
            if (playersPlaying.Count <= 0 || playersPlaying == null) {
                throw new ArgumentException("There must be at least 1 player playing.", "players");
            }

            GameBoard.CreateBoard(xSize, ySize, typeOfCardsCount);

            players = playersPlaying;
            GameRunning = true;
        }

        public MatchResult MatchPair(Point first, Point second) {
            return MatchPair(first.X, first.Y, second.X, second.Y);
        }

        public MatchResult MatchPair(int x1, int y1, int x2, int y2) {
            if (!GameRunning) {
                throw new InvalidOperationException("Current game session is not running, start the game first!");
            }
            // Invalid position given.
            if (x1 < 0 || x1 >= GameBoard.ColumnCount) {
                throw new ArgumentException("Point given must be within the board range. (Start from 0)", "x1");
            } else if (x2 < 0 || x2 >= GameBoard.ColumnCount) {
                throw new ArgumentException("Point given must be within the board range. (Start from 0)", "x2");
            } else if (y1 < 0 || y1 >= GameBoard.RowCount) {
                throw new ArgumentException("Point given must be within the board range. (Start from 0)", "y1");
            } else if (y2 < 0 || y2 >= GameBoard.RowCount) {
                throw new ArgumentException("Point given must be within the board range. (Start from 0)", "y2");
            }

            Card firstCard = GameBoard[x1, y1];
            Card secondCard = GameBoard[x2, y2];

            // Card was already matched.
            if (firstCard.IsMatched) {
                throw new InvalidOperationException("The first card was previously matched!");
            } else if (secondCard.IsMatched) {
                throw new InvalidOperationException("The second card was previously matched!");
            }

            if (firstCard.Matches(secondCard)) {
                firstCard.IsMatched = true;
                secondCard.IsMatched = true;
                ++CurrentPlayer.Score;

                CheckForGameOver();
                return MatchResult.MATCH;
            } else {
                MoveToNextPlayer();
                return MatchResult.NO_MATCH;
            }
        }

        private void CheckForGameOver() {
            var board = GameBoard.GetBoard();

            foreach (var card in board) {
                // A card was not matched yet, game is not over.
                if (!card.IsMatched) {
                    return;
                }
            }

            GameRunning = false;

            if (onGameOverDelegate != null) {
                onGameOverDelegate();
            }
        }

        private void MoveToNextPlayer() {
            ++CurrentPlayerNo;

            if (CurrentPlayerNo >= players.Count) {
                CurrentPlayerNo = 0;
            }
        }
    }
}
