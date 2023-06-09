using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using pannella.analoguepocket.gui.Models;

namespace pannella.analoguepocket.gui.ViewModels;

public partial class CoresViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<SimpleCore> cores = new ObservableCollection<SimpleCore>();

    [ObservableProperty] private bool loading = true;
    
    public CoresViewModel()
    {
        LoadCores();
    }
    
    public void SaveClick()
    {
        SaveSettings();
    }

    private void SaveSettings()
    {
        foreach (SimpleCore c in Cores)
        {
            if (c.track)
            {
                Globals.Instance.SettingsManager.EnableCore(c.identifier);
            }
            else
            {
                Globals.Instance.SettingsManager.DisableCore(c.identifier);
            }

            Core core = Globals.Instance.Updater.GetCores().Find(x => x.identifier == c.identifier);
            core.UpdatePlatform(c.platform, c.category);
        }
        
        Globals.Instance.SettingsManager.SaveSettings();
        Globals.Instance.Updater.Initialize();
        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow("You Did It!", "Core choices and platform names saved.");
        messageBoxStandardWindow.Show();
    }
    
    private async Task LoadCores()
    {
        await Globals.Instance.Updater.Initialize();
        List<Core> list = Globals.Instance.Updater.GetCores();
        List<Core> local = await Globals.Instance.Updater.GetLocalCores();
  
        list.AddRange(local);
        list = list.OrderBy(x => x.platform.name).ToList();
        Cores.Clear();
        foreach (Core c in list)
        {
            Platform p = c.ReadPlatformFile();
            var core = new SimpleCore()
            {
                Identifier = c.identifier,
                Track = !Globals.Instance.SettingsManager.GetCoreSettings(c.identifier).skip,
                Platform = p.name,
                Category = p.category,
                Config = c.getConfig(),
                PlatformId = c.platform_id
            };
            core.SetImage();
            Cores.Add(core);
        }

        Loading = false;
    }

    public void All()
    {
        for(int i = 0; i < Cores.Count; i++)
        {
            Cores[i].Track = true;
        }
    }
    
    public void None()
    {
        for(int i = 0; i < Cores.Count; i++)
        {
            Cores[i].Track = false;
        }
    }
}