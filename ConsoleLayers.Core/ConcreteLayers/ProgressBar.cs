using ConsoleLayers.Core.Tools;
using System;

namespace ConsoleLayers.Core.ConcreteLayers
{
    public class ProgressBar : Layer
    {
        public ConsoleColor ProgressForeColor { get; set; } = Settings.Colors.DefaultText;
        public ConsoleColor ProgressBackColor { get; set; } = Settings.Colors.DefaultBackground;
        public ConsoleColor LeftForeColor { get; set; } = Settings.Colors.DefaultText;
        public ConsoleColor LeftBackColor { get; set; } = Settings.Colors.DefaultBackground;
        public char ProgressLoadingChar { get; set; } = '█';
        public char LeftLoadingChar { get; set; } = Symbol.EmptyChar;
        /// <summary>
        /// Progress value, should be from 0.0d to 1.0d.
        /// </summary>
        public double Value { get; set; }

        public ProgressBar()
        {
        }

        public ProgressBar(int gridX, int gridY, int width) : base(gridX, gridY, width, 1)
        {
        }

        protected override Symbol GetLayerSymbolAt(int x, int y)
        {
            int roundValue = (int)(Value.Clamp(0d, 1d) * Width);

            return x > roundValue ?
                new Symbol
                {
                    ForeColor = LeftForeColor,
                    BackColor = LeftBackColor,
                    Text = LeftLoadingChar.ToString(),
                } :
                new Symbol
                {
                    ForeColor = ProgressForeColor,
                    BackColor = ProgressBackColor,
                    Text = ProgressLoadingChar.ToString(),
                };
        }
    }
}
