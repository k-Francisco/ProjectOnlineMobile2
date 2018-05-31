using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimesheetWorkPage : ContentPage
	{
		public TimesheetWorkPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            MessagingCenter.Instance.Send<String>("", "WorkPagePushed");
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Instance.Send<String>("", "ClearEntries");
        }
    }
}