using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlphaMobile.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; } 
        public string Address { get; set; }    
        public byte[] Image { get; set; } 

        //public virtual ICollection<OpenTimePeriod> OpeningTimes { get; set; }

        public Menu Menu { get; set; }
    }

    public class Menu
    {
        public int MenuId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Item> ItemList { get; set; }
    }

    public class Item
    {
        public void UpdateDisplayOrderedQuanity(List<OrderedItem> listOrderedItems)
        {
            if(listOrderedItems != null)
            {
                OrderedDisplayQty = 0;
                foreach (var orderedItem in listOrderedItems)
                {
                    if(orderedItem.ItemId == ItemId)
                    {
                        OrderedDisplayQty += orderedItem.Quantity;
                    }
                }
            }
        }
        public int ItemId { get; set; }
        public string Name { get; set; }

        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public List<string> Price
        {
            get
            {
                if (!HasSize)
                {
                    return new List<string> { UnitPrice.ToString() + "€" };
                }
                else
                {
                    List<string> priceList = new List<string>();
                    foreach (var size in AvailableSizes)
                    {                        
                        priceList.Add(size.MealSize.ToString() + " - " + size.Price.ToString() + "€");
                    }
                    return priceList;
                }
            }
        }
        public string NameWithQty
        {
            get
            {
                if (OrderedDisplayQty > 0)
                {
                    return Name + " (" + OrderedDisplayQty.ToString() + ")";

                }
                else
                {
                    return Name;
                }
            }
        }

        public TypeOfFood TypeOfFood { get; set; }
        public bool HasSize { get; set; }
        public List<SizedMeal> AvailableSizes { get; set; }
        public bool CanBeSalt { get; set; }
        public bool CanBeHotNotCold { get; set; }
        public bool CanHaveMeat { get; set; }
        public bool CanHaveSauce { get; set; }

        public int OrderedDisplayQty { get; set; }
        public Image Image { get; set; }
        public UriImageSource ImageSource { get; set; }
    }

    public class SizedMeal
    {
        public int Id { get; set; }
        public MealSize MealSize { get; set; }
        public decimal Price { get; set; }
    }

    public enum TypeOfFood
    {
        Frites = 0,
        Sauce = 1,
        Snack = 2,
        Meal = 3,
        Menu = 4,
        Boisson = 5
    }

    public enum MealTime
    {
        Breakfast = 0,
        Lunch = 1,
        Diner = 2
    }



}
