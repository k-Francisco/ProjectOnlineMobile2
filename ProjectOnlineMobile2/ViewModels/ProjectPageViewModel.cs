using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectOnlineMobile2.ViewModels
{
    public class ProjectPageViewModel
    {
        public ObservableCollection<String> Sample { get; set; }

        public ProjectPageViewModel()
        {
            Sample = new ObservableCollection<string>();
            Sample.Add("");
            Sample.Add("");
        }
    }
}
