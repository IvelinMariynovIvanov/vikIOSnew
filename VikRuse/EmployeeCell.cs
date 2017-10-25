using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using UserNotifications;

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

            MoneyToPayValue.Text = currentEmployee.MoneyToPay.ToString();

            EnddateLabel.Text = "срок за плащане: ";

            EnddateValue.Text = currentEmployee.EndPayDate.ToString();

            OldBillLabel.Text = "старо задължение";

            OldBillLValue.Text = currentEmployee.OldBill.ToString();

            ReportDateLabel.Text = "дата на отчитане: ";

            ReportDateValue.Text = currentEmployee.StartReportDate.ToString();

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