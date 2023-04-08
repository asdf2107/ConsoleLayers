using ConsoleLayers.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleLayers.Core
{
    public static class Layers
    {
        private static readonly List<Layer> _layers = new();
        internal static readonly int[,] ZMap = new int[Settings.Grid.Width, Settings.Grid.Height];

        public static int MaxZ => _layers.Any() ? _layers.Max(l => l.GridZ) : -1;

        public static Task StartLoop()
        {
            return ScreenDrawer.StartDrawLoop();
        }

        public static void RenderAll()
        {
            if (AnyLayerChanged())
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

        /// <summary>
        /// Adds layer to list of layers to render. If GridZ is default (0), the layer is added to the top with appropriate value set in GridZ.
        /// </summary>
        public static void Add(Layer layer)
        {
            if (!_layers.Contains(layer))
            {
                if (layer.GridZ == 0)
                    layer.GridZ = MaxZ + 1;

                _layers.Add(layer);
            }
        }

        /// <summary>
        /// Adds layers to list of layers to render. If GridZ is default (0), the layer is added to the top with appropriate value set in GridZ.
        /// </summary>
        public static void Add(params Layer[] layers)
        {
            foreach (var layer in layers)
            {
                Add(layer);
            }
        }

        /// <summary>
        /// Removes layer from list of layers to render.
        /// </summary>
        public static void Remove(Layer layer)
        {
            if (_layers.Remove(layer))
                RegenerateZMap();
        }

        internal static void RegenerateZMap()
        {
            Array.Clear(ZMap);

            foreach (var layer in _layers
                .Where(l => l.Visible)
                .OrderBy(l => l.GridZ))
            {
                if (_layers.Count(l => l.GridZ == layer.GridZ) > 1)
                    throw new InvalidOperationException("Multiple identical GridZ values.");

                for (int j = ClampVertical(layer.GridY); j < ClampVertical(layer.GridY + layer.Height); j++)
                {
                    for (int i = ClampHorizontal(layer.GridX); i < ClampHorizontal(layer.GridX + layer.Width); i++)
                    {
                        ZMap[i, j] = layer.GridZ;
                    }
                }
            }

            ResetChanged();
        }

        internal static bool AnyLayerChanged()
        {
            return _layers.Any(l => l.Changed);
        }

        private static void ResetChanged()
        {
            foreach (var layer in _layers)
            {
                layer.Changed = false;
            }
        }

        internal static int ClampHorizontal(int x) => x.Clamp(0, Settings.Grid.Width);

        internal static int ClampVertical(int y) => y.Clamp(0, Settings.Grid.Height);
    }
}