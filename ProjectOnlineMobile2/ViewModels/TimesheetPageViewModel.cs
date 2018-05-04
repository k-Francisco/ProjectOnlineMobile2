using LineResult = ProjectOnlineMobile2.Models.TLL.Result;
using TimesheetPeriodsResult = ProjectOnlineMobile2.Models.TSPL.Result;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Collections.ObjectModel;
using System;
using ProjectOnlineMobile2.Services;
using System.Windows.Input;
using Xamarin.Forms;

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

            MessagingCenter.Instance.Subscribe<String>(this, "CreateTimesheet", (periodId) =>
            {
                CreateTimesheet(periodId);
            });

            //for android 
            MessagingCenter.Instance.Subscribe<String>(this, "SendTimesheetIds", (s) =>{
                string[] ids = { periodId, lineId};
                MessagingCenter.Send<String[]>(ids, "TimesheetWork");
            });

            GetTimesheetPeriods();
            
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

        private async void GetTimesheetPeriods()
        {
            if (IsConnectedToInternet())
            {
                //TODO: check if the collections in database and in the cloud are equal

                var periods = await PSapi.GetAllTimesheetPeriods();
                foreach (var item in periods.D.Results)
                {
                    PeriodList.Add(item);
                }

                for (int i = 0; i < PeriodList.Count; i++)
                {
                    if (DateTime.Compare(DateTime.Now, PeriodList[i].Start) >= 0 &&
                            DateTime.Compare(DateTime.Now, PeriodList[i].End) < 0)
                    {
                        SelectedIndex = i;
                        ExecuteSelectedItemChangedCommand();
                        break;
                    }
                }

            }
            else
            {
                //TODO: retrieve items in the db
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

        private async void ExecuteSelectedItemChangedCommand()
        {
            try
            {
                PeriodLines.Clear();
                var lines = await PSapi.GetTimesheetLinesByPeriod(PeriodList[SelectedIndex].Id);
                foreach (var item in lines.D.Results)
                {
                    PeriodLines.Add(item);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteSelectedItemChangedCommand", e.Message);
            }
        }

    }
}
