using ConsoleLayers.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace ConsoleLayers.Core
{
    public struct LocatedSymbol
    {
        public Symbol Symbol { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
        public bool IsSingleLength => Symbol.Length == 1;

        public bool CanBeMergedWith(LocatedSymbol next)
        {
            return GridY == next.GridY &&
                GridX + Symbol.Length == next.GridX &&
                Symbol.IsSameForDrawing(next.Symbol);
        }

        public LocatedSymbol MergeWith(LocatedSymbol next)
        {
            return new LocatedSymbol
            {
                GridX = GridX,
                GridY = GridY,
                Symbol = Symbol.MergeWith(next.Symbol),
            };
        }

        public LocatedSymbol CutOffStart(int newGridLength)
        {
            return new LocatedSymbol
            {
                GridX = GridX,
                GridY = GridY,
                Symbol = Symbol.CutOffStart(newGridLength),
            };
        }

        public LocatedSymbol CutOffEnd(int newGridLength)
        {
            return new LocatedSymbol
            {
                GridX = GridX + (Symbol.Length - newGridLength),
                GridY = GridY,
                Symbol = Symbol.CutOffEnd(newGridLength),
            };
        }

        public void ThrowIfNotSingleLength()
        {
            if (!IsSingleLength)
                throw new InvalidSymbolException("Only single-length LocatedSymbols are allowed in this context.");
        }

        public override bool Equals(object obj)
        {
            return obj is LocatedSymbol symbol &&
                   EqualityComparer<Symbol>.Default.Equals(Symbol, symbol.Symbol) &&
                   GridX == symbol.GridX &&
                   GridY == symbol.GridY;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Symbol, GridX, GridY);
        }

        public static bool operator ==(LocatedSymbol left, LocatedSymbol right)
        {
            return left.GridX == right.GridX &&
                left.GridY == right.GridY &&
                left.Symbol == right.Symbol;
        }

        public static bool operator !=(LocatedSymbol left, LocatedSymbol right)
        {
            return !(left == right);
        }
    }
}
