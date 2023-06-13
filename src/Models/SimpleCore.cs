using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace pannella.analoguepocket.gui.Models;

public partial class SimpleCore : ObservableObject
{
    [ObservableProperty]
    public string identifier;
    
    [ObservableProperty]
    public bool track;

    [ObservableProperty]
    public string? platform;
    
    [ObservableProperty]
    public string category;

    [ObservableProperty]
    public Analogue.Config? config;

    [ObservableProperty] 
    public Bitmap? image;

    [ObservableProperty] 
    public string platformId;

    public async Task SetImage()
    {
        string path = Path.Combine(GlobalHelper.Instance.UpdateDirectory, "Platforms", "_images/", platformId + ".bin");
        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
            Image = Helpers.ImageRenderer.RenderBinImage(bytes, 521, 165, true);
        }
    }
}