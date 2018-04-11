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
using ProjectOnlineMobile2.Android.Fragments;

namespace ProjectOnlineMobile2.Android.Activities
{

    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon")]
    public class LoginActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);

            if (Singleton.Instance.TokenServices.IsAlreadyLoggedIn())
            {
                GoToLandingPage();
            }
            else
            {
                SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.flFragmentContainer, LoginFragment.NewInstance())
                .Commit();
            }
            
        }

        public void GoToLandingPage() {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            this.Finish();
        }
    }
}