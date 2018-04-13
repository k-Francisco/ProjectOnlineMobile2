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

[assembly:ExportRenderer(typeof(CustomProgressBar),typeof(CustomProgressBarRenderer))]
namespace ProjectOnlineMobile2.Android.Renderers
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        public CustomProgressBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            //Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.FromRgb(182, 231, 233).ToAndroid()); //Change the color
            Control.ScaleY = 30; //Changes the height
        }
    }
}