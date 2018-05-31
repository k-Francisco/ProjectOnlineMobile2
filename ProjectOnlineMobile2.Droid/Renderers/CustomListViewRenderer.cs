using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using ProjectOnlineMobile2.Controls;
using ProjectOnlineMobile2.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(CustomListView),typeof(CustomListViewRenderer))]
namespace ProjectOnlineMobile2.Droid.Renderers
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        public CustomListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            try
            {
                var x = Control.Parent as SwipeRefreshLayout;
                x?.SetColorSchemeColors(global::Android.Graphics.Color.ParseColor("#31752F"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CustomListViewRenderer", ex.Message);
            }
        }
    }
}