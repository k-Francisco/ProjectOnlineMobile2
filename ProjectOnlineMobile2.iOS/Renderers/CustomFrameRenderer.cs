using CoreGraphics;
using ProjectOnlineMobile2.Controls;
using ProjectOnlineMobile2.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(CustomFrame),typeof(CustomFrameRenderer))]
namespace ProjectOnlineMobile2.iOS.Renderers
{
    public class CustomFrameRenderer : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            Layer.ShadowRadius = 2.0f;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(2, 2);
            Layer.ShadowOpacity = 0.8f;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
        }
    }
}
