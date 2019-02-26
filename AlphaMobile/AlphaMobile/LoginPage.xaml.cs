using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using AlphaMobile.Configuration;
using Newtonsoft.Json;
using AlphaMobile.Models.APIModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlphaMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            string URL = AppConfiguration.APIServer_URI + "/token";
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // Define the expected return format

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "relativeAddress");
            request.Content = new FormUrlEncodedContent(new[]
{
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", Email.Text),
                new KeyValuePair<string, string>("password", Password.Text)
            });

            var json = await client.SendAsync(request);
            if (json != null)
            {
                JsonConvert.DeserializeObject<TokenRequestResponseAPIModel>(json.ToString());
            }
            

        }
    }
}