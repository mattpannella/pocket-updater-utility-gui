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
            RandomHeader();
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

        private string _HeaderText;
        
        public string HeaderText
        {
            get { return _HeaderText; } 
            set { this.RaiseAndSetIfChanged(ref _HeaderText, value); }
        }

        public void GoToHome()
        {
            CurrentPage = Pages[0];
        }

        public void GoToSettings()
        {
            CurrentPage = Pages[1];
        }

        private void RandomHeader()
        {
            string[] messages = new[]
            {
                "Blame yourself or God",
                "Welcome to Flavortown",
                "She finds you crusty, Dave",
                "This is a fancy restaurant",
                "Welcome to the Black Market!",
                "Itchy. Tasty.",
                "What're ya buyin?",
                "What is a man?",
                "Fission Mailed",
                "Umbasa",
                "Let's mosey"
            };
            
            Random random = new Random();
            int i = random.Next(0, messages.Length);
            HeaderText = messages[i];
        }
    }
}