using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace _2048.core
{
    public class Board : ICloneable, IEquatable<Board>
    {
        public static readonly int SIZE = 4;
        public Tile[,] Tiles = new Tile[SIZE, SIZE];
        private readonly Random random = new Random();

        // public interface
        public Board()
        {
            for (int i = 0; i < SIZE; ++i)
                for (int j = 0; j < SIZE; ++j)
                    Tiles[i, j] = new Tile(i, j);

            FillInitial();
        }
        public object Clone()
        {
            Board clone = new Board();
            for (int i = 0; i < SIZE; ++i)
                for (int j = 0; j < SIZE; ++j)
                    clone.Tiles[i, j].Value = Tiles[i, j].Value;
            return clone;
        }
        public bool Equals(Board other)
        {
            for (int i = 0; i < SIZE; ++i)
                for (int j = 0; j < SIZE; ++j)
                    if (other.Tiles[i, j].Value != Tiles[i, j].Value)
                        return false;
            return true;
        }
        public void CopyTileValues(Board other)
        {
            for (int i = 0; i < SIZE; ++i)
                for (int j = 0; j < SIZE; ++j)
                    Tiles[i, j].Value = other.Tiles[i, j].Value;
        }
        public void DisplayOnConsole()
        {
            for (int i = 0; i < SIZE; ++i)
            {
                for (int j = 0; j < SIZE; ++j)
                {
                    if (Tiles[i, j].IsEmpty())
                        Console.Write("[{0,4}]", " ");
                    else
                        Console.Write("[{0,4}]", Tiles[i, j]);
                }
                Console.Write("\n");
            }
        }
        public List<Tile> GetEmptyTiles()
        {
            List<Tile> emptyTiles = new List<Tile>();
            foreach (Tile tile in Tiles)
            {
                if (tile.IsEmpty())
                    emptyTiles.Add(tile);
            }
            return emptyTiles;
        }
        public bool Has2048()
        {
            foreach (Tile tile in Tiles)
            {
                if (tile.Value == Tile.MaxValue)
                    return true;
            }
            return false;
        }
        public bool HasValidMoves()
        {
            for (int i = 0; i < SIZE; ++i)
            {
                for (int j = 0; j < SIZE; ++j)
                {
                    if (Tiles[i, j].IsEmpty())
                        return true;
                    if (IsWithinBounds(i - 1, j) && Tiles[i - 1, j].Value == Tiles[i, j].Value)
                        return true;
                    if (IsWithinBounds(i + 1, j) && Tiles[i + 1, j].Value == Tiles[i, j].Value)
                        return true;
                    if (IsWithinBounds(i, j - 1) && Tiles[i, j - 1].Value == Tiles[i, j].Value)
                        return true;
                    if (IsWithinBounds(i, j + 1) && Tiles[i, j + 1].Value == Tiles[i, j].Value)
                        return true;
                }
            }
            return false;
        }

        private static bool IsWithinBounds(int i, int j)
        {
            return (i >= 0 && i < SIZE && j >= 0 && j < SIZE);
        }

        // logic to move every tile on board in a given direction
        public Tuple<bool, int> MoveLeft()
        {
            Board clone = (Board)this.Clone();

            int points = 0;
            for (int i = 0; i < SIZE; ++i)
            {
                for (int j = 0; j < SIZE; ++j)
                {
                    points += clone.MoveTileLeft(clone.Tiles[i, j]);
                }
            }
            if (!this.Equals(clone))
            {
                this.CopyTileValues(clone);
                return new Tuple<bool, int>(true, points);
            }

            return new Tuple<bool, int>(false, points);
        }
        public Tuple<bool, int> MoveRight()
        {
            Board clone = (Board)this.Clone();

            int points = 0;
            for (int i = 0; i < SIZE; ++i)
            {
                for (int j = SIZE - 1; j >= 0; --j)
                {
                    points += clone.MoveTileRight(clone.Tiles[i, j]);
                }
            }
            if (!this.Equals(clone))
            {
                this.CopyTileValues(clone);
                return new Tuple<bool, int>(true, points);
            }

            return new Tuple<bool, int>(false, points);
        }
        public Tuple<bool, int> MoveUp()
        {
            Board clone = (Board)this.Clone();

            int points = 0;
            for (int i = 0; i < SIZE; ++i)
            {
                for (int j = 0; j < SIZE; ++j)
                {
                    points += clone.MoveTileUp(clone.Tiles[j, i]);
                }
            }
            if (!this.Equals(clone))
            {
                this.CopyTileValues(clone);
                return new Tuple<bool, int>(true, points);
            }

            return new Tuple<bool, int>(false, points);
        }
        public Tuple<bool, int> MoveDown()
        {
            Board clone = (Board)this.Clone();

            int points = 0;
            for (int i = 0; i < SIZE; ++i)
            {
                for (int j = SIZE - 1; j >= 0; --j)
                {
                    points += clone.MoveTileDown(clone.Tiles[j, i]);
                }
            }
            if (!this.Equals(clone))
            {
                this.CopyTileValues(clone);
                return new Tuple<bool, int>(true, points);
            }

            return new Tuple<bool, int>(false, points);
        }

        // logic to move a single tile on board in a given direction
        private int MoveTileLeft(Tile tile)
        {
            if (!tile.IsEmpty())
            {
                for (int newCol = tile.Col - 1; newCol >= 0; --newCol)
                {
                    Tile current = Tiles[tile.Row, newCol];
                    if (!current.IsEmpty())
                    {
                        if (current.Value == tile.Value && !current.WasUpdated)
                            return HandleCollision(tile, current);
                        Tiles[tile.Row, newCol + 1].Value = tile.Value;
                        if (newCol + 1 != tile.Col) tile.MakeEmpty();
                        return 0;
                    }
                }
                if (tile.Col != 0)
                {
                    Tiles[tile.Row, 0].Value = tile.Value;
                    tile.MakeEmpty();
                }
            }
            return 0;
        }
        private int MoveTileRight(Tile tile)
        {
            if (!tile.IsEmpty())
            {
                for (int newCol = tile.Col + 1; newCol < SIZE; ++newCol)
                {
                    Tile current = Tiles[tile.Row, newCol];
                    if (!current.IsEmpty())
                    {
                        if (current.Value == tile.Value && !current.WasUpdated)
                            return HandleCollision(tile, current);
                        Tiles[tile.Row, newCol - 1].Value = tile.Value;
                        if (newCol - 1 != tile.Col) tile.MakeEmpty();
                        return 0;
                    }
                }
                if (tile.Col != SIZE - 1)
                {
                    Tiles[tile.Row, SIZE - 1].Value = tile.Value;
                    tile.MakeEmpty();
                }
            }
            return 0;
        }
        private int MoveTileUp(Tile tile)
        {
            if (!tile.IsEmpty())
            {
                for (int newRow = tile.Row - 1; newRow >= 0; --newRow)
                {
                    Tile current = Tiles[newRow, tile.Col];
                    if (!current.IsEmpty())
                    {
                        if (current.Value == tile.Value && !current.WasUpdated)
                            return HandleCollision(tile, current);
                        Tiles[newRow + 1, tile.Col].Value = tile.Value;
                        if (newRow + 1 != tile.Row) tile.MakeEmpty();
                        return 0;
                    }
                }
                if (tile.Row != 0)
                {
                    Tiles[0, tile.Col].Value = tile.Value;
                    tile.MakeEmpty();
                }
            }
            return 0;
        }
        private int MoveTileDown(Tile tile)
        {
            if (!tile.IsEmpty())
            {
                for (int newRow = tile.Row + 1; newRow < SIZE; ++newRow)
                {
                    Tile current = Tiles[newRow, tile.Col];
                    if (!current.IsEmpty())
                    {
                        if (current.Value == tile.Value && !current.WasUpdated)
                            return HandleCollision(tile, current);
                        Tiles[newRow - 1, tile.Col].Value = tile.Value;
                        if (newRow - 1 != tile.Row) tile.MakeEmpty();
                        return 0;
                    }
                }
                if (tile.Row != SIZE - 1)
                {
                    Tiles[SIZE - 1, tile.Col].Value = tile.Value;
                    tile.MakeEmpty();
                }
            }
            return 0;
        }

        // other helper methods
        private void FillInitial()
        {
            foreach (Tile tile in GetEmptyTiles().OrderBy(x => random.Next()).Take(2))
            {
                tile.IncreaseValue();
            }
        }
        private static int HandleCollision(Tile tile1, Tile tile2)
        {
            tile2.IncreaseValue();
            tile2.WasUpdated = true;
            tile1.MakeEmpty();
            return tile2.Value;
        }
    }
}
