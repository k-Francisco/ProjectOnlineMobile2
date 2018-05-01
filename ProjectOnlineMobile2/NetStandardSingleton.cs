using ProjectOnlineMobile2.Models.PSPL;
using ProjectResult = ProjectOnlineMobile2.Models.PSPL.Result;
using TasksResult = ProjectOnlineMobile2.Models.ResourceAssignmentModel.Result;
using TimesheetPeriodsResult = ProjectOnlineMobile2.Models.TSPL.Result;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using ProjectOnlineMobile2.Models.TLWM;

namespace ProjectOnlineMobile2
{
    public sealed class NetStandardSingleton
    {
        private static volatile NetStandardSingleton _instance;
        private static readonly object _syncLock = new object();

        private NetStandardSingleton() { }

        public static NetStandardSingleton Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (_syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new NetStandardSingleton();
                    }
                }

                return _instance;
            }
        }

        //public List<ProjectResult> projects = new List<ProjectResult>();
        //public ObservableCollection<TasksResult> userTasks = new ObservableCollection<TasksResult>();
       // public ObservableCollection<TimesheetPeriodsResult> periods = new ObservableCollection<TimesheetPeriodsResult>();

    }
}
