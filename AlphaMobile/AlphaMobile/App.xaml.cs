using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AlphaMobile.Views;
using AlphaMobile.Controllers.API;
using AlphaMobile.Models;
using AlphaMobile.Models.APIModels;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AlphaMobile
{
    public partial class App : Application
    {
        // APP configuration
        private const string UserLoginKey = "UserLogin";
        private const string UserPWKey = "UserPW";
        private const string UserProfileKey = "UserProfile";
        private const string OAuth_TokenKey = "OAuthToken";
        private const string OAuth_VilidityTimeKey = "OAuthValidityTime";
        private const string WelcomeCustomerWizzardDoneKey = "WelcomeCustomerWizzardDone";

        // Non saved objects 
        public Restaurant resto;
        public OrderAPIModel order;
        public OrderedItem orderedItem;


        // Memorized objects
        public bool WelcomeCustomerWizzardDone
        {
            get
            {
                if (Properties.ContainsKey(WelcomeCustomerWizzardDoneKey))
                    return (bool)Properties[WelcomeCustomerWizzardDoneKey];
                return false;
            }

            set
            {
                Properties[WelcomeCustomerWizzardDoneKey] = value;

            }
                
        }
        public string UserLogin
        {
            get
            {
                if (Properties.ContainsKey(UserLoginKey))
                    return Properties[UserLoginKey].ToString();
                return "";
            }
            set
            {
                Properties[UserLoginKey] = value;
            }
        }
        public string UserPW
        {
            get
            {
                if (Properties.ContainsKey(UserPWKey))
                    return Properties[UserPWKey].ToString();
                return "";
            }
            set
            {
                Properties[UserPWKey] = value;
            }
        }
        public string UserProfile
        {
            get
            {
                if (Properties.ContainsKey(UserProfileKey))
                    return Properties[UserProfileKey].ToString();
                return "";
            }
            set
            {
                Properties[UserProfileKey] = value;
            }
        }
        public string OAuth_Token
        {
            get
            {
                if (Properties.ContainsKey(OAuth_TokenKey))
                    return Properties[OAuth_TokenKey].ToString();
                return "";
            }
            set
            {
                Properties[OAuth_TokenKey] = value;
            }
        }
        public string OAuth_VilidityTime
        {
            get
            {
                if (Properties.ContainsKey(OAuth_VilidityTimeKey))
                    return Properties[OAuth_VilidityTimeKey].ToString();
                return "";
            }
            set
            {
                Properties[OAuth_VilidityTimeKey] = value;
            }
        }

        private CloudController _cloudController = new CloudController();


        App app = Application.Current as App;
        


        public App()
        {
            InitializeComponent();
            //app.OAuth_Token = "";
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override async void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
