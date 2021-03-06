﻿using System;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WeatherWizz.Common;
using Windows.Storage;
using WeatherWizz.DataModel;
using System.Collections.ObjectModel;
using System.Net;
using Windows.UI.Popups;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace WeatherWizz
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        public static ApplicationViewModel ApplicationViewModel
        { get; private set; }

        /// <summary>
        /// Initializes the singleton instance of the <see cref="App"/> class. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            this.EnsureSettings();

            this.UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.HandleException(e);
        }

        private async void HandleException(UnhandledExceptionEventArgs e)
        {
            if(e.Exception is WebException)
            {
                MessageDialog dialog = new MessageDialog("Service unavailable", "Error");
                await dialog.ShowAsync();
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // Something went wrong restoring state.
                        // Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(HubPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }


            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void EnsureSettings()
        {
            if (ApplicationData.Current.LocalSettings.Values["SavedCities"] == null)
            {
                List<string> defaultCities = WeatherDataServiceConsumer.GetDefaultCities();

#if WINDOWS_PHONE_APP
                defaultCities.Insert(0, "CurrentLocation");
#endif

                ApplicationData.Current.LocalSettings.Values["SavedCities"] = defaultCities.ToArray();
            }

            if (ApplicationData.Current.LocalSettings.Values["Units"] == null)
            {
                ApplicationData.Current.LocalSettings.Values["Units"] = "Metric";
            }

            if (ApplicationData.Current.LocalSettings.Values["SelectedLocation"] == null)
            {
#if WINDOWS_PHONE_APP
                ApplicationData.Current.LocalSettings.Values["SelectedLocation"] = "CurrentLocation";
#else
                ApplicationData.Current.LocalSettings.Values["SelectedLocation"] = "Sofia";
#endif
            }

            ApplicationViewModel = new ApplicationViewModel();
            ApplicationViewModel.SavedCities = new ObservableCollection<string>(ApplicationData.Current.LocalSettings.Values["SavedCities"] as string[]);
#if WINDOWS_PHONE_APP
            if (!ApplicationViewModel.SavedCities.Contains("CurrentLocation"))
            {
                ApplicationViewModel.SavedCities.Insert(0, "CurrentLocation");
            }
            else
            {
                ApplicationViewModel.SavedCities.Remove("CurrentLocation");
                ApplicationViewModel.SavedCities.Insert(0, "CurrentLocation");
            }
#endif


            ApplicationViewModel.MeasurementUnits = ApplicationData.Current.LocalSettings.Values["Units"].Equals("Metric") ?
                MeasurementUnits.Metric : MeasurementUnits.Imperial;

            ApplicationViewModel.SelectedLocation = (string)ApplicationData.Current.LocalSettings.Values["SelectedLocation"];
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        private void SaveSettings()
        {
            ApplicationData.Current.LocalSettings.Values["SavedCities"] = ApplicationViewModel.SavedCities.ToArray<string>();

            ApplicationData.Current.LocalSettings.Values["Units"] = ApplicationViewModel.MeasurementUnits.ToString();

            ApplicationData.Current.LocalSettings.Values["SelectedLocation"] = ApplicationViewModel.SelectedLocation;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            this.SaveSettings();

            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}