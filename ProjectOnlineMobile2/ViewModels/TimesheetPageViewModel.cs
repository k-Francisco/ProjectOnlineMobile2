using Result = ProjectOnlineMobile2.Models.TSPL.Result;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Collections.ObjectModel;
using System;
using ProjectOnlineMobile2.Services;

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

        public TimesheetPageViewModel()
        {
            TimesheetPeriods = new ObservableCollection<String>();

            if(NetStandardSingleton.Instance.periods.Count is 0)
            {
                GetTimesheetPeriod();
            }
            else
            {
                foreach (var item in NetStandardSingleton.Instance.periods)
                {
                    TimesheetPeriods.Add(item.Name + " ( "+item.Start.ToShortDateString()+ "-"+ item.End.ToShortDateString()+" )");
                }
            }
        }

        private async void GetTimesheetPeriod()
        {
            var timesheetperiod = await PSapi.GetAllTimesheetPeriods();
            foreach (var item in timesheetperiod.D.Results)
            {
                NetStandardSingleton.Instance.periods.Add(item);
                TimesheetPeriods.Add(item.Name + " ( " + item.Start.ToShortDateString() + "-" + item.End.ToShortDateString() + " )");
            }
        }
    }
}
