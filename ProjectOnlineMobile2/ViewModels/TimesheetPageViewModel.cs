using LineResult = ProjectOnlineMobile2.Models.TLL.Result;
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
        private ObservableCollection<String> _timesheetPeriods;
        public ObservableCollection<String> TimesheetPeriods
        {
            get { return _timesheetPeriods; }
            set { SetProperty(ref _timesheetPeriods, value); }
        }

        private ObservableCollection<LineResult> _periodLines;
        public ObservableCollection<LineResult> PeriodLines
        {
            get { return _periodLines; }
            set { SetProperty(ref _periodLines, value); }
        }

        public int SelectedIndex { get; set; }

        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand TimesheetLineClicked { get; set; }

        public TimesheetPageViewModel()
        {
            TimesheetPeriods = new ObservableCollection<String>();
            PeriodLines = new ObservableCollection<LineResult>();
            SelectedItemChangedCommand = new Command(ExecuteSelectedItemChangedCommand);
            TimesheetLineClicked = new Command<LineResult>(ExecuteTimesheetLineClicked);

            MessagingCenter.Instance.Subscribe<String>(this,"CreateTimesheet", (periodId) => {
                CreateTimesheet(periodId);
            });

            if (NetStandardSingleton.Instance.periods.Count is 0)
            {
                GetTimesheetPeriod();
            }
            else
            {
                AddPeriods();
            }
            
        }

        private void ExecuteTimesheetLineClicked(LineResult timesheetLine)
        {
            string[] ids = { NetStandardSingleton.Instance.periods[SelectedIndex].Id, timesheetLine.Id };
            MessagingCenter.Send<LineResult>(timesheetLine, "PushTimesheetWorkPage");
            MessagingCenter.Send<String[]>(ids, "TimesheetWork");
        }

        private async void CreateTimesheet(string periodId)
        {
            try
            {
                var webContextInfo = await SPapi.GetFormDigest();
                Debug.WriteLine("FormDigest", webContextInfo.D.GetContextWebInformation.FormDigestValue);
                var createTimesheet = await PSapi.CreateTimesheet(periodId, webContextInfo.D.GetContextWebInformation.FormDigestValue);
                Debug.WriteLine("CreateTimesheet", createTimesheet);
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateTimesheetFromViewModel", e.Message);
            }
        }

        private async void ExecuteSelectedItemChangedCommand()
        {
            try
            {
                PeriodLines.Clear();
                var lines = await PSapi.GetTimesheetLinesByPeriod(NetStandardSingleton.Instance.periods[SelectedIndex].Id);
                foreach (var item in lines.D.Results)
                {
                    PeriodLines.Add(item);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("ExecuteSelectedItemChangedCommand", e.Message);
            }
        }

        private async void GetTimesheetPeriod()
        {
            var timesheetperiod = await PSapi.GetAllTimesheetPeriods();
            foreach (var item in timesheetperiod.D.Results)
            {
                NetStandardSingleton.Instance.periods.Add(item);
            }

            AddPeriods();
        }

        private void AddPeriods()
        {
            foreach (var item in NetStandardSingleton.Instance.periods)
            {
                TimesheetPeriods.Add(item.Name + " ( " + item.Start.ToShortDateString() + "-" + item.End.ToShortDateString() + " )");
            }

            for (int i = 0; i < NetStandardSingleton.Instance.periods.Count; i++)
            {
                if (DateTime.Compare(DateTime.Now, NetStandardSingleton.Instance.periods[i].Start) == 0)
                {
                    SelectedIndex = i;
                    ExecuteSelectedItemChangedCommand();
                    break;
                }
                else if (DateTime.Compare(DateTime.Now, NetStandardSingleton.Instance.periods[i].Start) > 0 &&
                        DateTime.Compare(DateTime.Now, NetStandardSingleton.Instance.periods[i].End) < 0)
                {
                    SelectedIndex = i;
                    ExecuteSelectedItemChangedCommand();
                    break;
                }
            }
        }
    }
}
