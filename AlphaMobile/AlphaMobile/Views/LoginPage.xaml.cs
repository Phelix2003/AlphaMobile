using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using AlphaMobile.Configuration;
using Newtonsoft.Json;
using AlphaMobile.Models.APIModels;
using AlphaMobile;
using AlphaMobile.Controllers.API;
using AlphaMobile.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Connectivity;

namespace AlphaMobile

{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        private CloudController _cloudController = new CloudController();
        App app = Application.Current as App;


        public LoginPage ()
		{  
			InitializeComponent ();
            BindingContext = Application.Current;
        }

        private  void Button_Clicked(object sender, EventArgs e)
        {
            IsConnected();
        }

        protected override  void OnAppearing()
        {
            base.OnAppearing();
            IsConnected();
        }

        private async void IsConnected()
        {
            // Check if internet connexion is available
            while (!CrossConnectivity.Current.IsConnected)
                await DisplayAlert("Connection", "Oups, il y a de la friture sur la ligne. Vérifier votre connexion internet", "Ok");

            if (await _cloudController.UpdateOAuthToken())
            {
                await app.SavePropertiesAsync();
                app.MainPage = new NavigationPage(new RestaurantListPage());
                await Navigation.PopToRootAsync();
            }
            else
            {                
                await DisplayAlert("Connexion", "Login / PW incorrecte", "Ok");
            }
        }
    }
}