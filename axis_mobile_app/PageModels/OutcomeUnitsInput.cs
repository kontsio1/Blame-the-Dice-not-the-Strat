using CommunityToolkit.Mvvm.ComponentModel;

namespace axis_mobile_app.PageModels;

public partial class OutcomeUnitsInput : ObservableObject
{
    [ObservableProperty] private int _infantry;
    [ObservableProperty] private int _artillery;
    [ObservableProperty] private int _tank;
    [ObservableProperty] private int _fighter;
    [ObservableProperty] private int _bomber;
    [ObservableProperty] private int _antiAir;
    [ObservableProperty] private int _transport;
    [ObservableProperty] private int _submarine;
    [ObservableProperty] private int _destroyer;
    [ObservableProperty] private int _cruiser;
    [ObservableProperty] private int _battleship;
    [ObservableProperty] private int _carrier;

    public void Reset()
    {
        Infantry = 0;
        Artillery = 0;
        Tank = 0;
        Fighter = 0;
        Bomber = 0;
        AntiAir = 0;
        Transport = 0;
        Submarine = 0;
        Destroyer = 0;
        Cruiser = 0;
        Battleship = 0;
        Carrier = 0;
    }
}

