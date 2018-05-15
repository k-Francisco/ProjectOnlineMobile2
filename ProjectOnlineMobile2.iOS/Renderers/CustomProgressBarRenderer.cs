using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using ProjectOnlineMobile2.Controls;
using ProjectOnlineMobile2.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(CustomProgressBar),typeof(CustomProgressBarRenderer))]
namespace ProjectOnlineMobile2.iOS.Renderers
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var x = 1.0f;
            var y = 15.0f;

            CGAffineTransform transform = CGAffineTransform.MakeScale(x, y);
            this.Transform = transform;
        }
    }
}