using Android.App;
using Android.OS;
using GalaSoft.MvvmLight.Helpers;
using CoinClient.ViewModel;
using System.Collections.Generic;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Microsoft.Azure.Mobile.Push;
using System;

namespace CoinClient
{
    [Activity(Label = "Bitcoin watcher", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity
    {
        private MainViewModel Vm => App.Locator.Main;
        private List<Binding> _bindings = new List<Binding>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // This should come before MobileCenter.Start() is called
            Push.PushNotificationReceived += HandlePushNotificationReceived;

            // Set the Azure Mobile Center up
            MobileCenter.Start(
                "b02bf21a-e359-4e48-9442-394952ea67b2", 
                typeof(Analytics), 
                typeof(Crashes),
                typeof(Push));

            _bindings.Add(this.SetBinding(
                () => Vm.CurrentCoinValue,
                () => ValueLabel.Text));

            _bindings.Add(this.SetBinding(
                () => Vm.IsUpTrendVisible)
                .WhenSourceChanges(() =>
                {
                    if (Vm.IsUpTrendVisible)
                    {
                        ArrowImage.SetImageResource(Resource.Drawable.ArrowUp);
                    }
                }));

            _bindings.Add(this.SetBinding(
                () => Vm.IsFlatTrendVisible)
                .WhenSourceChanges(() =>
                {
                    if (Vm.IsFlatTrendVisible)
                    {
                        ArrowImage.SetImageResource(Resource.Drawable.ArrowFlat);
                    }
                }));

            _bindings.Add(this.SetBinding(
                () => Vm.IsDownTrendVisible)
                .WhenSourceChanges(() =>
                {
                    if (Vm.IsDownTrendVisible)
                    {
                        ArrowImage.SetImageResource(Resource.Drawable.ArrowDown);
                    }
                }));

            RefreshButton.SetCommand(App.Locator.Main.RefreshCommand);
        }

        private void HandlePushNotificationReceived(object sender, PushNotificationReceivedEventArgs e)
        {
            // Add the notification message and title to the message
            var summary = $"Push notification received:" +
                                $"\n\tNotification title: {e.Title}" +
                                $"\n\tMessage: {e.Message}";

            // If there is custom data associated with the notification,
            // print the entries
            if (e.CustomData != null)
            {
                summary += "\n\tCustom data:\n";
                foreach (var key in e.CustomData.Keys)
                {
                    summary += $"\t\t{key} : {e.CustomData[key]}\n";
                }
            }

            // Send the notification summary to debug output
            System.Diagnostics.Debug.WriteLine(summary);
        }
    }
}

