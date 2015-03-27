using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Threading;
using Shots.ViewModel;
using Shots.Views;

namespace Shots
{
    public sealed partial class App
    {
        /// <summary>
        ///     Initializes the singleton application object.  This is the first line of authored code
        ///     executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        public static bool SupressBackEvent { get; set; }
        public static event EventHandler<BackPressedEventArgs> SupressedBackEvent;

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (SupressBackEvent)
            {
                e.Handled = true;
                if (SupressedBackEvent != null)
                    SupressedBackEvent(sender, e);

                return;
            }

            if (!RootFrame.CanGoBack) return;
            e.Handled = true;
            RootFrame.GoBack();
        }

        public static ViewModelLocator Locator
        {
            get { return _locator ?? (_locator = (ViewModelLocator) Current.Resources["Locator"]); }
        }

        /// <summary>
        ///     Invoked when the application is launched normally by the end user.  Other entry points
        ///     will be used when the application is launched to open a specific file, to display
        ///     search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            RootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                RootFrame = new Frame();

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = RootFrame;
            }

            if (RootFrame.Content == null)
            {
                DispatcherHelper.Initialize();
                // Removes the turnstile navigation for startup.
                if (RootFrame.ContentTransitions != null)
                {
                    _transitions = new TransitionCollection();
                    foreach (var c in RootFrame.ContentTransitions)
                        _transitions.Add(c);
                }

                RootFrame.ContentTransitions = null;
                RootFrame.Navigated += RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (
                    !RootFrame.Navigate(
                        (Locator.ShotsService.IsAuthenticated ? typeof (HomePage) : typeof (WelcomePage)), e.Arguments))
                    throw new Exception("Failed to create initial page");
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        public static Frame RootFrame { get; set; }

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        ///     Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            RootFrame.ContentTransitions = _transitions ?? new TransitionCollection {new NavigationThemeTransition()};
            RootFrame.Navigated -= RootFrame_FirstNavigated;
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
        }

        #region Fields and Constants

        private TransitionCollection _transitions;
        private static ViewModelLocator _locator;

        #endregion
    }
}