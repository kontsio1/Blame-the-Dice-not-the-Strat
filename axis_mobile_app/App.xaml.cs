using Microsoft.Extensions.DependencyInjection;

namespace axis_mobile_app;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell()) { Width = 650, Height = 800 };

        return window;
    }
}
