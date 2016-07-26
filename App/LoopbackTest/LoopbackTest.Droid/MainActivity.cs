using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace LoopbackTest.Droid
{
    [Activity(Label = "LoopbackTest", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

                       global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        public void startIntent(Intent i)
        {
            StartActivity(i);
        }

    }
}

