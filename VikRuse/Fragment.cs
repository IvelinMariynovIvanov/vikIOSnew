using Foundation;
using System;
using UIKit;

namespace VikRuse
{
    public partial class Fragment : UIViewController
    {
        public Fragment (IntPtr handle) : base (handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Black.ColorWithAlpha(0.5f);
            View.Opaque = false;


        }

      
    
    }
}