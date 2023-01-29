using System;

namespace ConsoleLayers.Core
{
    public abstract class ComponentLayer : Layer
    {
        public ConsoleColor ForeColor { get; set; } = Settings.Colors.DefaultText;
        public ConsoleColor BackColor { get; set; } = Settings.Colors.DefaultBackground;

        protected ComponentLayer()
        {
        }

        protected ComponentLayer(int gridX, int gridY, int width, int height) : base(gridX, gridY, width, height)
        {
        }
    }
}
