using LineResult = ProjectOnlineMobile2.Models.TLL.TimesheetLineResult;
using TimesheetPeriodsResult = ProjectOnlineMobile2.Models.TSPL.TimesheetPeriodResult;
using ProjectResult = ProjectOnlineMobile2.Models.PSPL.Result;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectOnlineMobile2.Models;
using System.Threading;

namespace ProjectOnlineMobile2.ViewModels
{
    public class TimesheetPageViewModel : BaseViewModel
    {

        private ObservableCollection<TimesheetPeriodsResult> _periodList;
        public ObservableCollection<TimesheetPeriodsResult> PeriodList
        {
            get { return _periodList; }
            set { SetProperty(ref _periodList, value); }
        }


        private ObservableCollection<LineResult> _periodLines;
        public ObservableCollection<LineResult> PeriodLines
        {
            get { return _periodLines; }
            set { SetProperty(ref _periodLines, value); }
        }

        private ObservableCollection<String> _projectsAssigned;
        public ObservableCollection<String> ProjectsAssigned
        {
            get { return _projectsAssigned; }
            set { SetProperty(ref _projectsAssigned, value); }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        private int _selectedProject;
        public int SelectedProject
        {
            get { return _selectedProject; }
            set { SetProperty(ref _selectedProject, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        private bool _openPicker = false;
        public bool OpenPicker
        {
            get { return _openPicker; }
            set { SetProperty(ref _openPicker, value); }
        }

        private IQueryable<SavedLinesModel> _savedLines { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand SelectedProjectChangedCommand { get; set; }
        public ICommand TimesheetLineClicked { get; set; }
        public ICommand RefreshLinesCommand { get; set; }

        public string periodId, lineId;

        public TimesheetPageViewModel()
        {

            MessagingCenter.Instance.Subscribe<String>(this, "CreateTimesheet", (periodId) =>
            {
                CreateTimesheet(periodId);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "SubmitTimesheet", (comment) => {
                ExecuteSubmitTimesheet(comment);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "RecallTimesheet", (s) => {
                ExecuteRecallTimesheet();
            });

            MessagingCenter.Instance.Subscribe<String[]>(this, "AddTimesheetLine", (s) => {
                //first index in the string array is task name
                //second index in the string array is comment

                ExecuteAddTimesheetLine(s[0], s[1]);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "RefreshTimesheetLines", (s)=> {
                ExecuteRefreshLinesCommand();
            });

            MessagingCenter.Instance.Subscribe<String>(this, "AddAssignedProjects", (s)=> {
                ExecuteAddAssignedProjects();
            });

            PeriodList = new ObservableCollection<TimesheetPeriodsResult>();
            PeriodLines = new ObservableCollection<LineResult>();
            ProjectsAssigned = new ObservableCollection<string>();

            SelectedItemChangedCommand = new Command(ExecuteSelectedItemChangedCommand);
            SelectedProjectChangedCommand = new Command(ExecuteSelectedProjectChangedCommand);
            TimesheetLineClicked = new Command<LineResult>(ExecuteTimesheetLineClicked);
            RefreshLinesCommand = new Command(ExecuteRefreshLinesCommand);

            var savedPeriods = realm.All<TimesheetPeriodsResult>().ToList();
            foreach (var item in savedPeriods)
            {
                PeriodList.Add(item);
            }

            FindTodaysPeriod();

            SyncTimesheetPeriods(savedPeriods);

        }

        private void ExecuteAddAssignedProjects()
        {
            ProjectsAssigned.Add("Personal Task");

            var savedProjects = realm.All<ProjectResult>().ToList();
            foreach (var item in savedProjects)
            {
                if (item.IsUserAssignedToThisProject)
                    ProjectsAssigned.Add(item.ProjectName);
            }
        }

        private async void ExecuteAddTimesheetLine(string taskName, string comment)
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    MessagingCenter.Instance.Send<String[]>(new string[] { "Adding timesheet line...", "Close" }, "DisplayAlert");

                    string body = "";

                    var project = realm.All<ProjectResult>()
                                   .Where(p => p.ProjectName.Equals(ProjectsAssigned[SelectedProject]))
                                   .FirstOrDefault();

                    var formDigest = await SPapi.GetFormDigest();

                    if (project != null)
                    {
                        body = "{'parameters':" +
                            "{'TaskName':'" + taskName + "', " +
                            "'Comment':'" + comment + "', " +
                            "'ProjectId':'" + project.ProjectId + "'}}";
                    }
                    else
                    {
                        body = "{'parameters':{'TaskName':'" + taskName + "', " +
                            "'Comment':'" + comment + "'}}";
                    }

                    var addLine = await PSapi.AddTimesheetLine(PeriodList[SelectedIndex].Id, body, formDigest.D.GetContextWebInformation.FormDigestValue);
                    if (addLine)
                    {
                        string[] alertStrings = { "Successfully added line", "Close" };
                        MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");

                        ExecuteRefreshLinesCommand();
                    }
                    else
                    {
                        string[] alertStrings = { "There was an error adding the line. Please try again", "Close" };
                        MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
                    }
                }
                else
                {
                    MessagingCenter.Instance.Send<String[]>(new string[] { "Your device is not connected to the internet", "Close" }, "DisplayAlert");
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteAddTimesheetLine", e.Message);
                string[] alertStrings = { "There was an error adding the line. Please try again", "Close" };
                MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
            }
        }

        private void ExecuteSelectedProjectChangedCommand()
        {
            
            MessagingCenter.Instance.Send<String>("", "CloseProjectPicker");
            MessagingCenter.Instance.Send<String>("", "AddTimesheetLineDialog");
        }

        private void ExecuteRefreshLinesCommand()
        {
            try
            {
                IsRefreshing = true;

                if (IsConnectedToInternet())
                {
                    realm.Write(()=> {
                        realm.RemoveRange<SavedLinesModel>(_savedLines);
                    });
                    PeriodLines.Clear();

                    realm.Refresh();

                    SyncTimesheetLines(_savedLines.ToList());
                }
                else
                    IsRefreshing = false;
                
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteRefreshLinesCommand", e.Message);
                IsRefreshing = false;
            }
        }

        private async void SyncTimesheetPeriods(List<TimesheetPeriodsResult> savedPeriods)
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    var periods = await PSapi.GetAllTimesheetPeriods();
                    syncDataService.SyncTimesheetPeriods(savedPeriods, periods.D.Results, PeriodList);
                    if(SelectedIndex <= 0)
                    {
                        FindTodaysPeriod();
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncTimesheetPeriods", e.Message);
            }
        }

        private void FindTodaysPeriod()
        {
            for (int i = 0; i < PeriodList.Count; i++)
            {
                if (DateTime.Compare(DateTime.Now, PeriodList[i].Start.DateTime) >= 0 &&
                        DateTime.Compare(DateTime.Now, PeriodList[i].End.DateTime) < 0)
                {
                    SelectedIndex = i;
                    ExecuteSelectedItemChangedCommand();
                    break;
                }
            }
        }

        private async void ExecuteRecallTimesheet()
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    MessagingCenter.Instance.Send<String[]>(new string[] { "Recalling timesheet...","Close"}, "DisplayAlert");
                    var formDigest = await SPapi.GetFormDigest();

                    var recall = await PSapi.RecallTimesheet(PeriodList[SelectedIndex].Id, formDigest.D.GetContextWebInformation.FormDigestValue);

                    if (recall)
                    {
                        MessagingCenter.Instance.Send<String[]>(new string[] { "Successfully recalled timesheet", "Close" }, "DisplayAlert");
                        ExecuteRefreshLinesCommand();
                        GetTimesheetStatus();
                    }
                    else
                    {
                        MessagingCenter.Instance.Send<String[]>(new string[] { "There was an error recalling the timesheet. Please try again", "Close" }, "DisplayAlert");
                    }
                }
                else
                {
                    MessagingCenter.Instance.Send<String[]>(new string[] { "The device is not connected to the internet", "Close" }, "DisplayAlert");
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("ExecuteRecallTimesheet", e.Message);
                MessagingCenter.Instance.Send<String[]>(new string[] { "There was an error recalling the timesheet. Please try again", "Close" }, "DisplayAlert");
            }
        }

        private async void ExecuteSubmitTimesheet(string comment)
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    MessagingCenter.Instance.Send<String[]>(new string[] { "Submitting timesheet...","Close"}, "DisplayAlert");
                    var formDigest = await SPapi.GetFormDigest();

                    var submit = await PSapi.SubmitTimesheet(PeriodList[SelectedIndex].Id, comment, formDigest.D.GetContextWebInformation.FormDigestValue);

                    if (submit)
                    {
                        MessagingCenter.Instance.Send<String[]>(new string[] { "Successfully submitted the timesheet", "Close" }, "DisplayAlert");
                        ExecuteRefreshLinesCommand();
                        GetTimesheetStatus();
                    }
                    else
                    {
                        MessagingCenter.Instance.Send<String[]>(new string[] { "There was a problem submitting the timesheet. Please try again", "Close" }, "DisplayAlert");
                    }
                }
                else
                {
                    MessagingCenter.Instance.Send<String[]>(new string[] { "Your device is not connected to the internet", "Close" }, "DisplayAlert");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteSubmitTimesheet", e.Message);
                MessagingCenter.Instance.Send<String[]>(new string[] { "There was a problem submitting the timesheet. Please try again", "Close" }, "DisplayAlert");
            }
        }

        private async void CreateTimesheet(string periodId)
        {
            if (IsConnectedToInternet())
            {
                var formDigest = await SPapi.GetFormDigest();

                if (await PSapi.CreateTimesheet(periodId, formDigest.D.GetContextWebInformation.FormDigestValue))
                {
                    ExecuteRefreshLinesCommand();
                }
                else
                {
                    string[] alertStrings = { "There was an error creating the timesheet. Please try again", "Close"};
                    MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
                }
            }
            else
            {
                string[] alertStrings = { "The device is not connected to the internet", "Close" };
                MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
            }
        }

        private void ExecuteTimesheetLineClicked(LineResult timesheetLine)
        {
            lineId = timesheetLine.Id;
            string[] ids = { _periodList[SelectedIndex].Id, timesheetLine.Id };
            MessagingCenter.Send<LineResult>(timesheetLine, "PushTimesheetWorkPage");
            MessagingCenter.Send<String[]>(ids, "TimesheetWork");

        }

        private void ExecuteSelectedItemChangedCommand()
        {
            var periodId = PeriodList[SelectedIndex].Id;
            _savedLines = realm.All<SavedLinesModel>().Where(p => p.PeriodId == periodId);

            GetTimesheetStatus();

            PeriodLines.Clear();
            if (IsConnectedToInternet())
            {
                ExecuteRefreshLinesCommand();
            }
            else
            {
                foreach (var item in _savedLines.ToList())
                {
                    PeriodLines.Add(item.LineModel);
                }
            }

            MessagingCenter.Instance.Send<String>(PeriodList[SelectedIndex].ToString(), "TimesheetPeriod");
        }

        private async void GetTimesheetStatus()
        {
            try
            {
                var timesheetModel = await PSapi.GetTimesheet(PeriodList[SelectedIndex].Id);
                MessagingCenter.Instance.Send<String>(timesheetModel.D.Status.ToString(), "TimesheetStatus");
            }
            catch(Exception e)
            {
                Debug.WriteLine("GetTimesheetStatus", e.Message);
            }
        }

        private async void SyncTimesheetLines(List<SavedLinesModel> savedLines)
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    var periodLines = await PSapi.GetTimesheetLinesByPeriod(PeriodList[SelectedIndex].Id);
                    syncDataService.SyncTimesheetLines(savedLines, periodLines.D.Results, PeriodLines, PeriodList[SelectedIndex].Id);
                    IsRefreshing = false;
                }
                else
                {
                    IsRefreshing = false;
                    string[] alertStrings = { "Your device is not connected to the internet", "Close" };
                    MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SyncTimesheetLines", e.Message);
                IsRefreshing = false;
            }
        }
    }
}
