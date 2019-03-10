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
using AlphaMobile;

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

            BindingContext = Application.Current;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var app = Application.Current as App;

            HttpClient client = new HttpClient();
            string url = AppConfiguration.CloudServer_URI + "/token";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // define the expected return format

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/token");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", app.UserLogin ),
                new KeyValuePair<string, string>("password", app.UserPW)
            });

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TokenRequestResponseAPIModel tokenResponse = JsonConvert.DeserializeObject<TokenRequestResponseAPIModel>(await response.Content.ReadAsStringAsync());
                app.OAuth_Token = tokenResponse.access_token;
                app.OAuth_VilidityTime = tokenResponse.expires_in;
                await DisplayAlert("Connexion", "Login / PW correct", "Ok");
                await Navigation.PopModalAsync();                
            }
            else
            {
                if(response.StatusCode== HttpStatusCode.BadRequest)
                {

                    ErrorResponseAPIModel errorDescription = JsonConvert.DeserializeObject<ErrorResponseAPIModel>(await response.Content.ReadAsStringAsync());
                    await DisplayAlert("Erreur", errorDescription.error_description, "Ok");
                    
                }else
                {
                    await DisplayAlert("Oupps", "Connexion problem", "Ok");
                    await Navigation.PopModalAsync();
                }
                Editorbox.Text = "";
            }
            

        }
    }
}