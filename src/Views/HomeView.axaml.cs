using Avalonia.Controls;
using Avalonia.Interactivity;
using pannella.analoguepocket.gui.ViewModels;

namespace pannella.analoguepocket.gui.Views;

public partial class HomeView : UserControl
{
    //private ListBox messageLogScrollViewer; 
    public HomeView()
    {
        InitializeComponent();
        
        //DataContext = new HomeViewModel(this);
        DataContext = new HomeViewModel();
      //  messageLogScrollViewer = this.FindControl<ListBox>("OutputBox");
    }

    public void ScrollTextToEnd()
    {
      //  messageLogScrollViewer.ScrollIntoView(messageLogScrollViewer.ItemCount - 1);
    }

    private void SettingsClick(object? sender, RoutedEventArgs e)
    {
        MainWindowViewModel context = this.Parent.DataContext as MainWindowViewModel;
        context.GoToSettings();
    }

    private void ExitClick(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    private void CoresClick(object? sender, RoutedEventArgs e)
    {
        MainWindowViewModel context = this.Parent.DataContext as MainWindowViewModel;
        context.GoToCores();
    }
}
