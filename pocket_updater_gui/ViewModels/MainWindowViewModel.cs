using DynamicData;
using ReactiveUI;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace pannella.analoguepocket.gui.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            // Set current page to first on start up
            _CurrentPage = Pages[0];
            
            //NavigateNextCommand = ReactiveCommand.Create(NavigateNext, true);
            //NavigatePreviousCommand = ReactiveCommand.Create(NavigatePrevious, true);
        }

        // A read.only array of possible pages
        private readonly ObservableObject[] Pages = 
        { 
            new HomeViewModel(),
            new SettingsViewModel()
        };

        // The default is the first page
        private ObservableObject _CurrentPage;

        /// <summary>
        /// Gets the current page. The property is read-only
        /// </summary>
        public ObservableObject CurrentPage
        {
            get { return _CurrentPage; } 
            set { this.RaiseAndSetIfChanged(ref _CurrentPage, value); }
        }

        public void GoToHome()
        {
            CurrentPage = Pages[0];
        }

        public void GoToSettings()
        {
            CurrentPage = Pages[1];
        }
    }
}