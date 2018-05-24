using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using AGragphics = Android.Graphics;
using AViews = Android.Views;
using ProjectOnlineMobile2.Controls;
using ProjectOnlineMobile2.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Support.V4.Content;

[assembly:ExportRenderer(typeof(CustomPicker),typeof(CustomPickerRenderer))]
namespace ProjectOnlineMobile2.Droid.Renderers
{
    public class CustomPickerRenderer : PickerRenderer
    {
        CustomPicker element;

        public CustomPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            element = (CustomPicker)this.Element;

            if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
                Control.Background = AddPickerStyles(element.Image);
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = AGragphics.Color.Gray;
            border.SetPadding(10, 10, 10, 10);
            border.Paint.SetStyle(Paint.Style.Stroke);

            Drawable[] layers = { border, GetDrawable(imagePath) };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);

            return layerDrawable;
        }

        private Drawable GetDrawable(string imagePath)
        {
            int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 24, 24, true));
            result.Gravity = AViews.GravityFlags.Right;

            return result;
        }
    }
}