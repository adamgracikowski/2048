using _2048.core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _2048
{
    public partial class MainWindow : Window
    {
        private Board board;
        private int score;
        private int highestScore;
        private int moves;
        private readonly Random random = new Random();
        private enum Direction { Left, Right, Up, Down };

        // for undo feature
        private Stack<Board> lastBoards = new Stack<Board>();
        private Stack<int> lastScores = new Stack<int>();

        // board styling
        private readonly Dictionary<int, string> backgroundTileColors = new Dictionary<int, string>()
        {
            {1,    "#9e948a"},
            {2,    "#eee4da"},
            {4,    "#ede0c8"},
            {8,    "#f2b179"},
            {16,   "#f59563"},
            {32,   "#f67c5f"},
            {64,   "#f65e3b"},
            {128,  "#edcf72"},
            {256,  "#edcc61"},
            {512,  "#edc850"},
            {1024, "#edc53f"},
            {2048, "#edc22e"},
        };
        private readonly Dictionary<int, string> foregroundTileColors = new Dictionary<int, string>()
        {
            {1,    "#9e948a"},
            {2,    "#776e65"},
            {4,    "#776e65"},
            {8,    "#f9f6f2"},
            {16,   "#f9f6f2"},
            {32,   "#f9f6f2"},
            {64,   "#f9f6f2"},
            {128,  "#f9f6f2"},
            {256,  "#f9f6f2"},
            {512,  "#f9f6f2"},
            {1024, "#f9f6f2"},
            {2048, "#f9f6f2"},
        };
        private Border[,] mainBoardItems = new Border[Board.SIZE, Board.SIZE];

        public MainWindow()
        {
            InitializeComponent();

            board = new Board();
            score = 0;
            highestScore = 0;
            moves = 0;

            ConnectWithLayout();
            UpdateStatistics();
            UpdateBoard();
        }

        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }
        private void ButtonUndo_Click(object sender, RoutedEventArgs e)
        {
            if (moves > 0)
            {
                moves--;
                score = lastScores.Pop();
                board = lastBoards.Pop();
                UpdateStatistics();
                UpdateBoard();
            }
        }
        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            ApplyMove(Direction.Left);
        }
        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            ApplyMove(Direction.Right);
        }
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            ApplyMove(Direction.Up);
        }
        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            ApplyMove(Direction.Down);
        }
        private async void ApplyMove(Direction direction)
        {
            lastBoards.Push((Board)board.Clone());
            lastScores.Push(score);
            Tuple<bool, int> result;
            switch (direction)
            {
                case Direction.Left:
                    result = board.MoveLeft();
                    break;
                case Direction.Right:
                    result = board.MoveRight();
                    break;
                case Direction.Up:
                    result = board.MoveUp();
                    break;
                default:
                    result = board.MoveDown();
                    break;
            }
            if (result.Item1 == true)
            {
                moves++;
                score += result.Item2;
                UpdateStatistics();
                UpdateBoard();
                await PrepareForNextMove();
            }
            else
            {
                lastBoards.Pop();
                lastScores.Pop();
            }
        }
        private async Task PrepareForNextMove()
        {
            if (board.Has2048())
            {
                GameWon();
            }

            await SpawnNewTile();
            UpdateBoard();

            if (!board.HasValidMoves())
            {
                GameLost();
            }
        }

        private void GameLost()
        {
            GameOverWindow gameOverWindow = new GameOverWindow(Width, Height, Left, Top);
            gameOverWindow.TryAgainButton.Click += (s, e) => { ResetGame(); gameOverWindow.Close(); };
            gameOverWindow.Closed += (sender, e) => { ResetGame(); };
            gameOverWindow.Show();
        }
        private void GameWon()
        {
            GameWonWindow gameWonWindow = new GameWonWindow(Width, Height, Left, Top);
            gameWonWindow.TryAgainButton.Click += (s, e) => { ResetGame(); gameWonWindow.Close(); };
            gameWonWindow.Closed += (sender, e) => { ResetGame(); };
            gameWonWindow.Show();
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < Board.SIZE; ++i)
            {
                for (int j = 0; j < Board.SIZE; ++j)
                {
                    Tile tile = board.Tiles[i, j];
                    Border border = mainBoardItems[i, j];
                    Label label = border.Child as Label;

                    label.Content = (tile.IsEmpty() ? string.Empty : tile.Value.ToString());
                    border.Background = new BrushConverter()
                        .ConvertFromString(backgroundTileColors[tile.Value]) as SolidColorBrush;
                    label.Foreground = new BrushConverter()
                       .ConvertFromString(foregroundTileColors[tile.Value]) as SolidColorBrush;
                }
            }
        }
        private void UpdateScore()
        {
            LabelScore.Content = score;
        }
        private void UpdateBest()
        {
            LabelBest.Content = (highestScore > score ? highestScore : score);
        }
        private void UpdateMoves()
        {
            LabelMoves.Content = moves;
        }
        private void UpdateStatistics()
        {
            UpdateScore();
            UpdateBest();
            UpdateMoves();
        }

        private void ConnectWithLayout()
        {
            int i = 0, j = 0;
            foreach (UIElement item in MainBoard.Children)
            {
                if (item is Border border)
                {
                    mainBoardItems[i, j] = border;
                }
                j++;
                if (j > (Board.SIZE - 1))
                {
                    j = 0;
                    i++;
                }
            }
        }
        private void ClearLastGameHistory()
        {
            lastBoards.Clear();
            lastScores.Clear();
        }
        private void ResetGame()
        {
            board = new Board();
            highestScore = (highestScore > score ? highestScore : score);
            score = 0;
            moves = 0;
            UpdateStatistics();
            UpdateBoard();
            ClearLastGameHistory();
        }
        private async Task SpawnNewTile()
        {
            await Task.Delay(100);
            var emptyTiles = board.GetEmptyTiles();
            emptyTiles[random.Next(emptyTiles.Count)].IncreaseValue();
        }
    }
}
