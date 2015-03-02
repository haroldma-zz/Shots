using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight.Threading;
using Lumia.Imaging;
using Lumia.Imaging.Adjustments;

namespace Shots.Utilities
{
    public class BlurImageTool
    {
        public static string GetSource(DependencyObject element)
        {
            return (string)element.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject element, string value)
        {
            element.SetValue(SourceProperty, value);
        }

        private static async void SourceCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var url = e.NewValue as string;
            var image = d as Image;
            var imageBrush = d as ImageBrush;
            var bounds = Window.Current.Bounds;

            if (string.IsNullOrEmpty(url) || (image == null && imageBrush == null)) return;

            if (image != null) image.Source = null;
            else imageBrush.ImageSource = null;

            // Download the image
            using (var client = new HttpClient())
            {
                using (var resp = await client.GetAsync(url).ConfigureAwait(false))
                {
                    if (!resp.IsSuccessStatusCode) return;

                    using (var stream = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        using (var rnd = stream.AsRandomAccessStream())
                        {
                            using (var source = new RandomAccessStreamImageSource(rnd, ImageFormat.Jpeg))
                            {
                                // Create effect collection with the source stream
                                using (var filters = new FilterEffect(source))
                                {
                                    // Initialize the filter and add the filter to the FilterEffect collection
                                    filters.Filters = new IFilter[] {new BlurFilter(256)};

                                    // Create a target where the filtered image will be rendered to
                                    WriteableBitmap target = null;

                                    // Need to switch to UI thread for this
                                    await DispatcherHelper.RunAsync(
                                            () => target = new WriteableBitmap((int) (bounds.Width),
                                                (int) (bounds.Height))).AsTask().ConfigureAwait(false);

                                    // Create a new renderer which outputs WriteableBitmaps
                                    using (var renderer = new WriteableBitmapRenderer(filters, target))
                                    {
                                        // Render the image with the filter(s)
                                        await renderer.RenderAsync().AsTask().ConfigureAwait(false);

                                        // Set the output image to Image control as a source
                                        // Need to switch to UI thread for this
                                        await DispatcherHelper.RunAsync(() =>
                                        {
                                            if (image != null) image.Source = target;
                                            else if (imageBrush != null) imageBrush.ImageSource = target;
                                        }).AsTask().ConfigureAwait(false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached(
            "Source",
            typeof(string),
            typeof(BlurImageTool),
            new PropertyMetadata(string.Empty, SourceCallback));
    }
}
