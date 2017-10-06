using CoinClient.Services;
using CoinClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Plugin.TextToSpeech;
using SkiaSharp;

namespace CoinClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        double currentCoinValue = 3000;
        bool isBusy;
        bool isDownTrendVisible;
        bool isFlatTrendVisible = true;
        bool isUpTrendVisible;
        string errorMessage;
        ICoinService service;

        public MainViewModel(ICoinService service)
        {
            this.service = service;
            RefreshCommand = new Command(async () => await Refresh());
        }

        public ObservableCollection<CoinTrend> CoinTrends { get; set; } = new ObservableCollection<CoinTrend>();

        public ObservableCollection<Microcharts.Entry> Entries { get; set; } = new ObservableCollection<Microcharts.Entry>();

        public double CurrentCoinValue
        {
            get => currentCoinValue;
            set => SetProperty(ref currentCoinValue, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value))
                    RefreshCommand.ChangeCanExecute();
            }
        }



        public bool IsDown
        {
            get => isDownTrendVisible;
            set => SetProperty(ref isDownTrendVisible, value);
        }

        public bool IsFlat 
        {
            get => isFlatTrendVisible;
            set => SetProperty(ref isFlatTrendVisible, value);
        }

        public bool IsUp
        {
            get => isUpTrendVisible;
            set => SetProperty(ref isUpTrendVisible, value);
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public Command RefreshCommand
        {
            get;
            set;
        }

        async Task Refresh()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {

                if (!CrossConnectivity.Current.IsConnected) {
                    return;
                }

                var trend = await service.GetTrend();
                CoinTrends.Add(trend);

                CurrentCoinValue = trend.CurrentValue;

                IsUp = trend.Trend > 0;
                IsFlat = trend.Trend == 0;
                IsDown = trend.Trend < 0;

                var color = IsUp ? SKColor.Parse("#77d065") : (IsFlat ? SKColor.Parse("#3498db") : SKColor.Parse("#ff0000"));

                Entries.Add(new Microcharts.Entry((float)trend.CurrentValue) {

                    Color = color,
                    TextColor = color,
                    ValueLabel = trend.CurrentValue.ToString("N4")

                });

                ErrorMessage = string.Empty;
                var text = IsUp ? "Is Up" : (IsDown ? "Is Down" : "Is Flat");
                await CrossTextToSpeech.Current.Speak(text);
            }
            catch (Exception ex)
            {
                CurrentCoinValue = 0;
                IsUp = false;
                IsFlat = true;
                IsDown = false;
                ErrorMessage = ex.Message;
            }
            IsBusy = false;
        }


        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>
        /// <param name="backingStore">Backing store.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="onChanged">On changed.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
