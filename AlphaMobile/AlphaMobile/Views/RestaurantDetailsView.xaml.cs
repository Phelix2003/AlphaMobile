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
            await ProcessTheItem();
        }

        
        private List<ItemGroup> GenerateItemGroupList(Restaurant resto)
        {        
            if(app.order != null)
            {
                if(app.order.OrderedItems != null)
                {
                    foreach(var item in resto.Menu.ItemList)
                    {
                        item.UpdateDisplayOrderedQuanity(app.order.OrderedItems.ToList());
                    }
                }
            }
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

        private async void ListView_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            //var item = e.SelectedItem as Item;
            var item = e.Item as Item;
            app.orderedItem = new OrderedItem { Item = item };
            await ProcessTheItem();
            ((ListView)sender).SelectedItem = null;
        }

        private async Task ProcessTheItem()
        {
            // State machine to process a new Item to be add in the order. 
            // This is to refresh the order in case of someone is also changing the order on the desktop version
            app.order = await _Cloud.GetCustomerOrderAsync();
            if(app.orderedItem != null)
            {
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
                            if (app.order.OrderRestaurantId != _restoId)
                            {
                                // The resto used in the existing order is different from the one we 
                                // want to use here.

                                // Some actions here.   
                                return;
                            }
                        }
                    }

                    // First Check is there is a slot time associated to this order.
                    if (app.order.OrderSlot == null)
                    {
                        // Get the available slot tome for today. 
                        await Navigation.PushAsync(new SlotTimeSelectionPage(app.resto.Id));
                    }
                    else
                    {
                        // Then check if this Item need to open the configuration page
                        if (app.orderedItem.Item.CanBeHotNotCold == true ||
                            app.orderedItem.Item.CanBeSalt == true ||
                            app.orderedItem.Item.CanHaveMeat == true ||
                            app.orderedItem.Item.CanHaveSauce == true ||
                            app.orderedItem.Item.HasSize == true)
                        {
                            if (!app.orderedItem.HasBeenConfigured)
                            {
                                await Navigation.PushAsync(new ItemConfigurationPage(app.orderedItem));
                            }
                            else
                            {
                                // Add Item in the cloud 
                                bool response = await _Cloud.AddItemToCustomerOrder(new OrderedItem
                                {
                                    ItemId = app.orderedItem.Item.ItemId,
                                    Quantity = 1,
                                    SelectedHotNotCold = app.orderedItem.SelectedHotNotCold,
                                    SelectedMeatId = app.orderedItem.SelectedMeatId,
                                    SelectedSalt = app.orderedItem.SelectedSalt,
                                    SelectedSauceId = app.orderedItem.SelectedSauceId,
                                    SelectedSize = app.orderedItem.SelectedSize
                                });
                                app.orderedItem = null;
                            }
                        }
                        else
                        {                               
                            // Add Item in the cloud 
                            bool response = await _Cloud.AddItemToCustomerOrder(new OrderedItem
                            {
                                ItemId = app.orderedItem.Item.ItemId,
                                Quantity = 1,
                                SelectedHotNotCold = app.orderedItem.SelectedHotNotCold,
                                SelectedMeatId = app.orderedItem.SelectedMeatId,
                                SelectedSalt = app.orderedItem.SelectedSalt,
                                SelectedSauceId = app.orderedItem.SelectedSauceId,
                                SelectedSize = app.orderedItem.SelectedSize
                            });
                        }
                    }
                }
                else
                {
                    // Could not get the order.
                    // Error No order for this user. 
                    await Navigation.PopAsync();
                }
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderSummary());
        }
    }
}