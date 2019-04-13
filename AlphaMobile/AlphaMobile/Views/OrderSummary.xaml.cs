using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AlphaMobile.Controllers.API;

namespace AlphaMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderSummary : ContentPage
	{
        App app = Application.Current as App;
        private CloudController _Cloud = new CloudController();



        public OrderSummary ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}