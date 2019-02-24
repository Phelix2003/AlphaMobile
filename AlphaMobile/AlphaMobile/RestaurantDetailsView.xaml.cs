using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AlphaMobile.Controllers.API;
using AlphaMobile.Models;

namespace AlphaMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestaurantDetailsView : ContentPage
	{
        private CloudController _Cloud = new CloudController();

        private Restaurant resto;

        private async Task<Restaurant> UpdtaeRestoFromCloud(int RestoId)
        {
            return await _Cloud.GetRestaurantDetailAsync(RestoId);
        }
        

		public RestaurantDetailsView ()
		{
			InitializeComponent ();

		}
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            resto = await UpdtaeRestoFromCloud(2);
            if (resto != null)
            {
                if (resto.Menu != null)
                {
                    listView.ItemsSource = resto.Menu.ItemList;
                }
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            resto = await UpdtaeRestoFromCloud(2);
            if (resto != null)
            {
                RestoName.Text = resto.Name;
                if (resto.Menu != null)
                {
                    MenuName.Text = resto.Menu.Name;
                    listView.ItemsSource = resto.Menu.ItemList;
                }
            }

        }

        
    }
}