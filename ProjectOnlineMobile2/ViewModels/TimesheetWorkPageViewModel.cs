using System;
using LineWorkResult = ProjectOnlineMobile2.Models.TLWM.Result;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Diagnostics;

namespace ProjectOnlineMobile2.ViewModels
{
    public class TimesheetWorkPageViewModel : BaseViewModel
    {
        private ObservableCollection<LineWorkResult> _lineWork;
        public ObservableCollection<LineWorkResult> LineWork
        {
            get { return _lineWork; }
            set { SetProperty(ref _lineWork, value); }
        }

        public TimesheetWorkPageViewModel()
        {
            LineWork = new ObservableCollection<LineWorkResult>();

            MessagingCenter.Instance.Subscribe<String[]>(this, "TimesheetWork", (ids) => {
                ExecuteGetTimesheetLineWork(ids);
            });

        }

        private async void ExecuteGetTimesheetLineWork(string[] ids)
        {
            var workHours = await PSapi.GetTimesheetLineWork(ids[0], ids[1]);
            foreach (var item in workHours.D.Results)
            {
                LineWork.Add(item);
            }
        }
    }
}
