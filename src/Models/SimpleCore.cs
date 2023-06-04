using CommunityToolkit.Mvvm.ComponentModel;

namespace pannella.analoguepocket.gui.Models;

public partial class SimpleCore : ObservableObject
{
    [ObservableProperty]
    public string identifier;
    
    [ObservableProperty]
    public bool track;

    [ObservableProperty] public string? platform;
    [ObservableProperty] public string category;
}