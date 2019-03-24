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

        private int _restoId;
        private Item _item;



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
                app.resto = await UpdtaeRestoFromCloud(_restoId);
                if (app.resto == null && app.OAuth_Token == "")
                    await Navigation.PushModalAsync(new LoginPage());
            } while (app.OAuth_Token == "");

            if (app.resto != null)
            {
                var imageSource = new UriImageSource { Uri = new Uri("https://alpha-easio.azurewebsites.net/restaurant/RenderRestoPhoto?RestoId=" + app.resto.Id), CachingEnabled = true };
                RestoImage.Source = imageSource;
                RestoName.Text = app.resto.Name;
                if (app.resto.Menu != null)
                {
                    listView.ItemsSource = GenerateItemGroupList(app.resto);
                }
            }else
            {
                // Error during the get of the restaurant.
                await DisplayAlert("Erreur", "Impossible de charger le restaurant avec l'ID" + _restoId, "Ok");
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
            ItemGroup groupeMeal = new ItemGroup("Préparations", "P");
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
            await ProcessTheItem(item);
        }

        private async Task ProcessTheItem(Item item)
        {
            // This is to refresh the order in case of someone is also changing the order on the desktop version
            app.order = await _Cloud.GetCustomerOrderAsync();

            if (app.order != null)
            {
                // Check if the order is already completed or not. 
                if (app.order.IsOrderCompleted)
                {
                    //Action to take if the order is already completed. 
                    return;
                }

                // Check if the order has already been started
                // 

                if (app.order.OrderedItems != null)
                {
                    if (app.order.OrderedItems.Count() > 0)
                    {
                        //Order has already been started before/
                        // Check now if the restaurant used for this order is the same has
                        // the one requested here. 
                        if (app.order.OrderRestaurantId != _restoId)                        {
                            // The resto used in the existing order is different from the one we 
                            // want to use here.

                            // Some actions here.   
                            return;
                        }
                    }
                }

                // Check is there is a slot time associated to this order.
                if (app.order.OrderSlot == null)
                {
                    // Get the available slot tome for today. 
                    await Navigation.PushAsync(new SlotTimeSelectionPage(app.resto.Id));
                }
                else
                {
                    if(app.resto != null)
                    {
                        await Navigation.PushAsync(new ItemConfigurationPage(new OrderedItem { Item = item }));
                    }

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
                // Could not get the order.
                // Error No order for this user. 
                await Navigation.PopAsync();
            }

        }
    }
}