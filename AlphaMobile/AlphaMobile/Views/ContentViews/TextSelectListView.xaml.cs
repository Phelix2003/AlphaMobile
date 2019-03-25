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
	public partial class TextSelectListView : ContentView
	{
        public static readonly BindableProperty ItemListViewProperty =
            BindableProperty.Create("ItemListView", typeof(IEnumerable<string>), typeof(ItemSelectListView), default(string));

        public IEnumerable<string> ItemListView
        {
            get { return (IEnumerable<string>)GetValue(ItemListViewProperty); }
            set { SetValue(ItemListViewProperty, value); }
        }


        public static readonly BindableProperty TitleLabelProperty =
            BindableProperty.Create("TitleLabel", typeof(string), typeof(ItemSelectListView), default(string));

        public string TitleLabel
        {
            get { return (string)GetValue(TitleLabelProperty); }
            set { SetValue(TitleLabelProperty, value); }
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        public void NotifyItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ItemSelected(sender, e);
        }

        public TextSelectListView()
        {
            InitializeComponent();
            ItemsListView.SetBinding(ListView.ItemsSourceProperty, new Binding("ItemListView", source: this));
            Title.SetBinding(Label.TextProperty, new Binding("TitleLabel", source: this));
            ItemsListView.ItemSelected += NotifyItemSelected;
            ItemsListView.SetBinding(ListView.SelectedItemProperty, new Binding("ItemSelected", source: this));
        }
    }
}