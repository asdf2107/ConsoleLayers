using System;

#pragma warning disable CA2211

namespace ConsoleLayers.Core
{
    public static class Settings
    {
        public static class Grid
        {
            public static int Width = Console.WindowWidth - 1;
            public static int Height = Console.WindowHeight - 1;
        }

        public static class Colors
        {
            public static ConsoleColor DefaultBackground = ConsoleColor.Black;
            public static ConsoleColor DefaultText = ConsoleColor.White;
        }

        public static Optimization Optimization = Optimization.Merge;
    }

    /// <summary>
    /// Optimization mode.
    /// </summary>
    public enum Optimization
    {
        /// <summary>
        /// Merge consecutive symbols if possible. (recommended)
        /// </summary>
        Merge = 0,
        /// <summary>
        /// Do not merge consecutive symbols.
        /// </summary>
        DoNotMerge = 1,
    }
}

#pragma warning restore CA2211
