using ConsoleLayers.Core;
using ConsoleLayers.Core.Components;
using ConsoleLayers.Core.Tools;
using ConsoleLayers.Example.Layers;

Layer layer1 = null;
Task drawLoopTask;

Setup();
await Step1();
await Step2();
await Step3();
Finish();

void Setup()
{
    Console.CursorVisible = false;
    //Settings.Optimization = Optimization.DoNotMerge;

    drawLoopTask = Layers.StartLoop();
}

async Task Step1()
{
    var backgroundLayer = new BackgroundLayer();

    layer1 = new Frame(2, 4, 20, 10);
    layer1.AddChild(new Frame(2, 2, 10, 5) { BackColor = ConsoleColor.DarkYellow });

    var layer2 = new Frame(50, 6, 35, 15)
    {
        BackColor = ConsoleColor.Blue,
        Palette = FramePalettes.DoubleLine,
    };

    Layers.Add(backgroundLayer, layer1, layer2);
    Layers.RenderAll();

    await Task.Delay(1000);
}

static async Task Step2()
{
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
}

async Task Step3()
{
    var layer4 = new Frame(-4, -4, 14, 7)
    {
        BackColor = ConsoleColor.DarkRed,
    };

    var progressLayer = new Frame(0, Settings.Grid.Height - 4, Settings.Grid.Width, 3);

    var progressBar = new ProgressBar(1, 1, Settings.Grid.Width - 2)
    {
        LeftLoadingChar = '-',
    };

    var progressText = new Text((Settings.Grid.Width - 4) / 2 - 4, 0, 10, 1);

    progressText.Lines[0].Add(Symbol.FromText("Loading..."));

    progressLayer.AddChildren(progressBar, progressText);

    Layers.Add(layer4, progressLayer);

    for (int i = 0; i < 30; i++)
    {
        layer4.GridY++;

        if (i / 5 % 2 == 0)
            layer4.BackColor = ConsoleColor.Red;
        else
            layer4.BackColor = ConsoleColor.Cyan;

        progressBar.Value += 0.01d;

        Layers.RenderAll();
        await Task.Delay(100);
    }

    await Task.Delay(1000);

    for (int i = 0; i < 70; i++)
    {
        layer1.GridX++;
        progressBar.Value += 0.01d;

        Layers.RenderAll();
        await Task.Delay(50);
    }
}

void Finish()
{
    if (PlatformHelper.IsWindows)
    {
        Console.Beep();
    }

    Console.ReadKey(false);

    if (drawLoopTask.IsFaulted)
        throw drawLoopTask.Exception;
}