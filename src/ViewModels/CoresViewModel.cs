using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using pannella.analoguepocket.gui.Models;

namespace pannella.analoguepocket.gui.ViewModels;

public partial class CoresViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<SimpleCore> cores = new ObservableCollection<SimpleCore>();
    /*
    public ObservableCollection <SimpleCore> Cores {
        get
        {
            switch (CurrentSort)
            {
                case "Platform":
                    return new ObservableCollection<SimpleCore>(cores.OrderBy(x => x.platform).ToList());
                    break;
                case "Category":
                    cores.OrderBy(x => x.category);
                    break;
                case "Identifier":
                    cores.OrderBy(x => x.identifier);
                    break;
            }

            return cores;
        }
        set {
            Cores = value;
        }
    }*/

    [ObservableProperty] private bool loading = true;

    [ObservableProperty] private ObservableCollection<string> sortFields = new ObservableCollection<string>()
        { "Platform", "Category", "Identifier" };

    [ObservableProperty] private string currentSort = "Platform";

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

    private void SortList(string by)
    {
        string m = "a";
    }
}