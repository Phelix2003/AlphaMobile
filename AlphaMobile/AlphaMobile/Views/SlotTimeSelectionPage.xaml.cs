using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlphaMobile.Models.APIModels;
using AlphaMobile.Controllers.API;
using AlphaMobile.Models;
using AlphaMobile.ModelViews;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlphaMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SlotTimeSelectionPage : ContentPage
	{
        private CloudController _Cloud = new CloudController();
        private int _restoId;
        App app = Application.Current as App;

        private List<OrderSlot> _slotTime;
        private List<PossibleSlotTimeViewModel> _slotTimeViewModel = new List<PossibleSlotTimeViewModel>();
        private TimeSpan timeBetweenTwoButtons = new TimeSpan(0, 30, 0); // TODO : to add this in configuration




        public SlotTimeSelectionPage (int RestoId)
		{
			InitializeComponent ();
            _restoId = RestoId;
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _slotTime = await _Cloud.GetRestaurantSlotTime(_restoId);



            if (_slotTime == null)
            {
                await DisplayAlert("Complet", "Il n'y plus de desiponnibilité dans ce restaurant aujourd'hui", "Ok");
            }
            else
            {
                // Creates the view to select the slot. 
                // Convert all available slot time in group of buttons
                foreach(var item in _slotTime)
                {
                    DateTime roundedTime = RoundDown(item.OrderSlotTime, timeBetweenTwoButtons);
                    if(_slotTimeViewModel.Count(r => r.TimeFrom == roundedTime) == 0)
                    {
                        _slotTimeViewModel.Add(new PossibleSlotTimeViewModel
                        {Available =true,
                        SlotGroup = item.SlotGroup,
                        TimeFrom = roundedTime,
                        TimeTo = roundedTime.Add(timeBetweenTwoButtons)                      
                        });
                    }                  
                }

                // Set page content by code
                //Configure the first line for the tittle 




                // Configure the First line height
                GridArea.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(80, GridUnitType.Auto)
                });

                int line = 0;
                int coloumn = 0;


                if (_slotTimeViewModel.Where(r => r.SlotGroup == MealTime.Breakfast).Count() != 0)
                {
                    coloumn = 0; // every now section start with on the first grid element
                    //set the line description for the Meal time (breakfast / lunch / diner
                    var label = new Label { Text = "Déjeuner", FontSize = 24 };
                    GridArea.Children.Add(label, 0, 0);
                    Grid.SetColumnSpan(label, 4);
                    GridArea.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(20, GridUnitType.Auto)
                    });
                    line++;

                    // set all element in grid at the the associated MealTime
                    foreach (var slot in _slotTimeViewModel.Where(r=>r.SlotGroup == MealTime.Breakfast))
                    {
                        // Create the frame around the buttin
                        var button = new Button();
                        button.BackgroundColor = Color.Beige;
                        button.BorderWidth = 1;
                        button.BorderColor = Color.Gray;

                        // Set the button content 
                        button.Text = slot.TimeFrom.Hour.ToString() + " : " + slot.TimeFrom.Minute.ToString();

                        // Set the methode to use when the button is clicked
                        button.Clicked += OnClickButton;
                        slot.ButtonId = button.Id.ToString(); // Associate a unique ID to the slot to find it back later on. 

                        // Integrate the button in the grid 
                        GridArea.Children.Add(button, coloumn, line);

                        // Prepare the next line. 
                        if (coloumn >= 3)
                        {
                            GridArea.RowDefinitions.Add(new RowDefinition
                            {
                                Height = new GridLength(80, GridUnitType.Auto)
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

                if (_slotTimeViewModel.Where(r => r.SlotGroup == MealTime.Lunch).Count() != 0)
                {
                    coloumn = 0; // every now section start with on the first grid element
                    //set the line description for the Meal time (breakfast / lunch / diner
                    var label = new Label { Text = "Diner", FontSize = 24 };
                    GridArea.Children.Add(label, 0, 0);
                    Grid.SetColumnSpan(label, 4);
                    GridArea.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(20, GridUnitType.Auto)
                    });
                    line++;

                    // set all element in grid at the the associated MealTime
                    foreach (var slot in _slotTimeViewModel.Where(r=>r.SlotGroup == MealTime.Lunch))
                    {
                        // Create the frame around the buttin
                        var button = new Button();
                        button.BackgroundColor = Color.Beige;
                        button.BorderWidth = 1;
                        button.BorderColor = Color.Gray;

                        // Set the button content 
                        button.Text = slot.TimeFrom.Hour.ToString() + " : " + slot.TimeFrom.Minute.ToString();

                        // Set the methode to use when the button is clicked
                        button.Clicked += OnClickButton;
                        slot.ButtonId = button.Id.ToString(); // Associate a unique ID to the slot to find it back later on. 

                        // Integrate the button in the grid 
                        GridArea.Children.Add(button, coloumn, line);

                        // Prepare the next line. 
                        if (coloumn >= 3)
                        {
                            GridArea.RowDefinitions.Add(new RowDefinition
                            {
                                Height = new GridLength(80, GridUnitType.Auto)
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

                if (_slotTimeViewModel.Where(r => r.SlotGroup == MealTime.Diner).Count() != 0)
                {
                    coloumn = 0; // every now section start with on the first grid element
                    //set the line description for the Meal time (breakfast / lunch / diner
                    var label = new Label { Text = "Souper", FontSize = 24 };
                    GridArea.Children.Add(label, 0, 0);
                    Grid.SetColumnSpan(label, 4);
                    GridArea.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(20, GridUnitType.Auto)
                    });
                    line++;

                    // set all element in grid at the the associated MealTime
                    foreach (var slot in _slotTimeViewModel.Where(r => r.SlotGroup == MealTime.Diner))
                    {
                        // Create the frame around the buttin
                        var button = new Button();
                        button.BackgroundColor = Color.Beige;
                        button.BorderWidth = 1;
                        button.BorderColor = Color.Gray;

                        // Set the button content 
                        button.Text = slot.TimeFrom.Hour.ToString() + " : " + slot.TimeFrom.Minute.ToString();

                        // Set the methode to use when the button is clicked
                        button.Clicked += OnClickButton;
                        slot.ButtonId = button.Id.ToString(); // Associate a unique ID to the slot to find it back later on. 

                        // Integrate the button in the grid 
                        GridArea.Children.Add(button, coloumn, line);

                        // Prepare the next line. 
                        if (coloumn >= 3)
                        {
                            GridArea.RowDefinitions.Add(new RowDefinition
                            {
                                Height = new GridLength(80, GridUnitType.Auto)
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


            }
        }

        private async void OnClickButton(object sender, EventArgs e)
        {
            //Retreive the button beeing clicked
            var button = sender as Button;
            //Retreive the time preiode expected bhind the button
            var seletedSlot = _slotTimeViewModel.FirstOrDefault(r => r.ButtonId == button.Id.ToString());
            // Retreive the first slotime available in this time prediode
            var expectedSlotTiem = _slotTime
                .Where(r => r.OrderSlotTime.CompareTo(seletedSlot.TimeFrom) >= 0)                            
                .Where(r => r.OrderSlotTime.CompareTo(DateTime.Now) >= 0)                          
                .Where(r => r.OrderSlotTime.CompareTo(seletedSlot.TimeTo) <= 0)
                .FirstOrDefault();
            if(expectedSlotTiem != null) // Could eventually be null if user stay on the page a long time and time goes over the last slot available.
            {
                if (await _Cloud.SetRestaurantSlotTime(expectedSlotTiem.OrderSlotId))
                {
                    // Successfuly cached a slot time
                    await Navigation.PopAsync();
                }
                else
                {

                }
            }
            
        }

        private DateTime RoundDown(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks) / d.Ticks * d.Ticks, dt.Kind);
        }

    }




}