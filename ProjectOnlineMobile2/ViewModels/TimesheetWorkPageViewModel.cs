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

            MessagingCenter.Instance.Subscribe<String[]>(this, "TimesheetWork", (ids) =>
            {
                _periodId = ids[0];
                _lineId = ids[1];
                ExecuteGetTimesheetLineWork();
            });

            MessagingCenter.Instance.Subscribe<String>(this, "ClearRealm", (s) =>
            {
                realm.Write(() => {
                    realm.RemoveAll<SavedTimesheetLineWork>();
                });
            });

            MessagingCenter.Instance.Subscribe<String>(this, "SaveTimesheetWorkChanges", (s) =>
            {
                ExecuteSaveTimesheetWorkChanges();
            });

            //for android
            MessagingCenter.Instance.Subscribe<String>(this, "ClearCollection", (s)=> {
                LineWorkChanges.Clear();
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

        }

        private async void ExecuteGetTimesheetLineWork()
        {
            try
            {
                var savedLineWork = realm.All<SavedTimesheetLineWork>()
                    .Where(p => p.PeriodId == _periodId && p.LineId == _lineId)
                    .ToList()
                    .OrderBy(p => p.WorkModel.Start.DateTime);

                if (IsConnectedToInternet())
                {
                    var workHours = await PSapi.GetTimesheetLineWork(_periodId, _lineId);

                    if (!savedLineWork.Any())
                    {

                        foreach (var item in workHours.D.Results)
                        {

                            var save = new SavedTimesheetLineWork()
                            {
                                PeriodId = _periodId,
                                LineId = _lineId,
                                WorkModel = item
                            };
                            realm.Write(() =>
                            {
                                realm.Add(save);
                            });

                            LineWork.Add(item);
                        }

                    }
                    else
                    {
                        realm.Write(() => {
                            realm.RemoveAll<SavedTimesheetLineWork>();
                        });

                        foreach (var item in workHours.D.Results)
                        {
                            var save = new SavedTimesheetLineWork()
                            {
                                PeriodId = _periodId,
                                LineId = _lineId,
                                WorkModel = item
                            };
                            realm.Write(() => {
                                realm.Add(save);
                            });
                            LineWork.Add(save.WorkModel);
                        }
                    }

                }
                else
                {
                    foreach (var item in savedLineWork)
                    {
                        LineWork.Add(item.WorkModel);
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
