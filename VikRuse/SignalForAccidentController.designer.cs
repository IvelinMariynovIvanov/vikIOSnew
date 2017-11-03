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
    [Register ("SignalForAccidentController")]
    partial class SignalForAccidentController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addPicFromCamera { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addPicFromGalary { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField mAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField mCity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField mDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel mError { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField mFullName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField mPhoneNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView pic { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Sent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Test { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addPicFromCamera != null) {
                addPicFromCamera.Dispose ();
                addPicFromCamera = null;
            }

            if (addPicFromGalary != null) {
                addPicFromGalary.Dispose ();
                addPicFromGalary = null;
            }

            if (mAddress != null) {
                mAddress.Dispose ();
                mAddress = null;
            }

            if (mCity != null) {
                mCity.Dispose ();
                mCity = null;
            }

            if (mDescription != null) {
                mDescription.Dispose ();
                mDescription = null;
            }

            if (mError != null) {
                mError.Dispose ();
                mError = null;
            }

            if (mFullName != null) {
                mFullName.Dispose ();
                mFullName = null;
            }

            if (mPhoneNumber != null) {
                mPhoneNumber.Dispose ();
                mPhoneNumber = null;
            }

            if (pic != null) {
                pic.Dispose ();
                pic = null;
            }

            if (Sent != null) {
                Sent.Dispose ();
                Sent = null;
            }

            if (Test != null) {
                Test.Dispose ();
                Test = null;
            }
        }
    }
}