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
        public int GridX { get => _gridX; set { var oldValue = _gridX; _gridX = value; if (oldValue != value) Changed = true; } }
        public int GridY { get => _gridY; set { var oldValue = _gridY; _gridY = value; if (oldValue != value) Changed = true; } }
        public int GridZ { get => _gridZ; set { var oldValue = _gridZ; _gridZ = value; if (oldValue != value) Changed = true; } }
        public int Width { get => _width; set { var oldValue = _width; _width = value; if (oldValue != value) Changed = true; } }
        public int Height { get => _height; set { var oldValue = _height; _height = value; if (oldValue != value) Changed = true; } }
        public bool Visible { get => _visible; set { var oldValue = _visible; _visible = value; if (oldValue != value) Changed = true; } }
        internal bool Changed { get; set; } = true;
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
                RemoveChild(layer);
            }
        }

        public void RemoveAllChildren()
        {
            foreach (var layer in _innerGridChildren)
            {
                layer.Parent = null;
            }

            _innerGridChildren.Clear();
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
            if (_innerGridChildren.Any())
            {
                foreach (var childLayer in _innerGridChildren.OrderByDescending(cl => cl.GridZ))
                {
                    if (childLayer.GridX <= x && childLayer.GridY <= y &&
                        childLayer.GridX + childLayer.Width > x && childLayer.GridY + childLayer.Height > y)
                        return childLayer.GetSymbolAt(x - childLayer.GridX, y - childLayer.GridY);
                }
            }

            return GetLayerSymbolAt(x, y);
        }

        protected abstract Symbol GetLayerSymbolAt(int x, int y);
    }
}
