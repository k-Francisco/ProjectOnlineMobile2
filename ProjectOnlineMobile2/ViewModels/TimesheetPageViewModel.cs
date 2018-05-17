using LineResult = ProjectOnlineMobile2.Models.TLL.TimesheetLineResult;
using TimesheetPeriodsResult = ProjectOnlineMobile2.Models.TSPL.TimesheetPeriodResult;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectOnlineMobile2.Models;

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

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public string periodId, lineId;
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand TimesheetLineClicked { get; set; }
        public ICommand RefreshLinesCommand { get; set; }

        public TimesheetPageViewModel()
        {
            PeriodList = new ObservableCollection<TimesheetPeriodsResult>();
            PeriodLines = new ObservableCollection<LineResult>();

            SelectedItemChangedCommand = new Command(ExecuteSelectedItemChangedCommand);
            TimesheetLineClicked = new Command<LineResult>(ExecuteTimesheetLineClicked);
            RefreshLinesCommand = new Command(ExecuteRefreshLinesCommand);

            var savedPeriods = realm.All<TimesheetPeriodsResult>().ToList();
            foreach (var item in savedPeriods)
            {
                PeriodList.Add(item);
            }
            FindTodaysPeriod();

            SyncTimesheetPeriods(savedPeriods);

            MessagingCenter.Instance.Subscribe<String>(this, "CreateTimesheet", (periodId) =>
            {
                CreateTimesheet(periodId);
            });

            //for android 
            MessagingCenter.Instance.Subscribe<String>(this, "SendTimesheetIds", (s) =>{
                string[] ids = { periodId, lineId};
                MessagingCenter.Send<String[]>(ids, "TimesheetWork");
            });

            MessagingCenter.Instance.Subscribe<String>(this, "SubmitTimesheet", (comment)=> {
                ExecuteSubmitTimesheet(comment);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "RecallTimesheet", (s) => {
                ExecuteRecallTimesheet();
            });

        }

        private void ExecuteRefreshLinesCommand()
        {
            try
            {
                IsRefreshing = true;

                var savedLines = realm.All<SavedLinesModel>().Where(p => p.PeriodId == periodId);

                if (IsConnectedToInternet())
                {
                    realm.Write(()=> {
                        realm.RemoveRange<SavedLinesModel>(savedLines);
                    });
                    PeriodLines.Clear();

                    realm.Refresh();

                    SyncTimesheetLines(savedLines.ToList());
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
                    var formDigest = await SPapi.GetFormDigest();

                    var recall = await PSapi.RecallTimesheet(PeriodList[SelectedIndex].Id, formDigest.D.GetContextWebInformation.FormDigestValue);

                    if (recall)
                    {
                        string[] alertStrings = { "Successfully recalled timesheet", "Close" };
                        MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
                    }
                    else
                    {
                        string[] alertStrings = { "There was an error recalling the timesheet", "Close" };
                        MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
                    }
                }
                else
                {
                    string[] alertStrings = { "The device is not connected to the internet", "Close" };
                    MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("ExecuteRecallTimesheet", e.Message);
                string[] alertStrings = { "There was an error recalling the timesheet", "Close" };
                MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
            }
        }

        private async void ExecuteSubmitTimesheet(string comment)
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    var formDigest = await SPapi.GetFormDigest();

                    var submit = await PSapi.SubmitTimesheet(PeriodList[SelectedIndex].Id, comment, formDigest.D.GetContextWebInformation.FormDigestValue);

                    if (submit)
                    {
                        string[] alertStrings = { "Successfully submitted the timesheet", "Close" };
                        MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
                    }
                    else
                    {
                        string[] alertStrings = { "There was a problem submitting the timesheet", "Close" };
                        MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteSubmitTimesheet", e.Message);
                string[] alertStrings = { "There was a problem submitting the timesheet", "Close" };
                MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
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
                    string[] alertStrings = { "There was an error creating the timesheet", "Close"};
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
            periodId = _periodList[SelectedIndex].Id;
            lineId = timesheetLine.Id;
            string[] ids = { _periodList[SelectedIndex].Id, timesheetLine.Id };
            MessagingCenter.Send<LineResult>(timesheetLine, "PushTimesheetWorkPage");
            MessagingCenter.Send<String[]>(ids, "TimesheetWork");

        }

        private void ExecuteSelectedItemChangedCommand()
        {
            var periodId = PeriodList[SelectedIndex].Id;
            PeriodLines.Clear();
            if (IsConnectedToInternet())
            {
                ExecuteRefreshLinesCommand();
            }
            else
            {
                var savedLines = realm.All<SavedLinesModel>().Where(p => p.PeriodId == periodId).ToList();
                foreach (var item in savedLines)
                {
                    PeriodLines.Add(item.LineModel);
                }
            }
                

            MessagingCenter.Instance.Send<String>(PeriodList[SelectedIndex].ToString(), "TimesheetPeriod");
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
                    IsRefreshing = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("SyncTimesheetLines", e.Message);
                IsRefreshing = false;
            }
        }
    }
}
