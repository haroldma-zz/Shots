using Windows.UI;

namespace Shots.Helpers
{
    // TODO fix this for windows 10
    public static class StatusBarHelper
    {
        //public static StatusBar Instance => DesignMode.DesignModeEnabled ? null : StatusBar.GetForCurrentView();

        public static Color? ForegroundColor
        {
            get { return null; } //Instance?.ForegroundColor; }
            set { } //if (!DesignMode.DesignModeEnabled) Instance.ForegroundColor = value; }
        }

        public static void ShowStatus(string message)
        {
            ShowStatus(message, null);
        }

        public static async void ShowStatus(string message, double? progress)
        {
            /* Instance.ProgressIndicator.ProgressValue = progress;
            Instance.ProgressIndicator.Text = message;
            await Instance.ProgressIndicator.ShowAsync();*/
        }

        public static async void HideStatus()
        {
            //await Instance.ProgressIndicator.HideAsync();
        }
    }
}