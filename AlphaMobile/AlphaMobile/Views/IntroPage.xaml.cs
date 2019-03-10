using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace AlphaMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IntroPage : ContentPage
	{
		public IntroPage ()
		{
			InitializeComponent ();
            while (!CrossConnectivity.Current.IsConnected)
                DisplayAlert("Connection","Oups impossible d'atteindre votre friterie. Vérifier votre connexion internet","Ok");
            
              

		}
	}
}