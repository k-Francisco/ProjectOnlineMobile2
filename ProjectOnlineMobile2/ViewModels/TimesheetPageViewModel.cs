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

        public string periodId, lineId;
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand TimesheetLineClicked { get; set; }

        public TimesheetPageViewModel()
        {
            PeriodList = new ObservableCollection<TimesheetPeriodsResult>();
            PeriodLines = new ObservableCollection<LineResult>();

            SelectedItemChangedCommand = new Command(ExecuteSelectedItemChangedCommand);
            TimesheetLineClicked = new Command<LineResult>(ExecuteTimesheetLineClicked);

            var savedPeriods = realm.All<TimesheetPeriodsResult>().ToList();
            foreach (var item in savedPeriods)
            {
                PeriodList.Add(item);
            }
            FindTodaysPeriod();

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetPageInit", (s) => {
                SyncTimesheetPeriods(savedPeriods);
            });

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
                    //ExecuteSelectedItemChangedCommand();
                    break;
                }
            }
        }

        private async void ExecuteRecallTimesheet()
        {
            try
            {
                var formDigest = await SPapi.GetFormDigest();

                var recall = await PSapi.RecallTimesheet(PeriodList[SelectedIndex].Id, formDigest.D.GetContextWebInformation.FormDigestValue);

                if (recall)
                {
                    Debug.WriteLine("ExecuteRecallTimesheet", "success");
                    //show success 
                }
                else
                {
                    Debug.WriteLine("ExecuteRecallTimesheet", "failed");
                    //show failure
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("ExecuteRecallTimesheet", e.Message);
            }
        }

        private async void ExecuteSubmitTimesheet(string comment)
        {
            try
            {
                var formDigest = await SPapi.GetFormDigest();

                var submit = await PSapi.SubmitTimesheet(PeriodList[SelectedIndex].Id, comment, formDigest.D.GetContextWebInformation.FormDigestValue);

                if (submit)
                {
                    Debug.WriteLine("ExecuteSubmitTimesheet", "success");
                    //show success 
                }
                else
                {
                    Debug.WriteLine("ExecuteSubmitTimesheet", "failed");
                    //show failure
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteSubmitTimesheet", e.Message);
            }
        }

        private async void CreateTimesheet(string periodId)
        {
            if (IsConnectedToInternet())
            {
                var formDigest = await SPapi.GetFormDigest();

                if (await PSapi.CreateTimesheet(periodId, formDigest.D.GetContextWebInformation.FormDigestValue))
                {
                    ExecuteSelectedItemChangedCommand();
                }
                else
                {
                    //prompt user error
                }
            }
            else
            {
                //prompt user that the device is not connected to the internet
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
            PeriodLines.Clear();
            var periodId = PeriodList[SelectedIndex].Id;
            var savedLines = realm.All<SavedLinesModel>().Where(p => p.PeriodId == periodId).ToList();
            
            foreach (var item in savedLines)
            {
                PeriodLines.Add(item.LineModel);
            }

            SyncTimesheetLines(savedLines);
        }

        private async void SyncTimesheetLines(List<SavedLinesModel> savedLines)
        {
            if (IsConnectedToInternet())
            {
                var periodLines = await PSapi.GetTimesheetLinesByPeriod(PeriodList[SelectedIndex].Id);
                syncDataService.SyncTimesheetLines(savedLines, periodLines.D.Results, PeriodLines, PeriodList[SelectedIndex].Id);
            }
        }
    }
}
