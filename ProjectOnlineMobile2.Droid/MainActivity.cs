using Android.App;
using Android.OS;

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
using Android.Views.InputMethods;

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

        public string UserName, UserEmail, TimesheetPeriod;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);

            Forms.Init(this, bundle);

            MessagingCenter.Instance.Subscribe<string[]>(this, "DisplayAlert", (s) =>
            {

                //s[0] = message
                //s[1] = affirm button message
                //s[2] = cancel button message
                //s[3] = identifier
                //s[4] = period id

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


            });

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models.D_User>(this, "UserInfo", (userInfo)=> {
                UserName = userInfo.Title;
                UserEmail = userInfo.Email;
            });

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetPeriod", (tsp) => {
                TimesheetPeriod = tsp;
            });

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models.TLL.TimesheetLineResult>(this, "PushTimesheetWorkPage", (timesheetLine) => {
                PushTimesheetWorkPage(timesheetLine);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "AddTimesheetLineDialog", (s)=> {
                dialogHelper.DisplayAddTimesheetLineDialog();
            });

            MessagingCenter.Instance.Subscribe<String>(this,"ExitWorkPage",(s)=> {
                exitWorkPage();
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

            _homepageFragment = new HomePage().CreateSupportFragment(this);
            _timesheetWorkFragment = new TimesheetWorkPage().CreateSupportFragment(this);

            dialogHelper = new DialogHelper(this);

            var backButton = FindViewById<ImageView>(Resource.Id.arrow_back);
            backButton.Click += (sender,args) => {
                exitWorkPage();
            };

            var saveButton = FindViewById<ImageView>(Resource.Id.save_work);
            saveButton.Click += (sender,args) => {
                MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
            };

            var deleteLineButton = FindViewById<ImageView>(Resource.Id.delete_line);
            deleteLineButton.Click += (sender, args) => {
                dialogHelper.DisplayConfirmationDialog("Do you really want to delete this line?","Delete","Cancel");
            };

            var editLineButton = FindViewById<ImageView>(Resource.Id.edit_line);

            LoadFragment(Resource.Id.menu_projects);
        }

        private void exitWorkPage()
        {
            toolbar.Visibility = ViewStates.Visible;
            bottomNavigation.Visibility = ViewStates.Visible;

            InputMethodManager imm = InputMethodManager.FromContext(this.ApplicationContext);
            imm.HideSoftInputFromInputMethod(this.Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        private void PushTimesheetWorkPage(TimesheetLineResult timesheetLine)
        {
            try
            {
                toolbar.Visibility = ViewStates.Gone;
                bottomNavigation.Visibility = ViewStates.Gone;

                SupportFragmentManager.BeginTransaction()
                    .Replace(Resource.Id.content_frame, _timesheetWorkFragment)
                    .Commit();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("PushTimesheetWorkPage", e.Message);
            }

        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        Fragment fragment = null;
        void LoadFragment(int id)
        {
            
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
            else if(item.ItemId == Resource.Id.menu_add_line)
            {
                MessagingCenter.Instance.Send<String>("", "OpenProjectPicker");
            }
            

            return true;
        }

    }
}

