using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ICommand ListViewClickAction { get; set; };

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
                    if (app.orderedItem.Item.CanBeHotNotCold)
                    {
                        StackLayout stack = new StackLayout();
                        ItemSelectButtonView itemSelectButton = new ItemSelectButtonView();
                        itemSelectButton.LeftButtonText = "Chaud";
                        itemSelectButton.RightButtonText = "Froid";

                        itemSelectButton.LeftButtonClicked += async delegate
                        {
                            app.orderedItem.SelectedHotNotCold = true;
                            await MoveToNextPage();
                        };

                        itemSelectButton.RightButtonClicked += async delegate
                        {
                            app.orderedItem.SelectedHotNotCold = false;
                            await MoveToNextPage();
                        };
                        stack.Children.Add(itemSelectButton);
                        this.Children.Add(new ContentPage { Content = stack });
                    }

                    if (app.orderedItem.Item.CanBeSalt)
                    {
                        StackLayout stack = new StackLayout();
                        ItemSelectButtonView itemSelectButton = new ItemSelectButtonView();
                        itemSelectButton.LeftButtonText = "Avec Sel";
                        itemSelectButton.RightButtonText = "Sans Sel";

                        itemSelectButton.LeftButtonClicked += async delegate
                        {
                            app.orderedItem.SelectedSalt = true;
                            await MoveToNextPage();
                        };

                        itemSelectButton.RightButtonClicked += async delegate
                        {
                            app.orderedItem.SelectedSalt = false;
                            await MoveToNextPage();
                        };
                        stack.Children.Add(itemSelectButton);
                        this.Children.Add(new ContentPage { Content = stack });
                    }

                    if (app.orderedItem.Item.CanHaveMeat)
                    {
                        StackLayout stack = new StackLayout();
                        ItemSelectListView itemSelectList = new ItemSelectListView();
                        itemSelectList.ItemListView = app.resto.Menu.ItemList.Where(r=>r.TypeOfFood == TypeOfFood.Snack).ToList();
                        itemSelectList.TitleLabel = "Quel viande pour " + app.orderedItem.Item.Name;

                        ListViewClickAction = new Command();
                        itemSelectList.ItemClickCommand += async delegate
                        {

                        }
                        //itemSelectButton.RightButtonClicked += async delegate
                        //{
                        //    app.orderedItem.SelectedSalt = false;
                        //    await MoveToNextPage();
                        //};
                        stack.Children.Add(itemSelectList);
                        this.Children.Add(new ContentPage { Content = stack });
                    }

                    if (app.orderedItem.Item.CanHaveSauce)
                    {
                        StackLayout stack = new StackLayout();
                        stack.Children.Add(new Label { Text = "This can have a Sauce" });
                        this.Children.Add(new ContentPage { Content = stack });
                    }


                }

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
            }else
            {
                ContentPage nextPage = this.Children[index + 1];
                CurrentPage = nextPage;
            }
        }
    }
}