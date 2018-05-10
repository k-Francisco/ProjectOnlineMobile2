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

        public IOrderedEnumerable<SavedTimesheetLineWork> savedLineWork { get; set; }

        public TimesheetWorkPageViewModel()
        {
            LineWork = new ObservableCollection<LineWorkResult>();
            LineWorkChanges = new List<LineWorkChangesModel>();

            MessagingCenter.Instance.Subscribe<String[]>(this, "TimesheetWork", (ids) =>
            {
                _periodId = ids[0];
                _lineId = ids[1];

                savedLineWork = realm.All<SavedTimesheetLineWork>()
                    .Where(p => p.PeriodId == _periodId && p.LineId == _lineId)
                    .ToList()
                    .OrderBy(p => p.WorkModel.Start.DateTime);

                foreach (var item in savedLineWork)
                {
                    LineWork.Add(item.WorkModel);
                }

                SyncTimesheetLineWork();
            });

            MessagingCenter.Instance.Subscribe<String>(this, "SaveTimesheetWorkChanges", (s) =>
            {
                ExecuteSaveTimesheetWorkChanges();
            });

            //for android
            MessagingCenter.Instance.Subscribe<String>(this, "Clear", (s) =>
            {
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

                    foreach (var item in savedLineWork)
                    {
                        if (!string.IsNullOrWhiteSpace(item.WorkModel.EntryTextActualHours) || !string.IsNullOrWhiteSpace(item.WorkModel.EntryTextPlannedHours))
                        {
                            string actualHours, plannedHours;

                            if (string.IsNullOrWhiteSpace(item.WorkModel.EntryTextActualHours))
                                actualHours = item.WorkModel.ActualWork;
                            else
                                actualHours = item.WorkModel.EntryTextActualHours + "h";

                            if (string.IsNullOrWhiteSpace(item.WorkModel.EntryTextPlannedHours))
                                plannedHours = item.WorkModel.PlannedWork;
                            else
                                plannedHours = item.WorkModel.EntryTextPlannedHours + "h";

                            var body = "{'parameters':{'ActualWork':'" + actualHours + "', " +
                            "'PlannedWork':'" + plannedHours + "', " +
                            "'Start':'" + item.WorkModel.Start.DateTime + "', " +
                            "'NonBillableOvertimeWork':'0h', " +
                            "'NonBillableWork':'0h', " +
                            "'OvertimeWork':'0h'}}";

                            var response = await PSapi.AddTimesheetLineWork(_periodId, _lineId, body, formDigest.D.GetContextWebInformation.FormDigestValue);

                            if (response)
                            {
                                realm.Write(()=> {
                                    item.WorkModel.ActualWork = actualHours;
                                    item.WorkModel.PlannedWork = plannedHours;
                                    item.WorkModel.EntryTextActualHours = "";
                                    item.WorkModel.EntryTextPlannedHours = "";
                                });
                                //MessagingCenter.Instance.Send<String>("Successfully saved work", "Toast");
                            }
                            else
                            {
                                Debug.WriteLine("ExecuteSaveTimesheetWorkChanges", "failed");
                                //MessagingCenter.Instance.Send<String>("Unable to save work due to an encountered problem. Please try again", "Toast");
                            }
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

        private async void SyncTimesheetLineWork()
        {
            try
            {
                var savedPeriod = realm.All<TimesheetPeriodResult>()
                    .Where(p => p.Id.Equals(_periodId))
                    .FirstOrDefault();

                List<LineWorkResult> tempDates = new List<LineWorkResult>();

                if (IsConnectedToInternet())
                {
                    var workHours = await PSapi.GetTimesheetLineWork(_periodId, _lineId);

                    if (!savedLineWork.Any())
                    {

                        for (int i = 1; i <= savedPeriod.End.Day - savedPeriod.Start.Day; i++)
                        {
                            tempDates.Add(new LineWorkResult()
                            {
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

                        foreach (var item in tempDates)
                        {
                            LineWork.Add(item);
                            realm.Write(() => {
                                realm.Add(new SavedTimesheetLineWork()
                                {
                                    PeriodId = _periodId,
                                    LineId = _lineId,
                                    WorkModel = item
                                });
                            });
                        }

                        realm.Refresh();
                    }
                    else
                    {
                        syncDataService.SyncTimesheetLineWork(workHours, savedLineWork);
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SyncTimesheetLineWork", e.Message);
            }
        }
    }
}
