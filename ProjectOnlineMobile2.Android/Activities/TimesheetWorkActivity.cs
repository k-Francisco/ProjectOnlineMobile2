using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using View = Android.Views.View;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using ProjectOnlineMobile2.Pages;
using Xamarin.Forms.Platform.Android;

namespace ProjectOnlineMobile2.Android.Activities
{
    [Activity(Label = "")]
    public class TimesheetWorkActivity : AppCompatActivity
    {
        private Fragment _timesheetWorkPage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_timesheet_work);

            string taskName = this.Intent.GetStringExtra("TASK_NAME");
            string lineId = this.Intent.GetStringExtra("LINE_ID");

            _timesheetWorkPage = new TimesheetWorkPage().CreateSupportFragment(this);

            MessagingCenter.Instance.Send<String>("", "SendTimesheetIds");

            var toolbar = FindViewById<Toolbar>(Resource.Id.work_toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.Title = taskName;
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.timesheet_work_frame, _timesheetWorkPage)
                .Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.save_work_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    Finish();
                    break;
                case Resource.Id.action_save:
                    MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
                    break;
            }

            return true;
        }
    }
}