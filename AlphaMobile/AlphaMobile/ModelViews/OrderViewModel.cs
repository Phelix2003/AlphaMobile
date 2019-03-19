using System;
using System.Collections.Generic;
using System.Text;
using AlphaMobile.Models;

namespace AlphaMobile.ModelViews
{
    public class PossibleSlotTimeViewModel
    {
        public string Id { get; set; }
        public string ButtonId { get; set; }

        public  MealTime SlotGroup { get; set; }

        public DateTime TimeFrom { get; set; }
        public string TimeFromText() { return TimeFrom.ToString("T"); }
        public DateTime TimeTo { get; set; }
        public string TimeToText() { return TimeTo.ToString("T"); }
        public bool Available { get; set; }
    }
}
