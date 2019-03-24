using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaMobile.Models
{

    public class OrderedItem
    {
        public Item Item { get; set; }

        public int Quantity { get; set; }
        public MealSize? SelectedSize { get; set; }
        public bool SelectedSalt { get; set; }
        public bool SelectedHotNotCold { get; set; }

        public int? SelectedMeatId { get; set; }

        public int? SelectedSauceId { get; set; }
    }


    public enum MealSize
    {
        S = 0,
        M = 1,
        L = 2,
        XL = 3,
        XXL = 4
    }
}
