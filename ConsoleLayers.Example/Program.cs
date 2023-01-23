using ConsoleLayers.Core;
using ConsoleLayers.Example.Layers;

Console.CursorVisible = false;

var drawLoopTask = ScreenDrawer.StartDrawLoop();

var bg = new BackgroundLayer();

var p1 = new PopupLayer(2, 4, 20, 10);

var p2 = new PopupLayer(50, 6, 35, 15)
{
    BackColor = ConsoleColor.Blue,
};

Layer.RenderAll();

await Task.Delay(1000);

var p3 = new PopupLayer(10, 10, 42, 12)
{
    ForeColor = ConsoleColor.Black,
    BackColor = ConsoleColor.Green,
};

Layer.RenderAll();

await Task.Delay(1000);

p3.Visible = false;

Layer.RenderAll();

var p4 = new PopupLayer(0, 0, 10, 5)
{
    BackColor = ConsoleColor.DarkRed,
};

for (int i = 0; i < 20; i++)
{
    p4.GridX++;
    p4.GridY++;

    Layer.RenderAll();

    await Task.Delay(50);
}


Console.ReadKey(false);

if (drawLoopTask.IsFaulted)
    throw drawLoopTask.Exception;