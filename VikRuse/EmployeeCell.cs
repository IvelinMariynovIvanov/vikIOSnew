using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace VikRuse
{
    public partial class EmployeeCell : UITableViewCell
    {

        public UIButton DeleteBtn
        {
            

            get
            {
                return DeleteButton;
            }
        }

        public UIButton EditBtn
        {

            get
            {
                return EditButton;
            }
        }

       

        public EmployeeCell (IntPtr handle) : base (handle)
        {

        }

        public void UpdateCell(Customer currentEmployee)
        {

            BillNumber.Text = currentEmployee.Nomer;

            Address.Text = currentEmployee.Address;

            Name.Text = currentEmployee.FullName;

            MoneyToPayLabel.Text = "дължима сума: ";

            MoneyToPayValue.Text = currentEmployee.MoneyToPay.ToString("N2") + " лв";

            EnddateLabel.Text = "срок за плащане: ";

            EnddateValue.Text = currentEmployee.EndPayDate.ToShortDateString();

            OldBillLabel.Text = "старо задължение";

            OldBillLValue.Text = currentEmployee.OldBill.ToString("N2") +" лв";

            ReportDateLabel.Text = "дата на отчитане: ";

            if (currentEmployee.StartReportDate == DateTime.MinValue)
            {
                ReportDateValue.Text = "Не е зададен график.";
            }
            else
            {
                ReportDateValue.Text =
                             currentEmployee.StartReportDate.ToShortDateString() + " " +
                             currentEmployee.StartReportDate.ToShortTimeString() + "-" +
                             currentEmployee.EndReportDate.ToShortTimeString();
            }



            if (currentEmployee.ReceiveNotifyInvoiceOverdueToday == true)
            {
                MoneyToPayValue.TextColor = UIColor.Red.ColorWithAlpha(alpha: 0.8f);

                EnddateValue.TextColor = (UIColor.Red);
            }
            else if (currentEmployee.ReceiveNotifyInvoiceOverdueToday == false)
            {
                MoneyToPayValue.TextColor = UIColor.Green.ColorWithAlpha(0.8f);
            }

            if (currentEmployee.MoneyToPay == 0)
            {
                MoneyToPayValue.TextColor = (UIColor.Black);
            }

            if (currentEmployee.ReciveNotifyReadingToday == true)
            {
                ReportDateValue.TextColor = (UIColor.Red);
            }
            if (currentEmployee.ReceiveNotifyNewInvoiceToday == true)
            {
                MoneyToPayValue.TextColor = UIColor.Green.ColorWithAlpha(0.8f);
            }


            //  ReportDateValue.Text = currentEmployee.StartReportDate.ToString();



            //DeleteButton = DeleteBtn;

            //EditButton = EditButton;

            //  EditButton.TouchInside += (object sender, EventArgs e) => { };

            //var gradientLayer = new CAGradientLayer();
            ////#01579b, #3187cb
            //gradientLayer.Colors = new[] { UIColor.Red.CGColor, UIColor.Blue.CGColor };
            //gradientLayer.Locations = new NSNumber[] { 0, 1 };

            //// gradientLayer.Frame = this.Frame;
            //gradientLayer.Frame = HorizontalLine.Frame;

            //HorizontalLine.BackgroundColor = UIColor.Clear;
            //HorizontalLine.Layer.AddSublayer(gradientLayer);
            //HorizontalLine.Layer.InsertSublayer(gradientLayer, 1);

            ////gradientLayer.StartPoint = CGPointMake(0.0, 0.5);
            ////gradientLayer.EndPoint = CGPointMake(1.0, 0.5);

            ////HorizontalLine.Frame = this.Frame;



        }

       

        //partial void DeleteButton_TouchUpInside(UIButton sender)
        //{
        //    var detailViewController = Storyboard.InstantiateViewController("DetailView");
        //    detailViewController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
        //    PresentViewController(detailViewController, true, null);
        //}



    }
}