using Foundation;
using System;
using UIKit;

namespace ProjectOnlineMobile2.iOS
{
    public partial class HomeController : UIViewController
    {
        public HomeController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "Home";

            btnGo.TouchUpInside += GoToPage1;
        }

        private void GoToPage1(object sender, EventArgs e)
        {
            
        }
    }
}