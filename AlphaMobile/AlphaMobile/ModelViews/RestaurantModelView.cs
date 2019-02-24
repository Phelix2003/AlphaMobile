using System;
using System.Collections.Generic;
using System.Text;
using AlphaMobile.Models;

namespace AlphaMobile.ModelViews
{
    class ItemGroup : List<Item>
    {
        public string Title { get; set;}
        public string ShortTitle { get; set; }

        public ItemGroup(string title, string shortTitle)
        {
            Title = title;
            ShortTitle = shortTitle;
        }
    }
}
