using ConsoleLayers.Core;
using ConsoleLayers.Example.Layers;

var drawLoopTask = ScreenDrawer.StartDrawLoop();

var bg = new BackgroundLayer();

var p1 = new PopupLayer(2, 4, 20, 10);

var p2 = new PopupLayer(50, 6, 35, 15)
{
    BackColor = ConsoleColor.Blue,
};

Layer.RenderAll();

await Task.Delay(1000);

var p3 = new PopupLayer(10, 10, 40, 12)
{
    ForeColor = ConsoleColor.Black,
    BackColor = ConsoleColor.Green,
};

Layer.RenderAll();

await Task.Delay(1000);

p3.Visible = false;

Layer.RenderAll();



Console.ReadKey();

if (drawLoopTask.IsFaulted)
    throw drawLoopTask.Exception;