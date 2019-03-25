using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaMobile.Models
{
    public class Order
    {
        public int Id { get; set; }

        public bool IsOrderCompleted { get; set; }
        public bool IsInProgress { get; set; }
        public int OrderRestaurantId { get; set; }

        public OrderSlot OrderSlot { get; set; }
        //public virtual Payment Payment { get; set; }

        //A vérifier si dans une collection on peut ajouter plusieurs fois le même élément... 
        public ICollection<OrderedItem> OrderedItems { get; set; }
    }

    public class OrderSlot
    {
        public int OrderSlotId { get; set; }

        public DateTime OrderSlotTime { get; set; }

        // To group the slot by openning time
        public MealTime SlotGroup { get; set; }
    }

    public class OrderedItem
    {
        public Item Item { get; set; }
        public int ItemId { get; set; }

        public int Quantity { get; set; }
        public MealSize? SelectedSize { get; set; }
        public bool SelectedSalt { get; set; }
        public bool SelectedHotNotCold { get; set; }

        public int? SelectedMeatId { get; set; }

        public int? SelectedSauceId { get; set; }

        public bool HasBeenConfigured { get; set;}
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
