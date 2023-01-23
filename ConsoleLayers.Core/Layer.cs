using System;
using System.Collections.Generic;
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
        public int GridX { get => _gridX; set { _gridX = value; Layers.SetChanged(); } }
        public int GridY { get => _gridY; set { _gridY = value; Layers.SetChanged(); } }
        public int GridZ { get => _gridZ; set { _gridZ = value; Layers.SetChanged(); } }
        public int Width { get => _width; set { _width = value; Layers.SetChanged(); } }
        public int Height { get => _height; set { _height = value; Layers.SetChanged(); } }
        public bool Visible { get => _visible; set { _visible = value; Layers.SetChanged(); } }

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

        public Layer(int gridX, int gridY, int gridZ, int width, int height)
        {
            GridX = gridX;
            GridY = gridY;
            GridZ = gridZ;
            Width = width;
            Height = height;
        }

        public void Render()
        {
            if (Layers.Changed)
                Layers.RegenerateZMap();

            ScreenDrawer.Draw(GetRenderSymbols());
        }

        internal List<LocatedSymbol> GetRenderSymbols()
        {
            var locatedSymbols = new List<LocatedSymbol>();

            for (int j = 0; j < Height; j++)
            {
                var lineSymbols = new List<LocatedSymbol>();

                for (int i = 0; i < Width; i++)
                {
                    if (Layers.ZMap[GridX + i, GridY + j] == GridZ)
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
