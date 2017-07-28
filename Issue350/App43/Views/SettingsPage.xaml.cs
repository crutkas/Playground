using System.ComponentModel;
using System.Runtime.CompilerServices;

using App43.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Threading.Tasks;

namespace App43.Views
{
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
        // TODO WTS: Setup your privacy web in your Resource File, currently set to https://YourPrivacyUrlGoesHere

        //private bool _isLightThemeEnabled;
        //public bool IsLightThemeEnabled
        //{
        //    get { return _isLightThemeEnabled; }
        //    set { Set(ref _isLightThemeEnabled, value); }
        //}

        private string _appDescription;
        public string AppDescription
        {
            get { return _appDescription; }
            set { Set(ref _appDescription, value); }
        }

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
//            IsLightThemeEnabled = ThemeSelectorService.IsLightThemeEnabled;
            AppDescription = GetAppDescription();
        }

        private string GetAppDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void RadioButton_CheckedAsync(object sender, RoutedEventArgs e)
        {
            ElementTheme theme = ElementTheme.Default;

            if (sender == ThemeLight)
                theme = ElementTheme.Light;
            else if(sender == ThemeDark)
                theme = ElementTheme.Dark;

            await ThemeSelectorService2.SetThemeAsync(theme);
        }
    }
}
