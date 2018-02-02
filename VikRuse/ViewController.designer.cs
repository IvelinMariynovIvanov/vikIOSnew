// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace VikRuse
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem AddCustomer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem Camera { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView EmployeesTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel mFullUpdateDateText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationItem NavigationItem { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem RefreshBtn { get; set; }

        [Action ("AddCustomer_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddCustomer_Activated (UIKit.UIBarButtonItem sender);

        [Action ("Camera_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Camera_Activated (UIKit.UIBarButtonItem sender);

        [Action ("RefreshBtn_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RefreshBtn_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddCustomer != null) {
                AddCustomer.Dispose ();
                AddCustomer = null;
            }

            if (Camera != null) {
                Camera.Dispose ();
                Camera = null;
            }

            if (EmployeesTableView != null) {
                EmployeesTableView.Dispose ();
                EmployeesTableView = null;
            }

            if (mFullUpdateDateText != null) {
                mFullUpdateDateText.Dispose ();
                mFullUpdateDateText = null;
            }

            if (NavigationItem != null) {
                NavigationItem.Dispose ();
                NavigationItem = null;
            }

            if (RefreshBtn != null) {
                RefreshBtn.Dispose ();
                RefreshBtn = null;
            }
        }
    }
}