using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleLayers.Core
{
    public static class ScreenDrawer
    {
        private static readonly ConcurrentQueue<LocatedSymbol> _drawQueue = new();
        private static readonly ConcurrentDictionary<(int, int), LocatedSymbol> _symbols = new();

        public static void Draw(LocatedSymbol locatedSymbol)
        {
            _drawQueue.Enqueue(locatedSymbol);
        }

        public static void Draw(IEnumerable<LocatedSymbol> locatedSymbols)
        {
            lock(_drawQueue)
            {
                foreach (var locatedSymbol in locatedSymbols)
                {
                    _drawQueue.Enqueue(locatedSymbol);
                }
            }
        }

        public static Task StartDrawLoop(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => DrawLoop(), cancellationToken);
        }

        private static void DrawLoop()
        {
            while (true)
            {
                if (!_drawQueue.IsEmpty)
                {
                    lock (_drawQueue)
                    {
                        bool success = _drawQueue.TryDequeue(out LocatedSymbol first);
                        if (!success)
                            throw new Exception("Failed to dequeue locatedSymbol.");
                        first.ThrowIfNotSingleLength();

                        if (IsAlreadyDrawn(first))
                            continue;
                        else
                            SetAsAlreadyDrawn(first);

                        if (Settings.Optimization == Optimization.Merge)
                        {
                            while (!_drawQueue.IsEmpty)
                            {
                                var next = _drawQueue.First();

                                if (IsAlreadyDrawn(next))
                                    break;

                                if (first.CanBeMergedWith(next))
                                {
                                    next.ThrowIfNotSingleLength();
                                    SetAsAlreadyDrawn(next);

                                    first = first.MergeWith(next);
                                    if (!_drawQueue.TryDequeue(out _))
                                        throw new Exception("Failed to dequeue locatedSymbol.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        PerformDraw(first);
                    }
                }
            }
        }

        private static bool IsAlreadyDrawn(LocatedSymbol locatedSymbol)
        {
            var key = (locatedSymbol.GridX, locatedSymbol.GridY);
            return _symbols.ContainsKey(key) && locatedSymbol == _symbols[key];
        }

        private static void SetAsAlreadyDrawn(LocatedSymbol locatedSymbol)
        {
            _symbols.AddOrUpdate((locatedSymbol.GridX, locatedSymbol.GridY), locatedSymbol, (_, _) => locatedSymbol);
        }

        private static void PerformDraw(LocatedSymbol located)
        {
            if (Console.ForegroundColor != located.Symbol.ForeColor && !string.IsNullOrEmpty(located.Symbol.Text))
                Console.ForegroundColor = located.Symbol.ForeColor;

            if (Console.BackgroundColor != located.Symbol.BackColor)
                Console.BackgroundColor = located.Symbol.BackColor;

            if (Console.CursorLeft != located.GridX - 1 || Console.CursorTop != located.GridY)
                Console.SetCursorPosition(located.GridX, located.GridY);

            Console.Write(located.Symbol.Text);
        }
    }
}
