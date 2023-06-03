using Avalonia.Controls;
using pannella.analoguepocket.gui.ViewModels;

namespace pannella.analoguepocket.gui.Views;

public partial class MainWindow : Window
{
    private ListBox messageLogScrollViewer; 
    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = new MainWindowViewModel(this);
        messageLogScrollViewer = this.FindControl<ListBox>("OutputBox");
    }
    
    public void ScrollTextToEnd()
    {
        messageLogScrollViewer.ScrollIntoView(messageLogScrollViewer.ItemCount - 1);
    }
}
