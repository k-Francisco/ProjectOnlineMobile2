using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimesheetPage : ContentPage
	{
		public TimesheetPage ()
		{
			InitializeComponent ();

            MessagingCenter.Instance.Subscribe<String>(this,"OpenPeriodPicker",(s)=> {
                periodPicker.Focus();
            });

            MessagingCenter.Instance.Subscribe<String>(this, "OpenProjectPicker", (s)=> {
                projectPicker.Focus();
            });

            MessagingCenter.Instance.Subscribe<String>(this, "CloseProjectPicker", (s)=> {
                projectPicker.Unfocus();
            });

		}
    }
}