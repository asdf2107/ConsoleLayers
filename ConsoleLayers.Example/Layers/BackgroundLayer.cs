using ConsoleLayers.Core;

namespace ConsoleLayers.Example.Layers
{
    internal class BackgroundLayer : Layer
    {
        protected override Symbol GetLayerSymbolAt(int x, int y)
        {
            return (x) % 4 == 0 ? Symbol.FromText("X", ConsoleColor.DarkGray) : Symbol.Empty;
        }
    }
}
