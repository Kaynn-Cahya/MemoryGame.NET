using System;
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

        /// <summary>
        /// Get all the players that are currently playing this game session.
        /// </summary>
        public Player[] Players {
            get {
                try {
                    return players.ToArray();
                } catch (NullReferenceException) {
                    throw new InvalidOperationException("The current game session is not running. Start the game with some players first.");
                }
            }
        }

        public GameStatus GameStatus { get; private set; }

        public CardBoard GameBoard { get; private set; }

        private List<Player> players;

        #region Delegate
        private OnGameOver onGameOverDelegate;

        /// <summary>
        /// Callback when the game is over.
        /// </summary>
        /// <param name="del"></param>
        public void AddOnGameOverCallback(OnGameOver del) {
            onGameOverDelegate += del;
        }

        public void RemoveOnGameOverCallback(OnGameOver del) {
            onGameOverDelegate -= del;
        }

        #endregion

        public GameSession() {
            GameBoard = new CardBoard();
            players = new List<Player>();
            GameStatus = GameStatus.NOT_STARTED;
        }

        #region StartGame_Functs

        /// <summary>
        /// Start the current game session. (Disposes the current game if its ongoing)
        /// </summary>
        /// <param name="config">The configuration for the layout of the board.</param>
        /// <param name="noOfPlayers">The number of players playing.</param>
        public void StartGame(BoardConfiguration config, int noOfPlayers = 1) {
            StartGame(config.Columns, config.Rows, config.TypeOfCardsCount, noOfPlayers);
        }

        /// <summary>
        /// Start the current game session. (Disposes the current game if its ongoing)
        /// </summary>
        /// <param name="config">The configuration for the layout of the board.</param>
        /// <param name="playersPlaying">The list of players playing.</param>
        public void StartGame(BoardConfiguration config, IEnumerable<Player> playersPlaying) {
            StartGame(config.Columns, config.Rows, config.TypeOfCardsCount, playersPlaying);
        }

        /// <summary>
        /// Start the current game session. (Disposes the current game if its ongoing)
        /// </summary>
        /// <param name="xSize">The number of columns this game board has.</param>
        /// <param name="ySize">The number of rows this game board has.</param>
        /// <param name="typeOfCardsCount">The number of different kinds of cards this game board has.</param>
        /// <param name="noOfPlayers">The number of players playing.</param>
        public void StartGame(int xSize, int ySize, int typeOfCardsCount, int noOfPlayers = 1) {
            if (noOfPlayers < 0) {
                throw new ArgumentException("There must be at least 1 player playing.", "noOfPlayers");
            }

            GameBoard.CreateBoard(xSize, ySize, typeOfCardsCount);

            InitalizePlayers(noOfPlayers);
            GameStatus = GameStatus.ON_GOING;
        }

        /// <summary>
        /// Start the current game session. (Disposes the current game if its ongoing)
        /// </summary>
        /// <param name="xSize">The number of columns this game board has.</param>
        /// <param name="ySize">The number of rows this game board has.</param>
        /// <param name="typeOfCardsCount">The number of different kinds of cards this game board has.</param>
        /// <param name="playersPlaying">The list of players playing.</param>
        public void StartGame(int xSize, int ySize, int typeOfCardsCount, IEnumerable<Player> playersPlaying) {
            if (playersPlaying.GetCount() <= 0 || playersPlaying == null) {
                throw new ArgumentException("There must be at least 1 player playing.", "players");
            }

            GameBoard.CreateBoard(xSize, ySize, typeOfCardsCount);

            players = playersPlaying.ToList();
            GameStatus = GameStatus.ON_GOING;
        }

        #endregion

        private void InitalizePlayers(int noOfPlayers) {
            players = new List<Player>();

            for (int i = 0; i < noOfPlayers; ++i) {
                Player player = new Player();
                players.Add(player);
            }

            CurrentPlayerNo = 0;
        }

        /// <summary>
        /// Attempt to match a pair.
        /// </summary>
        /// <param name="first">Position of the first card on the board. (Starts from 0)</param>
        /// <param name="second">Position of the second card on the board. (Starts from 0)</param>
        /// <returns>A result defining if the pair matched.</returns>
        public MatchResult MatchPair(Point first, Point second) {
            return MatchPair(first.X, first.Y, second.X, second.Y);
        }

        /// <summary>
        /// Attempt to match a pair.
        /// </summary>
        /// <param name="x1">X position of the first card on the board. (Starts from 0)</param>
        /// <param name="y1">Y position of the first card on the board. (Starts from 0)</param>
        /// <param name="x2">X position of the second card on the board. (Starts from 0)</param>
        /// <param name="y2">Y position of the second card on the board. (Starts from 0)</param>
        /// <returns>A result defining if the pair matched.</returns>
        public MatchResult MatchPair(int x1, int y1, int x2, int y2) {
            if (GameStatus == GameStatus.NOT_STARTED) {
                throw new InvalidOperationException("Current game session is not running, start the game first!");
            } else if (GameStatus == GameStatus.ENDED) {
                throw new InvalidOperationException("Current game session has ended. Start a new game session!");
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

            // Matching the same card
            if (x1 == x2 && y1 == y2) {
                throw new InvalidOperationException("You cannot match a card to itself!");
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
                GameBoard.ModifyCardIsMatched(x1, y1, true);
                GameBoard.ModifyCardIsMatched(x2, y2, true);
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

            GameStatus = GameStatus.ENDED;

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
