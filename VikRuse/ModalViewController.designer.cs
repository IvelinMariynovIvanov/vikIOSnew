// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace VikRuse
{
    [Register ("ModalViewController")]
    partial class ModalViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Cancel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel mCurrentPoss { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch mLateCharge { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch mNewCharge { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton mPossDown { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton mPossUp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch mReport { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Save { get; set; }

        [Action ("Cancel_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Cancel_TouchUpInside (UIKit.UIButton sender);

        [Action ("Save_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Save_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Cancel != null) {
                Cancel.Dispose ();
                Cancel = null;
            }

            if (mCurrentPoss != null) {
                mCurrentPoss.Dispose ();
                mCurrentPoss = null;
            }

            if (mLateCharge != null) {
                mLateCharge.Dispose ();
                mLateCharge = null;
            }

            if (mNewCharge != null) {
                mNewCharge.Dispose ();
                mNewCharge = null;
            }

            if (mPossDown != null) {
                mPossDown.Dispose ();
                mPossDown = null;
            }

            if (mPossUp != null) {
                mPossUp.Dispose ();
                mPossUp = null;
            }

            if (mReport != null) {
                mReport.Dispose ();
                mReport = null;
            }

            if (Save != null) {
                Save.Dispose ();
                Save = null;
            }
        }
    }
}