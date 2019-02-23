using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using AlphaMobile.MarkupExtensions;
using AlphaMobile.Models;


namespace AlphaMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            Restaurant Resto = new Restaurant()
            {
                Address = "Rue Tribomont, 8. 4860 Wegnez ",
                Description = "Resto",
                Id = 1,
                Name = "Le Postay"


            };
            InitializeComponent();
        }
    }
}
