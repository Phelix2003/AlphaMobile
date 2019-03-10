using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AlphaMobile.Controllers.API;
using AlphaMobile.Models;
using Plugin.Connectivity;

namespace AlphaMobile.Views
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestaurantListPage : ContentPage
	{
        private CloudController _Cloud = new CloudController();

        public RestaurantListPage()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            while (!CrossConnectivity.Current.IsConnected)
                await DisplayAlert("Connection", "Oups, il y a de la friture sur la ligne. Vérifier votre connexion internet", "Ok");
            List<Restaurant> restaurants = await _Cloud.GetRestaurantListAsync();
            if(restaurants == null)
            {
                await DisplayAlert("Erreur", "Il semble y avoir une problème de communication", "Ok");
            }
        }
    }
}