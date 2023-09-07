using System;

namespace _2048.core
{
    public class Tile
    {
        private static readonly int multiplier = 2;
        public static readonly int EmptyValue = 1;
        public static readonly int MaxValue = 2048;

        public int Value { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool WasUpdated { get; set; }

        public Tile(int row, int col, int value = 1, bool wasUpdated = false)
        {
            Row = row;
            Col = col;
            Value = value;
            WasUpdated = wasUpdated;
        }
        public bool IsEmpty()
        {
            return Value == EmptyValue;
        }
        public void MakeEmpty()
        {
            Value = EmptyValue;
        }
        public void IncreaseValue()
        {
            Value *= multiplier;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
