using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AlphaMobile.Models;
using System.Windows.Input;

namespace AlphaMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemConfigurationPage : CarouselPage
	{
        App app = Application.Current as App;


        public ItemConfigurationPage (OrderedItem item)
		{
            if(item != null)
            {
                app.orderedItem = item;
                InitializeComponent();

                // Convention ordre des pages
                // Size / HOT / Salt / Meat / Sauce
                if(app.orderedItem.Item != null)
                {
                    if (app.orderedItem.Item.HasSize)
                    {
                        StackLayout stack = new StackLayout();
                        TextSelectListView itemSelectList = new TextSelectListView();
                        itemSelectList.TitleLabel = "Quelle taille pour votre" + app.orderedItem.Item.Name;
                        List<MealSize> listMeal = app.orderedItem.Item.AvailableSizes.Select(i => i.MealSize).ToList();
                        List<string> listText = listMeal.ConvertAll<string>(x => x.ToString());
                        itemSelectList.ItemListView = listText;
                        itemSelectList.ItemSelected += OnItemSelectedSize;


                        stack.Children.Add(itemSelectList);
                        this.Children.Add(new ContentPage { Content = stack });
                    }



                    if (app.orderedItem.Item.CanBeHotNotCold)
                    {
                        StackLayout stack = new StackLayout();
                        TextSelectListView itemSelectList = new TextSelectListView();
                        itemSelectList.TitleLabel = "Comment voulez-vous votre" + app.orderedItem.Item.Name;
                        List<string> listText = new List<string> { "Chaud", "Froid" };
                        itemSelectList.ItemListView = listText;
                        itemSelectList.ItemSelected += OnItemSelectedHotCold;


                        stack.Children.Add(itemSelectList);
                        this.Children.Add(new ContentPage { Content = stack });
                    }

                    if (app.orderedItem.Item.CanBeSalt)
                    {
                        StackLayout stack = new StackLayout();
                        TextSelectListView itemSelectList = new TextSelectListView();
                        itemSelectList.TitleLabel = "Voulez-vous de sel sur votre" + app.orderedItem.Item.Name;
                        List<string> listText = new List<string> { "Salé", "Non salé" };
                        itemSelectList.ItemListView = listText;
                        itemSelectList.ItemSelected += OnItemSelectedSalt;


                        stack.Children.Add(itemSelectList);
                        this.Children.Add(new ContentPage { Content = stack });
                    }

                    if (app.orderedItem.Item.CanHaveMeat)
                    {
                        StackLayout stack = new StackLayout();
                        ItemSelectListView itemSelectList = new ItemSelectListView();
                        itemSelectList.TitleLabel = "Sélectionner la viande pour votre " + app.orderedItem.Item.Name;
                        itemSelectList.ItemListView = app.resto.Menu.ItemList.Where(r => r.TypeOfFood == TypeOfFood.Snack).ToList();
                        itemSelectList.ItemSelected += OnItemSelectedMeat;                         

                        stack.Children.Add(itemSelectList);
                        this.Children.Add(new ContentPage { Content = stack });
                    }

                    if (app.orderedItem.Item.CanHaveSauce)
                    {
                        StackLayout stack = new StackLayout();
                        ItemSelectListView itemSelectList = new ItemSelectListView();
                        List<Item> itemList = app.resto.Menu.ItemList.Where(r => r.TypeOfFood == TypeOfFood.Sauce).ToList();
                        itemList.Add(new Item { Name = "Sans Sauce", Description = "" });

                        itemSelectList.TitleLabel = "Sélectionner la sauce pour votre " + app.orderedItem.Item.Name;
                        itemSelectList.ItemListView = itemList;
                        itemSelectList.ItemSelected += OnItemSelectedSauce;

                        stack.Children.Add(itemSelectList);
                        this.Children.Add(new ContentPage { Content = stack });
                    }
                }
            }

        }

        private async void OnItemSelectedSize(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as string;
            IEnumerable<MealSize> possibleSizes = Enum.GetValues(typeof(MealSize)).Cast<MealSize>();
            foreach(var size in possibleSizes)
            {
                if (size.ToString() == item)
                    app.orderedItem.SelectedSize = size;
            }
            await MoveToNextPage();
        }

        private async void OnItemSelectedHotCold(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as string;
            if (item == "Chaud")
                app.orderedItem.SelectedHotNotCold = true;
            else
                app.orderedItem.SelectedHotNotCold = false;
            await MoveToNextPage();
        }

        private async void OnItemSelectedSalt(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as string;
            if (item == "Salé")
                app.orderedItem.SelectedSalt = true;
            else
                app.orderedItem.SelectedSalt = false;
            await MoveToNextPage();
        }

        private async void OnItemSelectedMeat(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Item;
            if (item != null)
            {
                app.orderedItem.SelectedMeatId = item.ItemId;
                await MoveToNextPage();
            }
        }

        private async void OnItemSelectedSauce(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Item;
            if (item != null)
            {
                if(item.Name != "Sans Sauce")
                {
                    app.orderedItem.SelectedSauceId = item.ItemId;
                }
                await MoveToNextPage();
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();


        }

        private async Task MoveToNextPage()
        {
            int index = this.Children.IndexOf(CurrentPage);
            if(index >= (this.Children.Count()-1))
            {
                await Navigation.PopAsync();
                app.orderedItem.HasBeenConfigured = true;
            }else
            {
                ContentPage nextPage = this.Children[index + 1];
                CurrentPage = nextPage;
            }
        }
    }
}