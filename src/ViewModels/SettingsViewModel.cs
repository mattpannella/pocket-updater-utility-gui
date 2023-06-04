using CommunityToolkit.Mvvm.ComponentModel;

namespace pannella.analoguepocket.gui.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool downloadAssets;
    [ObservableProperty]
    private string archiveName;
    [ObservableProperty]
    private string githubToken;
    [ObservableProperty]
    private bool downloadFirmware;
    [ObservableProperty]
    private bool preservePlatforms;
    [ObservableProperty]
    private bool deleteSkippedCores;
    [ObservableProperty]
    private bool buildInstanceJSONs;
    [ObservableProperty]
    private bool crcCheck;
    [ObservableProperty]
    private bool fixJTNames;
    [ObservableProperty]
    private bool skipAltAssets;
    [ObservableProperty]
    private bool useCustomArchive;
    
    [ObservableProperty]
    private bool loaded = false;

    public SettingsViewModel()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        var settings = Globals.Instance.SettingsManager;
        var config = settings.GetConfig();
        PreservePlatforms = config.preserve_platforms_folder;
        DownloadAssets = config.download_assets;
        FixJTNames = config.fix_jt_names;
        ArchiveName = config.archive_name;
        CrcCheck = config.crc_check;
        DownloadFirmware = config.download_firmware;
        GithubToken = config.github_token;
        DeleteSkippedCores = config.delete_skipped_cores;
        SkipAltAssets = config.skip_alternative_assets;
        UseCustomArchive = config.use_custom_archive;
        BuildInstanceJSONs = config.build_instance_jsons;
        Loaded = true;
    }

    public void SaveClick()
    {
        SaveSettings();
    }

    private void SaveSettings()
    {
        Loaded = false;
        Globals.Instance.SettingsManager.GetConfig().preserve_platforms_folder = PreservePlatforms;
        Globals.Instance.SettingsManager.GetConfig().download_assets = DownloadAssets;
        Globals.Instance.SettingsManager.GetConfig().fix_jt_names = FixJTNames;
        Globals.Instance.SettingsManager.GetConfig().archive_name = ArchiveName;
        Globals.Instance.SettingsManager.GetConfig().crc_check = CrcCheck;
        Globals.Instance.SettingsManager.GetConfig().download_firmware = DownloadFirmware;
        Globals.Instance.SettingsManager.GetConfig().github_token = GithubToken;
        Globals.Instance.SettingsManager.GetConfig().delete_skipped_cores = DeleteSkippedCores;
        Globals.Instance.SettingsManager.GetConfig().skip_alternative_assets = SkipAltAssets;
        Globals.Instance.SettingsManager.GetConfig().use_custom_archive = UseCustomArchive;
        Globals.Instance.SettingsManager.GetConfig().build_instance_jsons = BuildInstanceJSONs;

        Globals.Instance.SettingsManager.SaveSettings();

        RefreshUpdaterSettings();
        
        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow("Congrats", "Settings Saved.");
        messageBoxStandardWindow.Show();
        Loaded = true;
    }

    private void RefreshUpdaterSettings()
    {
        var settings = Globals.Instance.SettingsManager;
        Globals.Instance.Updater.DeleteSkippedCores(settings.GetConfig().delete_skipped_cores);
        Globals.Instance.Updater.SetGithubApiKey(settings.GetConfig().github_token);
        Globals.Instance.Updater.DownloadFirmware(settings.GetConfig().download_firmware);
        Globals.Instance.Updater.DownloadAssets(settings.GetConfig().download_assets);
        Globals.Instance.Updater.RenameJotegoCores(settings.GetConfig().fix_jt_names);
        Globals.Instance.Updater.LoadSettings();
    }
}