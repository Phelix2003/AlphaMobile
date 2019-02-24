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
        public int ItemId { get; set; }
        public string Name { get; set; }

        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }

        public bool HasSize { get; set; }
        //public virtual List<SizedMeal> AvailableSizes { get; set; }
        public bool CanBeSalt { get; set; }
        public bool CanBeHotNotCold { get; set; }
        public bool CanHaveMeat { get; set; }
        public bool CanHaveSauce { get; set; }

        public Image Image { get; set; }
        public string ImageSource { get; set; }
    }

}
