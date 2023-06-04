using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using pannella.analoguepocket.gui.ViewModels;

namespace pannella.analoguepocket.gui.Views;

public partial class CoresView : UserControl
{
    public CoresView()
    {
        InitializeComponent();
        DataContext = new CoresViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private void GoHome(object? sender, RoutedEventArgs e)
    {
        MainWindowViewModel context = this.Parent.DataContext as MainWindowViewModel;
        context.GoToHome();
    }
    
}