using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Newtonsoft.Json;
using ProjectOnlineMobile2.Pages;
using Xamarin.Forms.Platform.Android;

namespace ProjectOnlineMobile2.Droid
{
    [Activity(Label = "TimesheetWorkActivity")]
    public class TimesheetWorkActivity : AppCompatActivity
    {

        private Fragment _timesheetWorkFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_timesheet_work);

            string taskName = this.Intent.GetStringExtra("TASK_NAME");
            string lineId = this.Intent.GetStringExtra("LINE_ID");
            TimesheetWorkPage workPage = JsonConvert.DeserializeObject<TimesheetWorkPage>(this.Intent.GetStringExtra("WORK_PAGE"));
            //_timesheetWorkFragment = JsonConvert.DeserializeObject<TimesheetWorkPage>(this.Intent.GetStringExtra("WORK_PAGE")).CreateSupportFragment(this);
            _timesheetWorkFragment = new TimesheetWorkPage().CreateSupportFragment(this);
            var toolbar = FindViewById<Toolbar>(Resource.Id.work_toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.Title = taskName;
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.timesheet_work_frame, _timesheetWorkFragment)
                .Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.timesheet_work_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == global::Android.Resource.Id.Home)
            {
                this.Finish();
            }
            else if (item.ItemId == Resource.Id.menu_save_work)
            {
                MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
            }

            return true;
        }

    }
}