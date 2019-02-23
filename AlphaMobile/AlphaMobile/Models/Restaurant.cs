using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaMobile.Models
{
    class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; } 
        public string Address { get; set; }    
        public byte[] Image { get; set; } 

        //public virtual ICollection<OpenTimePeriod> OpeningTimes { get; set; }

        //public virtual Menu Menu { get; set; }
    }
}
