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

namespace AlphaMobile.Controllers.API
{
    public class CloudController
    {
        private HttpClient _Client = new HttpClient();

        public async Task<Restaurant> GetRestaurantDetailAsync(int Id)
        {
            string requestURI = AppConfiguration.APIServer_URI + "/RestaurantAPI/" + Id.ToString();
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
                foreach(var item in restoAPIModel.Menu.ItemList)
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
                        ImageSource = "https://alpha-easio.azurewebsites.net/Menu/RenderItemPhoto?ItemId=4",
                        UnitPrice = item.UnitPrice,
                        Description = item.Description,
                        HasSize = item.HasSize,
                        ItemId = item.ItemId,
                        CanBeHotNotCold = item.CanBeHotNotCold,
                        CanBeSalt = item.CanBeSalt,
                        CanHaveMeat = item.CanHaveMeat,
                        CanHaveSauce = item.CanHaveSauce
                    });
                }
                return resto;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
