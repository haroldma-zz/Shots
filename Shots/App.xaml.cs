using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Shots.Views;
using Shots.Web.Services.Interface;

namespace Shots
{
    /// <summary>
    ///     Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        /// <summary>
        ///     Initializes the singleton application object.  This is the first line of authored code
        ///     executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        public new static App Current => Application.Current as App;

        public override Task OnInitializeAsync()
        {
            RootFrame.Template = Resources["AdFrameTemplate"] as ControlTemplate;
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            return Task.FromResult(0);
        }

        public override Task OnLaunchedAsync(ILaunchActivatedEventArgs e)
        {
            var service = Kernel.Resolve<IShotsService>();

            // Navigate to default page
            var page = typeof (MainPage);

            if (!service.IsAuthenticated)
                page = typeof (WelcomePage);

            NavigationService.Navigate(page);
            return Task.FromResult(0);
        }
    }
}