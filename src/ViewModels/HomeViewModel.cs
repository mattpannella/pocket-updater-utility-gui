using System.Collections.ObjectModel;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using pannella.analoguepocket.gui.Views;

namespace pannella.analoguepocket.gui.ViewModels;

public partial class HomeViewModel : ObservableObject
{
   // private HomeView _window;
    
    [ObservableProperty]
    private ObservableCollection<string> console = new ObservableCollection<string>();
    
    [ObservableProperty]
    private bool loaded = false;
    
    [ObservableProperty]
    private double progress = 0;
    
    [ObservableProperty]
    private int selectedItem = 0;

    //public HomeViewModel(HomeView window)
    public HomeViewModel()
    {
        //_window = window;
        LoadUpdater();
    }
    
    private async Task LoadUpdater()
    {
        Globals.Instance.Updater.StatusUpdated += updater_StatusUpdated;
        Globals.Instance.Updater.UpdateProcessComplete += updater_UpdateProcessComplete;
        Globals.Instance.Updater.SetDownloadProgressHandler(updater_ProgressUpdated);
        Loaded = true;
    }
    
    private void updater_ProgressUpdated(object sender, DownloadProgressEventArgs e)
    {
        Progress = e.progress;
    }

    private void StartUpdateClick()
    {
        RunUpdates();
    }
    
    private void StartAssetInstaller()
    {
        DownloadAssets();
    }
    
    private void StartJSONBuilder()
    {
        JSONBuilder();
    }
    
    private void StartFirmwareUpdate()
    {
        DownloadFirmware();
    }
    
    private async Task DownloadAssets()
    {
        Loaded = false;
        Console = new ObservableCollection<string>();
        await Globals.Instance.Updater.RunAssetDownloader();
        Loaded = true;
    }
    
    private async Task JSONBuilder()
    {
        Loaded = false;
        Console = new ObservableCollection<string>();
        await Globals.Instance.Updater.BuildInstanceJSON(true);
        Loaded = true;
    }
    
    private async Task DownloadFirmware()
    {
        Loaded = false;
        Console = new ObservableCollection<string>();
        await Globals.Instance.Updater.UpdateFirmware();
        Loaded = true;
    }

    private async Task RunUpdates()
    {
        Loaded = false;
        Console = new ObservableCollection<string>();
        await Globals.Instance.Updater.RunUpdates();
        Loaded = true;
        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow("Fin", "All updates complete");
        messageBoxStandardWindow.Show();
    }

    private void WriteLine(string text)
    {
        Console.Add(text);
        SelectedItem = Console.Count() - 1;
        //Dispatcher.UIThread.InvokeAsync(_window.ScrollTextToEnd);
    }

    private void updater_StatusUpdated(object sender, StatusUpdatedEventArgs e)
    {
        WriteLine(e.Message);
    }

    private void updater_UpdateProcessComplete(object sender, UpdateProcessCompleteEventArgs e)
    {
        WriteLine("-------------");
        WriteLine(e.Message);
        if (e.InstalledCores != null && e.InstalledCores.Count > 0)
        {
            WriteLine("Cores Updated:");
            foreach (Dictionary<string, string> core in e.InstalledCores)
            {
                WriteLine(core["core"] + " " + core["version"]);
            }
            WriteLine("");
        }
        if (e.InstalledAssets.Count > 0)
        {
            WriteLine("Assets Installed:");
            foreach (string asset in e.InstalledAssets)
            {
                WriteLine(asset);
            }
            WriteLine("");
        }
        if (e.SkippedAssets.Count > 0)
        {
            WriteLine("Assets Not Found:");
            foreach (string asset in e.SkippedAssets)
            {
                WriteLine(asset);
            }
            WriteLine("");
        }
        if (e.FirmwareUpdated != "")
        {
            WriteLine("New Firmware was downloaded. Restart your Pocket to install");
            WriteLine(e.FirmwareUpdated);
            WriteLine("");
        }
        WriteLine("we did it, come again soon");
    }
}

