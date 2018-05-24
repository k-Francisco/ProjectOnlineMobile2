using Android.App;
using Android.OS;
using ProjectOnlineMobile2.Droid.Fragments;

using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Support.V7.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Xamarin.Forms;
using System;
using ProjectOnlineMobile2.Pages;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Content;
using ProjectOnlineMobile2.Models.TLL;
using Newtonsoft.Json;
using Android.Content.PM;

namespace ProjectOnlineMobile2.Droid
{
    [Activity(Label = "@string/app_name",
              MainLauncher = false,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity
    {

        BottomNavigationView bottomNavigation;
        IMenu menu;
        Toolbar toolbar;

        DialogHelper dialogHelper;

        Fragment _homepageFragment, _projectsFragment, _tasksFragment, _timesheetFragment, _timesheetWorkFragment;

        TimesheetWorkPage _timesheetWork;

        public string UserName, UserEmail, TimesheetPeriod;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);

            Forms.Init(this, bundle);
            _homepageFragment = new HomePage().CreateSupportFragment(this);
            _timesheetWork = new TimesheetWorkPage();

            dialogHelper = new DialogHelper(this);

            MessagingCenter.Instance.Subscribe<string[]>(this, "DisplayAlert", (s) =>
            {

                //s[0] = message
                //s[1] = affirm button message
                //s[2] = cancel button message
                //s[3] = identifier
                //s[4] = period id

                try
                {
                    if (s.Length > 2)
                    {
                        if (!string.IsNullOrEmpty(s[3]))
                        {
                            if (s[3].Equals("CreateTimesheet"))
                            {
                                dialogHelper.DisplayCreateTimesheetDialog(s[0], s[4], s[1], s[2]);
                            }
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, s[0], ToastLength.Short).Show();
                    }
                }
                catch(Exception e)
                {

                }

                
            });

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models.D_User>(this, "UserInfo", (user) => {
                UserName = user.Title;
                UserEmail = user.Email;
            });

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetPeriod", (tsp) => {
                TimesheetPeriod = tsp;
            });

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models.TLL.TimesheetLineResult>(this, "PushTimesheetWorkPage", (timesheetLine) => {
                PushTimesheetWorkPage(timesheetLine);
            });

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);

            }

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            LoadFragment(Resource.Id.menu_projects);
        }

        private void PushTimesheetWorkPage(TimesheetLineResult timesheetLine)
        {
            try
            {
                var convertedPage = JsonConvert.SerializeObject(_timesheetWork);

                Intent intent = new Intent(this, typeof(TimesheetWorkActivity));
                intent.PutExtra("TASK_NAME", timesheetLine.TaskName);
                intent.PutExtra("LINE_ID", timesheetLine.Id);
                intent.PutExtra("WORK_PAGE", convertedPage);
                StartActivity(intent);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("PushTimesheetWorkPage", e.Message);
            }
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        void LoadFragment(int id)
        {
            Fragment fragment = null;

            if(id == Resource.Id.menu_projects)
            {
                if (_projectsFragment == null)
                    _projectsFragment = new ProjectPage().CreateSupportFragment(this);

                fragment = _projectsFragment;

                if(menu != null)
                {
                    menu.Clear();
                    MenuInflater.Inflate(Resource.Menu.projects_menu, menu);
                }
            }
            else if(id == Resource.Id.menu_tasks)
            {
                if (_tasksFragment == null)
                    _tasksFragment = new TasksPage().CreateSupportFragment(this);

                fragment = _tasksFragment;

                if (menu != null)
                {
                    menu.Clear();
                    MenuInflater.Inflate(Resource.Menu.tasks_menu, menu);
                }
            }
            else if(id == Resource.Id.menu_timesheets)
            {
                if (_timesheetFragment == null)
                    _timesheetFragment = new TimesheetPage().CreateSupportFragment(this);

                fragment = _timesheetFragment;

                if (menu != null)
                {
                    menu.Clear();
                    MenuInflater.Inflate(Resource.Menu.timesheet_menu, menu);
                }
            }
            
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.projects_menu, menu);
            this.menu = menu;
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            if(item.ItemId == Resource.Id.menu_userinfo)
            {
                dialogHelper.DisplayUserInfo(UserName, UserEmail);
            }
            else if(item.ItemId == Resource.Id.menu_period_details)
            {
                dialogHelper.DisplayPeriodDetails(TimesheetPeriod);
            }
            else if(item.ItemId == Resource.Id.menu_submit_timesheet)
            {
                dialogHelper.DisplaySubmitTimesheetDialog();
            }
            else if(item.ItemId == Resource.Id.menu_recall_timesheet)
            {
                MessagingCenter.Instance.Send<String>("", "RecallTimesheet");
            }
            

            return true;
        }

    }
}

