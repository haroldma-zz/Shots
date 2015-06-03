using System;
using Windows.UI.ViewManagement;

namespace Shots.Helpers
{
    public static class StatusBarHelper
    {
        public static StatusBar Instance => StatusBar.GetForCurrentView();

        public static void ShowStatus(string message)
        {
            ShowStatus(message, null);
        }

        public static async void ShowStatus(string message, double? progress)
        {
            Instance.ProgressIndicator.ProgressValue = progress;
            Instance.ProgressIndicator.Text = message;
            await Instance.ProgressIndicator.ShowAsync();
        }
        
        

        public static async void HideStatus()
        {
            await Instance.ProgressIndicator.HideAsync();
        }
    }
}
