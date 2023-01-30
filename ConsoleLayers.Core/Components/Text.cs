using System.Collections.Generic;

namespace ConsoleLayers.Core.Components
{
    public class Text : ComponentLayer
    {
        /// <summary>
        /// Represents text linest, where Lines[y] - each line of text.
        /// </summary>
        public List<List<Symbol>> Lines { get; protected set; } = new();

        public Text()
        {
            InitLines();
        }

        public Text(int gridX, int gridY, int width, int height) : base(gridX, gridY, width, height)
        {
            InitLines();
        }

        protected void InitLines()
        {
            Lines.Clear();

            for (int i = 0; i < Height; i++)
            {
                Lines.Add(new List<Symbol>());
            }
        }

        protected override Symbol GetLayerSymbolAt(int x, int y)
        {
            var line = Lines[y];

            int position = 0;

            foreach (var symbol in line)
            {
                if (symbol.Length > x - position)
                    return symbol[x - position];
                position += symbol.Length;
            }

            return Symbol.FromColor(BackColor);
        }
    }
}
