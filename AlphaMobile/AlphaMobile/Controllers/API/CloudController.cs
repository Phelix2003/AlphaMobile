using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaMobile.Configuration;
using AlphaMobile.Controllers;
using System.Diagnostics;
using Newtonsoft.Json;
using AlphaMobile.Models.APIModels;
using AlphaMobile.Models;
using Xamarin.Forms;
using System.IO;
using System.Net.Http.Headers;
using System.Net;

namespace AlphaMobile.Controllers.API
{
    public class CloudController
    {
        private HttpClient _Client = new HttpClient();
        App app = Application.Current as App;

        public async Task<bool> UpdateOAuthToken()
        {            
            string url = AppConfiguration.CloudServer_URI + "/token";
            _Client.BaseAddress = new Uri(url);
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // define the expected return format

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/token");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", app.UserLogin ),
                new KeyValuePair<string, string>("password", app.UserPW)
            });

            var response = await _Client. SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                TokenRequestResponseAPIModel tokenResponse = JsonConvert.DeserializeObject<TokenRequestResponseAPIModel>(await response.Content.ReadAsStringAsync());
                app.OAuth_Token = tokenResponse.access_token;
                app.OAuth_VilidityTime = tokenResponse.expires_in;
                return true;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {

                    ErrorResponseAPIModel errorDescription = JsonConvert.DeserializeObject<ErrorResponseAPIModel>(await response.Content.ReadAsStringAsync());
                    //await DisplayAlert("Erreur", errorDescription.error_description, "Ok"); TODO logger cette erreur
                    return false;

                }
                else
                {
                    // Problème de connexion
                    return false;
                }                
            }
        }

        public async Task<Restaurant> GetRestaurantDetailAsync(int Id)
        {
            string requestURI = AppConfiguration.APIServer_URI + "/Restaurant/" + Id.ToString();
            _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", app.OAuth_Token);
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var content = await _Client.GetStringAsync(requestURI);
                if (content == null)
                    return null;
                ListRestoAPIModel restoAPIModel = JsonConvert.DeserializeObject<ListRestoAPIModel>(content);
                if (restoAPIModel.Restos.Count == 0)
                    return null;

                Restaurant resto = new Restaurant
                {
                    Id = restoAPIModel.Restos[0].Id,
                    Address = restoAPIModel.Restos[0].Address,
                    Description = restoAPIModel.Restos[0].Description,
                    Image = restoAPIModel.Restos[0].Image,
                    Name = restoAPIModel.Restos[0].Name,
                    PhoneNumber = restoAPIModel.Restos[0].PhoneNumber,
                    Menu = new Models.Menu
                    {
                        MenuId = restoAPIModel.Restos[0].Menu.MenuId,
                        Name = restoAPIModel.Restos[0].Menu.Name,
                        ItemList = new List<Item>()
                    }
                };
                foreach (var item in restoAPIModel.Restos[0].Menu.ItemList)
                {
                    resto.Menu.ItemList.Add(new Item
                    {
                        Name = item.Name,
                        Brand = item.Brand,
                        ImageSource = AppConfiguration.ItemPictureRender_URI + "?ItemId=" + item.ItemId.ToString(),
                        UnitPrice = item.UnitPrice,
                        Description = item.Description,
                        HasSize = item.HasSize,
                        ItemId = item.ItemId,
                        CanBeHotNotCold = item.CanBeHotNotCold,
                        CanBeSalt = item.CanBeSalt,
                        CanHaveMeat = item.CanHaveMeat,
                        CanHaveSauce = item.CanHaveSauce,
                        TypeOfFood = item.TypeOfFood
                    });
                }
                return resto;
            }
            catch (HttpRequestException e)
            {
                if(e.Message.Contains("401"))
                {
                    // Authentication failed. 
                    // OAuthToken has been depreciated. Need for a new one 
                    app.OAuth_Token = "";                    
                    return null;
                }else
                {
                    // Connexion error
                    return null;
                }
            }
        }

        public async Task<List<Restaurant>> GetRestaurantListAsync()
        {
            string requestURI = AppConfiguration.APIServer_URI + "/RestaurantList/";
            _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", app.OAuth_Token);
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var content = await _Client.GetStringAsync(requestURI);
                if (content == null)
                    return null;

                ListRestoAPIModel restoAPIModel = JsonConvert.DeserializeObject<ListRestoAPIModel>(content);

                if (restoAPIModel.Restos.Count == 0)
                    return null;

                List<Restaurant> restaurants = new List<Restaurant>();

                foreach (var restoAPI in restoAPIModel.Restos)
                {
                    if(restoAPI.Menu !=null)
                    {
                        Restaurant resto = new Restaurant
                        {
                            Id = restoAPI.Id,
                            Address = restoAPI.Address,
                            Description = restoAPI.Description,
                            Image = restoAPI.Image,
                            Name = restoAPI.Name,
                            PhoneNumber = restoAPI.PhoneNumber,
                            Menu = new Models.Menu
                            {
                                MenuId = restoAPI.Menu.MenuId,
                                Name = restoAPI.Menu.Name,
                                ItemList = new List<Item>()
                            }
                        };
                        foreach (var item in restoAPI.Menu.ItemList)
                        {

                            resto.Menu.ItemList.Add(new Item
                            {
                                Name = item.Name,
                                Brand = item.Brand,
                                ImageSource = AppConfiguration.ItemPictureRender_URI + "?ItemId=" + item.ItemId.ToString(),
                                UnitPrice = item.UnitPrice,
                                Description = item.Description,
                                HasSize = item.HasSize,
                                ItemId = item.ItemId,
                                CanBeHotNotCold = item.CanBeHotNotCold,
                                CanBeSalt = item.CanBeSalt,
                                CanHaveMeat = item.CanHaveMeat,
                                CanHaveSauce = item.CanHaveSauce,
                                TypeOfFood = item.TypeOfFood
                            });
                        }
                        restaurants.Add(resto);


                    }
                }
                return restaurants;
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }

    }
}
