using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using pannella.analoguepocket.gui.ViewModels;
using pannella.analoguepocket.gui.Views;
using System.IO;

namespace pannella.analoguepocket.gui;

public partial class App : Application
{
    public override void Initialize()
    {
        LoadUpdater();
        AvaloniaXamlLoader.Load(this);
    }

    private void LoadUpdater()
    {
        string location = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        string? path = Path.GetDirectoryName(location);
        path = "/Users/mattpannella/pocket-test";
        PocketCoreUpdater updater = new PocketCoreUpdater(path);
        SettingsManager settings = new SettingsManager(path);

        updater.DeleteSkippedCores(settings.GetConfig().delete_skipped_cores);
        updater.SetGithubApiKey(settings.GetConfig().github_token);
        updater.DownloadFirmware(settings.GetConfig().download_firmware);
        updater.RenameJotegoCores(settings.GetConfig().fix_jt_names);
        updater.DownloadAssets(settings.GetConfig().download_assets);
        updater.Initialize();
        Globals.Instance.Updater = updater;
        Globals.Instance.SettingsManager = settings;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
