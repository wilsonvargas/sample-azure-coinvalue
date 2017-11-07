using CoinClient.Services;
using CoinClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoinClient
{
	public partial class MainPage : ContentPage
	{
        MainViewModel vm;
		public MainPage()
		{
			InitializeComponent();
            BindingContext = vm = new MainViewModel(DependencyService.Get<ICoinService>());

            vm.Entries.CollectionChanged += Entries_CollectionChanged;
            chart.Entries = vm.Entries.ToList();    
        }


        private void Entries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                chart.Entries = vm.Entries.ToList();
                chartView.InvalidateSurface();
            });
        }   
        

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.RefreshCommand.Execute(null);
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                vm.RefreshCommand.Execute(null);
                return true;
            });
        }
    }
}
