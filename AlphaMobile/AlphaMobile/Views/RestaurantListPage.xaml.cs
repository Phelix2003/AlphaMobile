﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AlphaMobile.Controllers.API;
using AlphaMobile.Models;
using Plugin.Connectivity;

namespace AlphaMobile.Views
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestaurantListPage : ContentPage
	{
        private CloudController _Cloud = new CloudController();

        public RestaurantListPage()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            while (!CrossConnectivity.Current.IsConnected)
                await DisplayAlert("Connection", "Oups, il y a de la friture sur la ligne. Vérifier votre connexion internet", "Ok");
            List<Restaurant> restaurants = await _Cloud.GetRestaurantListAsync();
            if(restaurants == null)
            {
                await DisplayAlert("Erreur", "Il semble y avoir une problème de communication", "Ok");
            }

            //Configure the first line for the tittle 
            var label = new Label { Text = "A proximité", FontSize = 24 };
            GridArea.Children.Add(label, 0, 0);
            Grid.SetColumnSpan(label, 2);
            GridArea.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(20, GridUnitType.Auto)
            });

            int line = 1;
            int coloumn = 0;

            // Configure the First line height
            GridArea.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Auto)
            });

            foreach (var resto in restaurants)
            {
                // Création de la fiche restaurant
                var layout = new StackLayout();
                layout.BackgroundColor = Color.Beige;
                // Ajout de l'image
                Image image = new Image { Source = "https://alpha-easio.azurewebsites.net/restaurant/RenderRestoPhoto?RestoId=" + resto.Id };
                layout.Children.Add(image);
                //layout.SetBinding(StackLayout.IsVisibleProperty, new Binding(""));
                // Titre du restaurant
                layout.Children.Add(new Label { Text = resto.Name, FontAttributes = FontAttributes.Bold, FontSize = 20 });
                
                //Description du restaurant 
                layout.Children.Add(new Label { Text = resto.Description, FontSize = 14 });
                // Associe l'ID du restaurant à ce stack layout. Pour retrouver le restaurant quand on click dessus. 
                layout.Children.Add(new Label { Text = resto.Id.ToString(), IsVisible = false });

                // Ajout d'un GestureRecognizure sur le layout afin de capter les les clicks. 
                var gesture = new TapGestureRecognizer();
                // Association du click sur la fonction OnClickRestaurant
                gesture.Tapped += OnClickRestaurant;
                layout.GestureRecognizers.Add(gesture);

                GridArea.Children.Add(layout, coloumn, line);                
                if(coloumn == 1)
                {
                    GridArea.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Auto)
                    });
                    line++;
                    coloumn = 0;
                }
                else
                {
                    coloumn++;
                }

            }


        }

        private async void OnClickRestaurant(object sender, EventArgs e)
        {
            StackLayout layout = (StackLayout)sender;
            // Récupère les élément constituant la stack Layout afin de retrouver l'Id du restaurant
            var childrens = layout.Children;
            var label = (Label)childrens.FirstOrDefault(r => r.IsVisible == false);


            await Navigation.PushAsync(new RestaurantDetailsView(int.Parse(label.Text)));
        }
    }
}