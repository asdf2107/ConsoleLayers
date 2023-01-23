using ConsoleLayers.Core.FramePalettesConcrete;

namespace ConsoleLayers.Core
{
    public static class FramePalettes
    {
        public static IFramePalette SingleLine { get; private set; } = new SingleLinePalette();
        public static IFramePalette DoubleLine { get; private set; } = new DoubleLinePalette();
    }
}
