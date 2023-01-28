using System;

namespace ConsoleLayers.Core.ConcreteLayers
{
    public class Frame : Layer
    {
        public ConsoleColor ForeColor { get; set; } = Settings.Colors.DefaultText;
        public ConsoleColor BackColor { get; set; } = Settings.Colors.DefaultBackground;
        public IFramePalette Palette { get; set; } = FramePalettes.SingleLine;

        public Frame()
        {
        }

        public Frame(int gridX, int gridY, int width, int height) : base(gridX, gridY, width, height)
        {
        }

        protected override Symbol GetLayerSymbolAt(int x, int y)
        {
            char textChar = Symbol.EmptyChar;

            if (x == 0 && y == 0) textChar = Palette.SE;
            else if (x == 0 && y == Height - 1) textChar = Palette.NE;
            else if (x == Width - 1 && y == 0) textChar = Palette.SW;
            else if (x == Width - 1 && y == Height - 1) textChar = Palette.NW;

            else if (x == 0 || x == Width - 1) textChar = Palette.Vertical;

            else if (y == 0 || y == Height - 1) textChar = Palette.Horizontal;

            return new Symbol
            {
                ForeColor = ForeColor,
                BackColor = BackColor,
                Text = textChar.ToString(),
            };
        }
    }
}
