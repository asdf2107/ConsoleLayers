using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleLayers.Core
{
    public abstract class Layer
    {
        private int _gridX = 0;
        private int _gridY = 0;
        private int _gridZ = MaxZ + 1;
        private int _width = Settings.Grid.Width;
        private int _height = Settings.Grid.Height;
        private bool _visible = true;
        public int GridX { get => _gridX; set { _gridX = value; _changedLayers = true; } }
        public int GridY { get => _gridY; set { _gridY = value; _changedLayers = true; } }
        public int GridZ { get => _gridZ; set { _gridZ = value; _changedLayers = true; } }
        public int Width { get => _width; set { _width = value; _changedLayers = true; } }
        public int Height { get => _height; set { _height = value; _changedLayers = true; } }
        public bool Visible { get => _visible; set { _visible = value; _changedLayers = true; } }

        #region Static

        private static bool _changedLayers = false;
        private static readonly List<Layer> _layers = new();
        private static readonly int[,] _zMap = new int[Settings.Grid.Width, Settings.Grid.Height];
        public static int MaxZ => _layers.Any() ? _layers.Max(l => l.GridZ) : -1;

        public static void RenderAll()
        {
            if (_changedLayers)
                RegenerateZMap();

            var locatedSymbols = new List<LocatedSymbol>();

            foreach (var layer in _layers)
            {
                locatedSymbols.AddRange(layer.GetRenderSymbols());
            }

            //locatedSymbols.Sort((l, r) => GetSymbolPositionNum(l) > GetSymbolPositionNum(r) ? 1 : -1); //buggy

            //static int GetSymbolPositionNum(LocatedSymbol locatedSymbol) =>
            //    locatedSymbol.GridY * Settings.Grid.Width + locatedSymbol.GridX;

            ScreenDrawer.Draw(locatedSymbols);
        }

        private static void RegenerateZMap()
        {
            Array.Clear(_zMap);

            foreach (var layer in _layers
                .Where(l => l.Visible)
                .OrderBy(l => l.GridZ))
            {
                if (_layers.Count(l => l.GridZ == layer.GridZ) > 1)
                    throw new InvalidOperationException("Multiple identical GridZ values.");

                for (int j = layer.GridY; j < layer.GridY + layer.Height; j++)
                {
                    for (int i = layer.GridX; i < layer.GridX + layer.Width; i++)
                    {
                        _zMap[i, j] = layer.GridZ;
                    }
                }
            }

            _changedLayers = false;
        }

        private static void AddLayer(Layer layer)
        {
            _layers.Add(layer);
            _changedLayers = true;
        }

        #endregion

        public Layer()
        {
            AddLayer(this);
        }

        public Layer(int gridX, int gridY, int width, int height)
        {
            GridX = gridX;
            GridY = gridY;
            Width = width;
            Height = height;

            AddLayer(this);
        }

        public Layer(int gridX, int gridY, int gridZ, int width, int height)
        {
            GridX = gridX;
            GridY = gridY;
            GridZ = gridZ;
            Width = width;
            Height = height;

            AddLayer(this);
        }

        public void Render()
        {
            if (_changedLayers)
                RegenerateZMap();

            ScreenDrawer.Draw(GetRenderSymbols());
        }

        private List<LocatedSymbol> GetRenderSymbols()
        {
            var locatedSymbols = new List<LocatedSymbol>();

            for (int j = 0; j < Height; j++)
            {
                var lineSymbols = new List<LocatedSymbol>();

                for (int i = 0; i < Width; i++)
                {
                    if (_zMap[GridX + i, GridY + j] == GridZ)
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

        public void RenderFields(IEnumerable<(int, int)> coords)
        {
            foreach (var coord in coords)
            {
                RenderField(coord.Item1, coord.Item2);
            }
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

        protected abstract Symbol GetSymbolAt(int x, int y);
    }
}
