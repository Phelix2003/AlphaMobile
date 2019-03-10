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
	public partial class SelectUserProfilePage : ContentPage
	{
		public SelectUserProfilePage ()
		{
			InitializeComponent ();
		}

        private async void Customer_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WelcomeCustomerWizzardPages());
        }

        private async void Chef_Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WelcomeChefWizzardPages());               

        }
    }
}