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

        public async Task<Restaurant> GetRestaurantDetailAsync(int Id)
        {
            string requestURI = AppConfiguration.APIServer_URI + "/RestaurantAPI/" + Id.ToString();
            _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", app.OAuth_Token);
            _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var content = await _Client.GetStringAsync(requestURI);
                if (content == null)
                {
                    return null;
                }
                RestoAPIModel restoAPIModel = JsonConvert.DeserializeObject<RestoAPIModel>(content);

                Restaurant resto = new Restaurant
                {
                    Id = restoAPIModel.Id,
                    Address = restoAPIModel.Address,
                    Description = restoAPIModel.Description,
                    Image = restoAPIModel.Image,
                    Name = restoAPIModel.Name,
                    PhoneNumber = restoAPIModel.PhoneNumber,
                    Menu = new Models.Menu
                    {
                        MenuId = restoAPIModel.Menu.MenuId,
                        Name = restoAPIModel.Menu.Name,
                        ItemList = new List<Item>()
                    }
                };
                foreach (var item in restoAPIModel.Menu.ItemList)
                {
                    // converting imgae Bytre[] to Image 
                    Image image = new Image();
                    Stream stream = new MemoryStream(item.Image);
                    image.Source = ImageSource.FromStream(() => { return stream; });

                    resto.Menu.ItemList.Add(new Item
                    {
                        Name = item.Name,
                        Brand = item.Brand,
                        Image = image,
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
    }
}
