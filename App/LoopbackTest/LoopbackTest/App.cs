using Android.Accounts;
using LBXamarinSDK;
using LoopbackTest.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace LoopbackTest
{
    public class App : Xamarin.Forms.Application
    {
        public User CurrentUser { get; set; }
        public AccountManager accountManager;
        public App()
        {
            App.Current.Properties["serverBaseURL"] = "http://10.0.0.26:3000/api/";
            Gateway.SetServerBaseURL(new Uri(App.Current.Properties["serverBaseURL"].ToString()));
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
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
