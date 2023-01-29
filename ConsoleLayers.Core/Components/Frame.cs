namespace ConsoleLayers.Core.Components
{
    public class Frame : ComponentLayer
    {
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
