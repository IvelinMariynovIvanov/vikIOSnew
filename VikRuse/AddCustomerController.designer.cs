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
    [Register ("AddCustomerController")]
    partial class AddCustomerController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddCustomer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField BillNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Egn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Error { get; set; }

        [Action ("AddCustomer_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddCustomer_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddCustomer != null) {
                AddCustomer.Dispose ();
                AddCustomer = null;
            }

            if (BillNumber != null) {
                BillNumber.Dispose ();
                BillNumber = null;
            }

            if (Egn != null) {
                Egn.Dispose ();
                Egn = null;
            }

            if (Error != null) {
                Error.Dispose ();
                Error = null;
            }
        }
    }
}