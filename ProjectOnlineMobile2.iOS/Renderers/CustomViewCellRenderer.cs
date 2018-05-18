using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using ProjectOnlineMobile2.Controls;
using ProjectOnlineMobile2.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(CustomViewCell),typeof(CustomViewCellRenderer))]
namespace ProjectOnlineMobile2.iOS.Renderers
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);

            cell.SelectedBackgroundView = new UIView {
                BackgroundColor = UIColor.Clear,
            };

            return cell;
        }
    }
}