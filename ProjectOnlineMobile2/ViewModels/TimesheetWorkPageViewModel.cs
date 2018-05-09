using System;
using LineWorkResult = ProjectOnlineMobile2.Models.TLWM.WorkResult;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Diagnostics;
using System.Windows.Input;
using ProjectOnlineMobile2.Models.TLWM;
using ProjectOnlineMobile2.Models;
using ProjectOnlineMobile2.Services;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Linq;
using ProjectOnlineMobile2.Models.TSPL;

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

        private List<LineWorkChangesModel> _lineWorkChanges;
        public List<LineWorkChangesModel> LineWorkChanges
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
            LineWorkChanges = new List<LineWorkChangesModel>();
            WorkTextChanged = new Command<LineWorkResult>(ExecuteWorkTextChanged);

            LineWorkChanges.Clear();

            MessagingCenter.Instance.Subscribe<String[]>(this, "TimesheetWork", (ids) =>
            {
                _periodId = ids[0];
                _lineId = ids[1];
                ExecuteGetTimesheetLineWork();
            });

            MessagingCenter.Instance.Subscribe<String>(this, "SaveTimesheetWorkChanges", (s) =>
            {
                ExecuteSaveTimesheetWorkChanges();
            });

            //for android
            MessagingCenter.Instance.Subscribe<String>(this, "Clear", (s) =>
            {
                var savedLineWork = realm.All<SavedTimesheetLineWork>()
                    .Where(p => p.PeriodId == _periodId && p.LineId == _lineId)
                    .ToList();

                foreach (var item in savedLineWork)
                {
                    realm.Write(()=> {
                        item.WorkModel.EntryTextActualHours = string.Empty;
                        item.WorkModel.EntryTextPlannedHours = string.Empty;
                    });
                }
            });

        }

        private async void ExecuteSaveTimesheetWorkChanges()
        {
            try
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

                        var response = await PSapi.AddTimesheetLineWork(_periodId, _lineId, body, formDigest.D.GetContextWebInformation.FormDigestValue).ConfigureAwait(false);
                        
                        if (response)
                        {
                            MessagingCenter.Instance.Send<String>("Successfully saved work", "Toast");
                            LineWorkChanges.Clear();
                        }
                        else
                        {
                            MessagingCenter.Instance.Send<String>("Unable to save work due to an encountered problem. Please try again", "Toast");
                        }
                    }
                }
                else
                {
                    //TODO: save it to the db
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteSaveTimesheetWorkChanges", e.Message);
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
                    if (DateTime.Compare(obj.Start.DateTime, tempCollection[i].StartDate) == 0)
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

                if (!isDateModified)
                    LineWorkChanges.Add(new LineWorkChangesModel() {
                        StartDate = obj.Start.DateTime,
                        ActualHours = obj.EntryTextActualHours,
                        PlannedHours = obj.EntryTextPlannedHours,
                        PeriodId = _periodId,
                        LineId = _lineId,
                    });
            }
            else
            {
                LineWorkChanges.Add(new LineWorkChangesModel()
                {
                    StartDate = obj.Start.DateTime,
                    ActualHours = obj.EntryTextActualHours,
                    PlannedHours = obj.EntryTextPlannedHours,
                    PeriodId = _periodId,
                    LineId = _lineId,
                });
            }

            foreach (var item in LineWorkChanges)
            {
                Debug.WriteLine("",item.StartDate.Date.ToShortDateString() + " - " + item.ActualHours + "/" + item.PlannedHours + " -- " + item.PeriodId + "/" + item.LineId );
            }

        }

        private async void ExecuteGetTimesheetLineWork()
        {
            try
            {
                var savedPeriod = realm.All<TimesheetPeriodResult>()
                    .Where(p => p.Id.Equals(_periodId))
                    .FirstOrDefault();

                var savedLineWork = realm.All<SavedTimesheetLineWork>()
                    .Where(p => p.PeriodId == _periodId && p.LineId == _lineId)
                    .ToList()
                    .OrderBy(p => p.WorkModel.Start.DateTime);

                foreach (var item in savedLineWork)
                {
                    LineWork.Add(item.WorkModel);
                }

                List<LineWorkResult> tempDates = new List<LineWorkResult>();

                if (IsConnectedToInternet())
                {
                    var workHours = await PSapi.GetTimesheetLineWork(_periodId, _lineId);

                    for (int i = 1; i <= savedPeriod.End.Day - savedPeriod.Start.Day; i++)
                    {
                        tempDates.Add(new LineWorkResult() {
                            Start = savedPeriod.Start.AddDays(i).DateTime,
                            End = savedPeriod.Start.AddDays(i).DateTime,
                            ActualWork = "0h",
                            PlannedWork = "0h",
                        });
                    }

                    foreach (var item in workHours.D.Results)
                    {
                        foreach (var item2 in tempDates)
                        {
                            if (item.Start.DateTime.ToShortDateString().Equals(item2.Start.DateTime.ToShortDateString()))
                            {
                                item2.ActualWork = item.ActualWork;
                                item2.ActualWorkMilliseconds = item.ActualWorkMilliseconds;
                                item2.ActualWorkTimeSpan = item.ActualWorkTimeSpan;
                                item2.End = item.End;
                                item2.ActualWorkMilliseconds = item.ActualWorkMilliseconds;
                                item2.ActualWorkTimeSpan = item.ActualWorkTimeSpan;
                                item2.Comment = item.Comment;
                                item2.Id = item.Id;
                                item2.NonBillableOvertimeWork = item.NonBillableOvertimeWork;
                                item2.NonBillableOvertimeWorkMilliseconds = item.NonBillableOvertimeWorkMilliseconds;
                                item2.NonBillableOvertimeWorkTimeSpan = item.NonBillableOvertimeWorkTimeSpan;
                                item2.NonBillableWork = item.NonBillableWork;
                                item2.NonBillableWorkMilliseconds = item.NonBillableWorkMilliseconds;
                                item2.NonBillableWorkTimeSpan = item.NonBillableWorkTimeSpan;
                                item2.OvertimeWork = item.OvertimeWork;
                                item2.OvertimeWorkMilliseconds = item.OvertimeWorkMilliseconds;
                                item2.OvertimeWorkTimeSpan = item.OvertimeWorkTimeSpan;
                                item2.PlannedWork = item.PlannedWork;
                                item2.PlannedWorkMilliseconds = item.PlannedWorkMilliseconds;
                                item2.PlannedWorkTimeSpan = item.PlannedWorkTimeSpan;
                            }
                        }
                    }

                    if (!savedLineWork.Any())
                    {
                        foreach (var item in tempDates)
                        {
                            LineWork.Add(item);
                            realm.Write(()=> {
                                realm.Add(new SavedTimesheetLineWork() {
                                    PeriodId = _periodId,
                                    LineId = _lineId,
                                    WorkModel = item
                                });
                            });
                        }
                    }
                    else
                    {
                        foreach (var item in savedLineWork)
                        {
                            var temp = tempDates
                                .Where(p => p.Start.DateTime.ToShortDateString().Equals(item.WorkModel.Start.DateTime.ToShortDateString()))
                                .FirstOrDefault();

                            realm.Write(()=> {
                                item.WorkModel.ActualWork = temp.ActualWork;
                                item.WorkModel.ActualWorkMilliseconds = temp.ActualWorkMilliseconds;
                                item.WorkModel.ActualWorkTimeSpan = temp.ActualWorkTimeSpan;
                                item.WorkModel.End = temp.End;
                                item.WorkModel.ActualWorkMilliseconds = temp.ActualWorkMilliseconds;
                                item.WorkModel.ActualWorkTimeSpan = temp.ActualWorkTimeSpan;
                                item.WorkModel.Comment = temp.Comment;
                                item.WorkModel.Id = temp.Id;
                                item.WorkModel.NonBillableOvertimeWork = temp.NonBillableOvertimeWork;
                                item.WorkModel.NonBillableOvertimeWorkMilliseconds = temp.NonBillableOvertimeWorkMilliseconds;
                                item.WorkModel.NonBillableOvertimeWorkTimeSpan = temp.NonBillableOvertimeWorkTimeSpan;
                                item.WorkModel.NonBillableWork = temp.NonBillableWork;
                                item.WorkModel.NonBillableWorkMilliseconds = temp.NonBillableWorkMilliseconds;
                                item.WorkModel.NonBillableWorkTimeSpan = temp.NonBillableWorkTimeSpan;
                                item.WorkModel.OvertimeWork = temp.OvertimeWork;
                                item.WorkModel.OvertimeWorkMilliseconds = temp.OvertimeWorkMilliseconds;
                                item.WorkModel.OvertimeWorkTimeSpan = temp.OvertimeWorkTimeSpan;
                                item.WorkModel.PlannedWork = temp.PlannedWork;
                                item.WorkModel.PlannedWorkMilliseconds = temp.PlannedWorkMilliseconds;
                                item.WorkModel.PlannedWorkTimeSpan = temp.PlannedWorkTimeSpan;
                            });
                        }
                    }

                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("ExecuteGetTimesheetLineWork", e.Message);
            }
        }
    }
}
