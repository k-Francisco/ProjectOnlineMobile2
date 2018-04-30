using System;
using LineWorkResult = ProjectOnlineMobile2.Models.TLWM.Result;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Diagnostics;
using System.Windows.Input;
using ProjectOnlineMobile2.Models.TLWM;
using ProjectOnlineMobile2.Models;

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

        private ObservableCollection<LineWorkChangesModel> _lineWorkChanges;
        public ObservableCollection<LineWorkChangesModel> LineWorkChanges
        {
            get { return _lineWorkChanges; }
            set { SetProperty(ref _lineWorkChanges, value); }
        }

        private string _periodId { get; set; }
        private string _lineId { get; set; }

        public ICommand WorkTextChanged { get; set; }

        public TimesheetWorkPageViewModel()
        {
            LineWork = new ObservableCollection<LineWorkResult>();
            LineWorkChanges = new ObservableCollection<LineWorkChangesModel>();
            WorkTextChanged = new Command<LineWorkResult>(ExecuteWorkTextChanged);

            MessagingCenter.Instance.Subscribe<String[]>(this, "TimesheetWork", (ids) => {
                _periodId = ids[0];
                _lineId = ids[1];
                ExecuteGetTimesheetLineWork();
            });

            MessagingCenter.Instance.Subscribe<String>(this, "SaveTimesheetWorkChanges", (s) => {
                ExecuteSaveTimesheetWorkChanges();
            });

        }

        private async void ExecuteSaveTimesheetWorkChanges()
        {
            if (IsConnectedToInternet())
            {
                var formDigest = await SPapi.GetFormDigest();

                foreach (var item in LineWorkChanges)
                {
                    var body = "{'parameters':{'ActualWork':'" + item.ActualHours + "', " +
                        "'PlannedWork':'" + item.PlannedHours + "', " +
                        "'Start':'" + item.StartDate + "', " +
                        "'NonBillableOvertimeWork':'0h', " +
                        "'NonBillableWork':'0h', " +
                        "'OvertimeWork':'0h'}}";
                    var response = await PSapi.AddTimesheetLineWork(_periodId, _lineId, body, formDigest.D.GetContextWebInformation.FormDigestValue);
                    Debug.WriteLine("ExecuteSaveTimesheetWorkChanges", response);
                }
            }
            else
            {
                //put the saved changes in the database
            }
        }

        private void ExecuteWorkTextChanged(LineWorkResult obj)
        {
            if(LineWorkChanges.Count != 0)
            {
                var tempCollection = LineWorkChanges;
                bool isDateModified = false;
                for (int i = 0; i < tempCollection.Count; i++)
                {
                    if (DateTime.Compare(obj.Start, tempCollection[i].StartDate) == 0)
                    {
                        if (string.IsNullOrWhiteSpace(obj.EntryTextActualHours))
                            LineWorkChanges[i].ActualHours = obj.ActualWork;
                        else
                            LineWorkChanges[i].ActualHours = obj.EntryTextActualHours;

                        if(string.IsNullOrWhiteSpace(obj.EntryTextPlannedHours))
                            LineWorkChanges[i].PlannedHours = obj.PlannedWork;
                        else
                            LineWorkChanges[i].PlannedHours = obj.EntryTextPlannedHours;

                        if (string.IsNullOrWhiteSpace(obj.EntryTextActualHours) && string.IsNullOrWhiteSpace(obj.EntryTextPlannedHours))
                            LineWorkChanges.RemoveAt(i);

                        isDateModified = true;
                    }
                }

                if(!isDateModified)
                    LineWorkChanges.Add(new LineWorkChangesModel(obj.Start, obj.EntryTextActualHours, obj.EntryTextPlannedHours));
            }
            else
            {
                LineWorkChanges.Add(new LineWorkChangesModel(obj.Start, obj.EntryTextActualHours, obj.EntryTextPlannedHours));
            }

        }

        private async void ExecuteGetTimesheetLineWork()
        {
            var workHours = await PSapi.GetTimesheetLineWork(_periodId, _lineId);
            foreach (var item in workHours.D.Results)
            {
                LineWork.Add(item);
            }
        }
    }
}
