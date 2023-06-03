using System.Collections.ObjectModel;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using pannella.analoguepocket.gui.Views;

namespace pannella.analoguepocket.gui.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private MainWindow _window;
    
    [ObservableProperty]
    private ObservableCollection<string> console = new ObservableCollection<string>();
    
    [ObservableProperty]
    private bool loaded = false;
    
    [ObservableProperty]
    private double progress = 0;

    public MainWindowViewModel(MainWindow window)
    {
        _window = window;
        LoadUpdater();
    }
    
    private async Task LoadUpdater()
    {
        WriteLine("loading...");
        string location = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        string? path = Path.GetDirectoryName(location);
        path = "/Users/mattpannella/pocket-test";
        PocketCoreUpdater updater = new PocketCoreUpdater(path);
        SettingsManager settings = new SettingsManager(path);

        updater.DeleteSkippedCores(settings.GetConfig().delete_skipped_cores);
        updater.SetGithubApiKey(settings.GetConfig().github_token);
        updater.DownloadFirmware(settings.GetConfig().download_firmware);
        updater.RenameJotegoCores(settings.GetConfig().fix_jt_names);
        updater.StatusUpdated += updater_StatusUpdated;
        updater.UpdateProcessComplete += updater_UpdateProcessComplete;
        updater.DownloadAssets(settings.GetConfig().download_assets);
        updater.SetDownloadProgressHandler(updater_ProgressUpdated);
        await updater.Initialize();

        Globals.Instance.Updater = updater;
        Globals.Instance.SettingsManager = settings;
        WriteLine("done");
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

    private async Task RunUpdates()
    {
        Loaded = false;
        Console = new ObservableCollection<string>();
        await Globals.Instance.Updater.RunUpdates();
        Loaded = true;
    }

    private void WriteLine(string text)
    {
        Console.Add(text);
        Dispatcher.UIThread.InvokeAsync(_window.ScrollTextToEnd);
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

