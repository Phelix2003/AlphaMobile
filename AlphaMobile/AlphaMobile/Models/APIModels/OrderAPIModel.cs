using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaMobile.Models.APIModels
{
    public class OrderAPIModel
    {
        public int Id { get; set; }

        public bool IsOrderCompleted { get; set; }
        public bool IsInProgress { get; set; }
        public int OrderRestaurantId { get; set; }

        public OrderSlotAPI OrderSlot { get; set; }
        //public virtual Payment Payment { get; set; }

        //A vérifier si dans une collection on peut ajouter plusieurs fois le même élément... 
        public ICollection<OrderedItemAPIModel> OrderedItems { get; set; }
    }

    public class OrderedItemAPIModel
    {
        public int ItemId { get; set; }

        public int Quantity { get; set; }
        public MealSize? SelectedSize { get; set; }
        public bool SelectedSalt { get; set; }
        public bool SelectedHotNotCold { get; set; }

        public int? SelectedMeatId { get; set; }

        public int? SelectedSauceId { get; set; }
    }

    //public class SlotTimeAPIModel
    //{
    //    public SlotTimeAPIModel()
    //    {
    //        ListOfPossibleTimes = new List<PossibleTimeAPI>();
    //    }
               
    //    public List<PossibleTimeAPI> ListOfPossibleTimes { get; set; }
    //    public int OrderId { get; set; }
    //}





}
