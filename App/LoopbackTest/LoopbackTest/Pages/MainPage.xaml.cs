using LBXamarinSDK;
using LBXamarinSDK.LBRepo;
using LoopbackTest.Helper;
using LoopbackTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LoopbackTest.Pages
{
    public partial class MainPage : ContentPage
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value;
                OnPropertyChanged("Username");
            }
            }


        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            NavigationPage.SetHasNavigationBar(this, false);
            try
            {
                getUser();
            }catch(Exception e)
            {
                DisplayAlert("Error", e.Message.ToString(), "OK");
            }
        }

        async void getUser()
        {
            var user = await LoginHelper.findById(SessionData.Resolve<AccessToken>().userID);
            Username = user.username;
   
        }

        async public void LogoutClicked(object sender, EventArgs args)
        {
            if (await LoginHelper.Logout())
            {
                await Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("Error", "Invalid Login credentials", "OK");
            }
        }
    }
}
