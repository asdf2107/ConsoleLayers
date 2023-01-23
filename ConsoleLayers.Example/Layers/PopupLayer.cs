using ConsoleLayers.Core;

namespace ConsoleLayers.Example.Layers
{
    internal class PopupLayer : Layer
    {
        public ConsoleColor ForeColor { get; set; } = ConsoleColor.White;
        public ConsoleColor BackColor { get; set; } = ConsoleColor.DarkMagenta;

        public PopupLayer(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }

        protected override Symbol GetSymbolAt(int x, int y)
        {
            if (x == 0 && y == 0 ||
                x == 0 && y == Height - 1 ||
                x == Width - 1 && y == 0 ||
                x == Width - 1 && y == Height - 1)
            {
                return new Symbol
                {
                    ForeColor = ForeColor,
                    BackColor = BackColor,
                    Text = "+",
                };
            }

            if (x == 0 || x == Width - 1)
                return new Symbol
                {
                    ForeColor = ForeColor,
                    BackColor = BackColor,
                    Text = "|",
                };

            if (y == 0 || y == Height - 1)
                return new Symbol
                {
                    ForeColor = ForeColor,
                    BackColor = BackColor,
                    Text = "-",
                };

            return Symbol.FromColor(BackColor);
        }
    }
}
