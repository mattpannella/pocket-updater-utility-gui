using System.IO;
using System.Net.Http;

namespace pannella.analoguepocket.gui;

public class Globals
{
    private static Globals instance = null;
    private static object syncLock = new object();
    public SettingsManager? SettingsManager { get; set; }
    public PocketCoreUpdater? Updater { get; set; }


    private Globals()
    {

    }

    public static Globals Instance
    {
        get
        {
            lock (syncLock)
            {
                if (Globals.instance == null)
                {
                    Globals.instance = new Globals();
                }

                return Globals.instance;
            }
        }
    }
}
