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
using ProjectOnlineMobile2.Models.TLL;

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
            var savedPeriods = realm.All<TimesheetPeriodsResult>().ToList();

            try
            {
                if (IsConnectedToInternet())
                {
                    var periods = await PSapi.GetAllTimesheetPeriods();

                    var isTheSame = savedPeriods.SequenceEqual(periods.D.Results);

                    if (isTheSame)
                    {
                        foreach (var item in savedPeriods)
                        {
                            PeriodList.Add(item);
                        }
                    }
                    else
                    {
                        realm.Write(() => {
                            realm.RemoveAll<TimesheetPeriodsResult>();
                        });

                        foreach (var item in periods.D.Results)
                        {
                            realm.Write(() => {
                                realm.Add(item);
                            });

                            PeriodList.Add(item);
                        }
                    }

                }
                else
                {
                    foreach (var item in savedPeriods)
                    {
                        PeriodList.Add(item);
                    }
                }

                if (PeriodList.Any())
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
            }
            catch(Exception e)
            {
                Debug.WriteLine("GetTimesheetPeriods",e.Message);
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
            PeriodLines.Clear();

            var savedLines = realm.All<SavedLinesModel>().Where(p => p.periodId == PeriodList[SelectedIndex].Id);

            var isTheSame = true;

            try
            {
                if (IsConnectedToInternet())
                {
                    var lines = await PSapi.GetTimesheetLinesByPeriod(PeriodList[SelectedIndex].Id);

                    if (!savedLines.Any())
                    {
                        realm.Write(()=> {
                            foreach (var item in lines.D.Results)
                            {
                                realm.Add(new SavedLinesModel() {
                                    periodId = PeriodList[SelectedIndex].Id,
                                    line = item
                                });
                                PeriodLines.Add(item);
                            }
                        });
                    }
                    else
                    {
                        var newSavedLines = realm.All<SavedLinesModel>();
                        foreach (var item in newSavedLines)
                        {
                            if (item.periodId.Equals(PeriodList[SelectedIndex].Id))
                            {
                                foreach (var item2 in lines.D.Results)
                                {
                                    if (!item.line.Equals(item2))
                                    {
                                        isTheSame = false;
                                        break;
                                    }
                                }
                                if (!isTheSame)
                                    break;
                            }
                        }

                        if (isTheSame)
                        {
                            foreach (var item in newSavedLines)
                            {
                                PeriodLines.Add(item.line);
                            }
                        }
                        else
                        {
                            foreach (var item in newSavedLines)
                            {
                                realm.Write(() =>
                                {
                                    realm.Remove(item);
                                });
                            }

                            foreach (var item in lines.D.Results)
                            {
                                realm.Write(() =>
                                {
                                    realm.Add(item);
                                });
                            }

                            realm.Refresh();

                            var newSavedLine2 = realm.All<SavedLinesModel>();
                            foreach (var item in newSavedLine2)
                            {
                                if(item.periodId.Equals(PeriodList[SelectedIndex].Id))
                                    PeriodLines.Add(item.line);
                            }
                        }

                    }

                }
                else
                {
                    var newSavedLines = realm.All<SavedLinesModel>();

                    foreach (var item in newSavedLines)
                    {
                        if (item.periodId.Equals(PeriodList[SelectedIndex].Id))
                        {
                            PeriodLines.Add(item.line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteSelectedItemChangedCommand", e.Message);
            }
        }

    }
}
