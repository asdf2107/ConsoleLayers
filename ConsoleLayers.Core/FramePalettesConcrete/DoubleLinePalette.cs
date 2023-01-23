namespace ConsoleLayers.Core.FramePalettesConcrete
{
    internal class DoubleLinePalette : IFramePalette
    {
        public char Horizontal => '═';
        public char Vertical => '║';
        public char NW => '╝';
        public char NE => '╚';
        public char SW => '╗';
        public char SE => '╔';
    }
}
