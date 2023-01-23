using System;

namespace ConsoleLayers.Core
{
    public struct Symbol
    {
        public ConsoleColor ForeColor { get; set; }
        public ConsoleColor BackColor { get; set; }
        public string Text { get; set; }
        public int Length { get => Text.Length; }
        public static readonly string EmptyText = " ";

        public static Symbol Empty => FromText(EmptyText);

        public static Symbol FromText(string text)
        {
            return new Symbol
            {
                ForeColor = Settings.Colors.DefaultText,
                BackColor = Settings.Colors.DefaultBackground,
                Text = text,
            };
        }

        public static Symbol FromText(string text, ConsoleColor color)
        {
            return new Symbol
            {
                ForeColor = color,
                BackColor = Settings.Colors.DefaultBackground,
                Text = text,
            };
        }

        public static Symbol FromColor(ConsoleColor color)
        {
            return new Symbol
            {
                ForeColor = color,
                BackColor = color,
                Text = EmptyText,
            };
        }

        public Symbol CutOffStart(int newGridLength)
        {
            if (newGridLength >= Length)
                throw new InvalidOperationException("Can't cut a Symbol to a bigger or same GridLength.");

            return new Symbol
            {
                ForeColor = ForeColor,
                BackColor = BackColor,
                Text = Text[(Text.Length - newGridLength * 2)..],
            };
        }

        public Symbol CutOffEnd(int newGridLength)
        {
            if (newGridLength >= Length)
                throw new InvalidOperationException("Can't cut a Symbol to a bigger or same GridLength.");

            return new Symbol
            {
                ForeColor = ForeColor,
                BackColor = BackColor,
                Text = Text[..(newGridLength * 2)],
            };
        }

        public bool IsSameForDrawing(Symbol other)
        {
            return ForeColor == other.ForeColor && BackColor == other.BackColor;
        }

        public Symbol MergeWith(Symbol next)
        {
            return new Symbol
            {
                ForeColor = next.ForeColor,
                BackColor = next.BackColor,
                Text = Text + next.Text,
            };
        }

        public Symbol GetTranslucent(ConsoleColor color)
        {
            return new Symbol
            {
                ForeColor = ForeColor,
                BackColor = color,
                Text = Text,
            };
        }

        public override bool Equals(object obj)
        {
            return obj is Symbol symbol &&
                   ForeColor == symbol.ForeColor &&
                   BackColor == symbol.BackColor &&
                   Text == symbol.Text &&
                   Length == symbol.Length;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ForeColor, BackColor, Text, Length);
        }

        public static bool operator ==(Symbol left, Symbol right)
        {
            return left.Text == right.Text && left.BackColor == right.BackColor &&
                (left.ForeColor == right.ForeColor || left.Text == EmptyText);
        }

        public static bool operator !=(Symbol left, Symbol right)
        {
            return !(left == right);
        }
    }
}
