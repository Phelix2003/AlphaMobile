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
using RestSharp;

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
            string url = AppConfiguration.CloudServer_URI + "/token";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // define the expected return format

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/token");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", Email.Text),
                new KeyValuePair<string, string>("password", Password.Text)
            });

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TokenRequestResponseAPIModel tokenResponse = JsonConvert.DeserializeObject<TokenRequestResponseAPIModel>(await response.Content.ReadAsStringAsync());
                Editorbox.Text = tokenResponse.access_token.ToString();
            }
            else
            {
                Editorbox.Text = "";
            }
            

        }
    }
}