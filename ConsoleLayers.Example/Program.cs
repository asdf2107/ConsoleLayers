using ConsoleLayers.Core;
using ConsoleLayers.Core.ConcreteLayers;
using ConsoleLayers.Example.Layers;

Console.CursorVisible = false;
//Settings.Optimization = Optimization.DoNotMerge;

var drawLoopTask = Layers.StartLoop();

var backgroundLayer = new BackgroundLayer();

var layer1 = new Frame(2, 4, 20, 10);
layer1.AddChild(new Frame(0, 0, 10, 5) { BackColor = ConsoleColor.DarkYellow });

var layer2 = new Frame(50, 6, 35, 15)
{
    BackColor = ConsoleColor.Blue,
    Palette = FramePalettes.DoubleLine,
};

Layers.Add(backgroundLayer, layer1, layer2);
Layers.RenderAll();

await Task.Delay(1000);

var layer3 = new Frame(10, 10, 42, 12)
{
    ForeColor = ConsoleColor.Black,
    BackColor = ConsoleColor.Green,
};

Layers.Add(layer3);
Layers.RenderAll();

await Task.Delay(1000);

layer3.Visible = false;

Layers.RenderAll();

var layer4 = new Frame(-4, -4, 14, 7)
{
    BackColor = ConsoleColor.DarkRed,
};

Layers.Add(layer4);

for (int i = 0; i < 32; i++)
{
    layer4.GridX++;
    layer4.GridY++;

    if (i / 8 % 2 == 0)
        layer4.BackColor = ConsoleColor.Red;
    else
        layer4.BackColor = ConsoleColor.Cyan;
    Layers.RenderAll();

    await Task.Delay(50);
}

await Task.Delay(1000);

for (int i = 0; i < 32; i++)
{
    layer1.GridX++;

    Layers.RenderAll();

    await Task.Delay(50);
}

Console.ReadKey(false);

if (drawLoopTask.IsFaulted)
    throw drawLoopTask.Exception;