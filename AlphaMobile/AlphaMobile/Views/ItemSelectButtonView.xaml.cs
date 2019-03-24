using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlphaMobile.Views
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemSelectButtonView : ContentView
	{
        public static readonly BindableProperty LeftButtonTextProperty =
            BindableProperty.Create("LeftButtonText", typeof(string), typeof(ItemSelectButtonView), default(string));

        public string LeftButtonText
        {
            get { return (string)GetValue(LeftButtonTextProperty);}
            set { SetValue(LeftButtonTextProperty, value); }
        }

        public static readonly BindableProperty RightButtonTextProperty =
            BindableProperty.Create("RightButtonText", typeof(string), typeof(ItemSelectButtonView), default(string));

        public string RightButtonText
        {
            get { return (string)GetValue(RightButtonTextProperty); }
            set { SetValue(RightButtonTextProperty, value); }
        }

        public event EventHandler RightButtonClicked;
        public event EventHandler LeftButtonClicked;



        public ItemSelectButtonView ()
		{
			InitializeComponent ();
            LeftButton.SetBinding(Button.TextProperty, new Binding("LeftButtonText", source: this));
            RightButton.SetBinding(Button.TextProperty, new Binding("RightButtonText", source: this));

            LeftButton.Command = new Command(() =>
            {
                LeftButtonClicked?.Invoke(this, EventArgs.Empty);
            });

            RightButton.Command = new Command(() =>
            {
                RightButtonClicked?.Invoke(this, EventArgs.Empty);
            });
        }

    }
}