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

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindowViewModel context = this.Parent.DataContext as MainWindowViewModel;
        context.GoToSettings();
    }
}
