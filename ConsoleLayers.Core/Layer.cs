using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ConsoleLayers.Core
{
    public abstract class Layer
    {
        private int _gridX = 0;
        private int _gridY = 0;
        private int _gridZ = 0;
        private int _width = Settings.Grid.Width;
        private int _height = Settings.Grid.Height;
        private bool _visible = true;
        private List<Layer> _innerGridChildren = new();
        public int GridX { get => _gridX; set { _gridX = value; Changed = true; } }
        public int GridY { get => _gridY; set { _gridY = value; Changed = true; } }
        public int GridZ { get => _gridZ; set { _gridZ = value; Changed = true; } }
        public int Width { get => _width; set { _width = value; Changed = true; } }
        public int Height { get => _height; set { _height = value; Changed = true; } }
        public bool Visible { get => _visible; set { _visible = value; Changed = true; } }
        internal bool Changed { get; set; } = true;
        public int InnerGridLeft { get; set; } = 1;
        public int InnerGridRight { get; set; } = 1;
        public int InnerGridTop { get; set; } = 1;
        public int InnerGridBottom { get; set; } = 1;
        public ReadOnlyCollection<Layer> InnerGridChildren => _innerGridChildren.AsReadOnly();
        public Layer Parent { get; private set; }

        public Layer()
        {
        }

        public Layer(int gridX, int gridY, int width, int height)
        {
            GridX = gridX;
            GridY = gridY;
            Width = width;
            Height = height;
        }

        public void AddChild(Layer layer)
        {
            if (!_innerGridChildren.Contains(layer))
            {
                _innerGridChildren.Add(layer);
                layer.Parent = this;
            }
        }

        public void AddChildren(params Layer[] layers)
        {
            foreach (var layer in layers)
            {
                AddChild(layer);
            }
        }

        public void RemoveChild(Layer layer)
        {
            if (_innerGridChildren.Contains(layer))
            {
                _innerGridChildren.Remove(layer);
                layer.Parent = null;
            }
        }

        public void RemoveChildren(params Layer[] layers)
        {
            foreach (var layer in layers)
            {
                RemoveChildren(layer);
            }
        }

        public void Render()
        {
            if (Layers.AnyLayerChanged())
                Layers.RegenerateZMap();

            ScreenDrawer.Draw(GetRenderSymbols());
        }

        internal List<LocatedSymbol> GetRenderSymbols()
        {
            var locatedSymbols = new List<LocatedSymbol>();

            for (int j = Layers.ClampVertical(-GridY); j < Height - Layers.ClampVertical(-Settings.Grid.Height + GridY + Height); j++)
            {
                var lineSymbols = new List<LocatedSymbol>();

                for (int i = Layers.ClampHorizontal(-GridX); i < Width - Layers.ClampHorizontal(-Settings.Grid.Width + GridX + Width); i++)
                {
                    if (Parent != null || Layers.ZMap[GridX + i, GridY + j] == GridZ)
                    {
                        lineSymbols.Add(GetLocatedSymbolAt(i, j));
                    }
                }

                locatedSymbols.AddRange(lineSymbols);
            }

            return locatedSymbols;
        }

        public void RenderField(int x, int y)
        {
            ScreenDrawer.Draw(GetLocatedSymbolAt(x, y));
        }

        private LocatedSymbol GetLocatedSymbolAt(int x, int y)
        {
            return new LocatedSymbol
            {
                GridX = GridX + x,
                GridY = GridY + y,
                Symbol = GetSymbolAt(x, y),
            };
        }

        private Symbol GetSymbolAt(int x, int y)
        {
            if (_innerGridChildren.Any() &&
                x >= InnerGridLeft && x < Width - InnerGridRight &&
                y >= InnerGridTop && y < Height - InnerGridBottom)
            {
                int innerX = x - InnerGridLeft;
                int innerY = y - InnerGridTop;
                foreach (var childLayer in _innerGridChildren.OrderByDescending(cl => cl.GridZ))
                {
                    if (childLayer.GridX <= innerX && childLayer.GridY <= innerY &&
                        childLayer.GridX + childLayer.Width > innerX && childLayer.GridY + childLayer.Height > innerY)
                        return childLayer.GetSymbolAt(innerX - childLayer.GridX, innerY - childLayer.GridY);
                }
            }

            return GetLayerSymbolAt(x, y);
        }

        protected abstract Symbol GetLayerSymbolAt(int x, int y);
    }
}
