using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AlphaMobile.Controllers.API;
using AlphaMobile.Models;
using AlphaMobile.ModelViews;

namespace AlphaMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestaurantDetailsView : ContentPage
	{
        App app = Application.Current as App;

        private CloudController _Cloud = new CloudController();

        private Restaurant _resto;
        private int _restoId;


        private async Task<Restaurant> UpdtaeRestoFromCloud(int RestoId)
        {

            return await _Cloud.GetRestaurantDetailAsync(RestoId);
        }
        

		public RestaurantDetailsView (int RestoId)
		{
			InitializeComponent ();
            _restoId = RestoId;

		}
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            do
            {
                _resto = await UpdtaeRestoFromCloud(_restoId);
                if (_resto == null && app.OAuth_Token == "")
                    await Navigation.PushModalAsync(new LoginPage());
            } while (app.OAuth_Token == "");
            
            if (_resto != null)
            {
                RestoName.Text = _resto.Name;
                if (_resto.Menu != null)
                {
                    MenuName.Text = _resto.Menu.Name;
                    listView.ItemsSource = GenerateItemGroupList(_resto);
                }
            }else
            {

            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
            _resto = await UpdtaeRestoFromCloud(2);
            if (_resto != null)
            {
               
                RestoName.Text = _resto.Name;
                if (_resto.Menu != null)
                {
                    MenuName.Text = _resto.Menu.Name;
                    listView.ItemsSource = GenerateItemGroupList(_resto);
                }
            }
        }
        
        private List<ItemGroup> GenerateItemGroupList(Restaurant resto)
        {
            MenuName.Text = resto.Menu.Name;
            IEnumerable<Item> ListItem = resto.Menu.ItemList.Where(s => s.TypeOfFood == TypeOfFood.Frites).ToList();
            ItemGroup groupeFrites = new ItemGroup("Frites", "F");
            groupeFrites.AddRange(ListItem);
            ListItem = resto.Menu.ItemList.Where(s => s.TypeOfFood == TypeOfFood.Sauce).ToList();
            ItemGroup groupeSauces = new ItemGroup("Sauces", "S");
            groupeSauces.AddRange(ListItem);
            ListItem = resto.Menu.ItemList.Where(s => s.TypeOfFood == TypeOfFood.Snack).ToList();
            ItemGroup groupeSnack = new ItemGroup("Snack", "S");
            groupeSnack.AddRange(ListItem);
            ListItem = resto.Menu.ItemList.Where(s => s.TypeOfFood == TypeOfFood.Boisson).ToList();
            ItemGroup groupeBoissons = new ItemGroup("Boissons", "B");
            groupeBoissons.AddRange(ListItem);
            ListItem = resto.Menu.ItemList.Where(s => s.TypeOfFood == TypeOfFood.Meal).ToList();
            ItemGroup groupeMeal = new ItemGroup("Préparations", "M");
            groupeMeal.AddRange(ListItem);
            ListItem = resto.Menu.ItemList.Where(s => s.TypeOfFood == TypeOfFood.Menu).ToList();
            ItemGroup groupeMenu = new ItemGroup("Menu", "M");
            groupeMenu.AddRange(ListItem);

            return new List<ItemGroup>
                    {
                        groupeFrites,
                        groupeSnack,
                        groupeMeal,
                        groupeMenu,
                        groupeBoissons,
                        groupeSauces
                    };
        }
    }
}