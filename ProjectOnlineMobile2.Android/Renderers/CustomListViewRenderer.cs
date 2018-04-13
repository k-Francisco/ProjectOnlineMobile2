using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ProjectOnlineMobile2.Android.Renderers;
using ProjectOnlineMobile2.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(CustomListView),typeof(CustomListViewRenderer))]
namespace ProjectOnlineMobile2.Android.Renderers
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        public CustomListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                Control.VerticalScrollBarEnabled = false;
            }
        }
    }
}