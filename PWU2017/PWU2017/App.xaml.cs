using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PWU2017
{
    sealed partial class App : Application
    {
        public App() { InitializeComponent(); }

        private Frame RootFrame => (Window.Current.Content as Frame)
                                   ?? (Window.Current.Content = new Frame()) as Frame;

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            var jumplist = e.TileId == "App" && !string.IsNullOrEmpty(e.Arguments);
            if (jumplist)
            {
                try
                {
                    var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(e.Arguments);
                    if (RootFrame.Content == null)
                        RootFrame.Navigate(typeof(MainPage), file);
                    else
                        FileReceived?.Invoke(this, file);
                }
                catch (Exception exception)
                {
                    throw;
                }
            }
            else
            {
                RootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            Window.Current.Activate();
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            if (!args.Files.Any())
                return;
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
                FileReceived?.Invoke(this, args.Files.First() as Windows.Storage.StorageFile);
            else
                RootFrame.Navigate(typeof(MainPage), args.Files.First());
            Window.Current.Activate();
        }

        public static event EventHandler<Windows.Storage.StorageFile> FileReceived;
    }
}
