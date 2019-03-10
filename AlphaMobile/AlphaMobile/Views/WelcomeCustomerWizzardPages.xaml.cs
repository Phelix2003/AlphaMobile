using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace AlphaMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WelcomeCustomerWizzardPages : ContentPage
	{
        App app = Application.Current as App;

        public WelcomeCustomerWizzardPages ()
		{
			InitializeComponent ();          
           
		}

        private async void Wizzard_Button_Clicked(object sender, EventArgs e)
        {
            app.WelcomeCustomerWizzardDone = true;
            if(app.UserProfile == "Chef")
            {
                app.MainPage = new NavigationPage(new SelectUserProfilePage());

            }
            else
            {
                app.MainPage = new NavigationPage(new RestaurantListPage());
            }

            await Navigation.PushAsync(new RestaurantListPage());
        }
    }
}