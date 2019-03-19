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
using AlphaMobile.Models.APIModels;

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
            // TOdo optimize // correct the login methodes. 
            do
            {
                app.order = await _Cloud.GetCustomerOrderAsync();
                _resto = await UpdtaeRestoFromCloud(_restoId);
                if (_resto == null && app.OAuth_Token == "")
                    await Navigation.PushModalAsync(new LoginPage());
            } while (app.OAuth_Token == "");

            var imageSource = new UriImageSource { Uri = new Uri("https://alpha-easio.azurewebsites.net/restaurant/RenderRestoPhoto?RestoId=" + _resto.Id), CachingEnabled = true};
            RestoImage.Source = imageSource;
            if (_resto != null)
            {
                RestoName.Text = _resto.Name;
                if (_resto.Menu != null)
                {
                    listView.ItemsSource = GenerateItemGroupList(_resto);
                }
            }else
            {

            }
        }

        
        private List<ItemGroup> GenerateItemGroupList(Restaurant resto)
        {            
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

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Item;

            // This is to refresh the order in case of someone is also changing the order on the desktop version
            app.order = await _Cloud.GetCustomerOrderAsync();

            if(app.order != null)
            {
                // Check is there is a slot tima associated to this order.
                if (app.order.OrderSlot == null)
                {
                    // Get the available slot tome for today. 
                    await Navigation.PushAsync(new SlotTimeSelectionPage(_resto.Id));
                }
                else
                {
                    await Navigation.PushAsync(new ItemConfigurationPage());
                }

                bool response = await _Cloud.AddItemToCustomerOrder(new OrderedItemAPIModel
                {
                    ItemId = item.ItemId,
                    Quantity = 1,
                    SelectedHotNotCold = true,
                    SelectedMeatId = null,
                    SelectedSalt = true,
                    SelectedSauceId = null,
                    SelectedSize = MealSize.L
                });

            }
            else
            {
                // Error No order for this user. 
                await Navigation.PopAsync();
            }


        }
    }
}