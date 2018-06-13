using ProjectOnlineMobile2.ViewModels;
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
	public partial class TasksPage : ContentPage
	{
        private bool didAppear;
        private TasksPageViewModel viewModel;

		public TasksPage ()
		{
			InitializeComponent ();
            viewModel = this.BindingContext as TasksPageViewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!didAppear)
            {
                MessagingCenter.Instance.Send<String>("", "SyncUserTasks");
                didAppear = true;
            }
        }
    }
}