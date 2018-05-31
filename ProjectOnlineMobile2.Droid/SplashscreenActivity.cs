using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ProjectOnlineMobile2.Droid
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/SplashTheme", Icon = "@mipmap/ic_launcher", LaunchMode = LaunchMode.SingleTop)]
    public class SplashscreenActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }
    }
}