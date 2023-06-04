using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace pannella.analoguepocket.gui;

public class CoreManagerRow : TemplatedControl
{
    public static readonly StyledProperty<string> CoreNameProperty = AvaloniaProperty.Register<CoreManagerRow, string>(
        nameof(CoreName),"meep");

    public string CoreName
    {
        get => GetValue(CoreNameProperty);
        set => SetValue(CoreNameProperty, value);
    }

    public static readonly StyledProperty<bool> SelectedProperty = AvaloniaProperty.Register<CoreManagerRow, bool>(
        nameof(Selected));

    public bool Selected
    {
        get => GetValue(SelectedProperty);
        set => SetValue(SelectedProperty, value);
    }

    public static readonly StyledProperty<string> platformProperty = AvaloniaProperty.Register<CoreManagerRow, string>(
        "platform");

    public string platform
    {
        get => GetValue(platformProperty);
        set => SetValue(platformProperty, value);
    }

    public static readonly StyledProperty<string> categoryProperty = AvaloniaProperty.Register<CoreManagerRow, string>(
        "category");

    public string category
    {
        get => GetValue(categoryProperty);
        set => SetValue(categoryProperty, value);
    }

    public static readonly StyledProperty<string> versionProperty = AvaloniaProperty.Register<CoreManagerRow, string>(
        "version");

    public string version
    {
        get => GetValue(versionProperty);
        set => SetValue(versionProperty, value);
    }
}