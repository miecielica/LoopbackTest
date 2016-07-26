using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LBXamarinSDK.LBRepo;
using LBXamarinSDK;
using LoopbackTest.Droid;
using LoopbackTest.Models;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidMethods))]
namespace LoopbackTest.Droid
{
    public class AndroidMethods : IAndroidMethods
    {
        

        public void sendVerificationEmail(string address, string id, string verificationToken)
        {
            var email = new Intent(Android.Content.Intent.ActionSend);
            email.PutExtra(Android.Content.Intent.ExtraEmail, new string[] {address});

            email.PutExtra(Android.Content.Intent.ExtraSubject, "Pfush Verification");

              
            email.PutExtra(Android.Content.Intent.ExtraText, "Thx for registering. pls click link: http://"+App.Current.Properties["serverBaseURL"]+"/users/confirm?uid="+id+"&token="+verificationToken);
            email.SetType("message/rfc822");
            Application.Context.StartActivity(email);

        }
    }
}