using ConsoleLayers.Core;

namespace ConsoleLayers.Example.Layers
{
    internal class BackgroundLayer : Layer
    {
        protected override Symbol GetSymbolAt(int x, int y)
        {
            return (x) % 4 == 0 ? Symbol.FromText("X") : Symbol.Empty;
        }
    }
}
