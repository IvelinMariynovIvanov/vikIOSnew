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
            SetLabelColors(currentEmployee);

            SetLabelText(currentEmployee);

        }

        private void SetLabelText(Customer currentEmployee)
        {
            BillNumber.Text = currentEmployee.Nomer;

            Address.Text = currentEmployee.Address;

            Name.Text = currentEmployee.FullName;

            MoneyToPayLabel.Text = "дължима сума: ";

            MoneyToPayValue.Text = currentEmployee.MoneyToPay.ToString("N2") + " лв";

            EnddateLabel.Text = "срок за плащане: ";

            EnddateValue.Text = currentEmployee.EndPayDate.ToShortDateString();

            OldBillLabel.Text = "старо задължение";

            OldBillLValue.Text = currentEmployee.OldBill.ToString("N2") + " лв";

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
        }

        private void SetLabelColors(Customer currentEmployee)
        {
            //if (currentEmployee.ReceiveNotifyInvoiceOverdueToday == true)
            //{
            //    MoneyToPayValue.TextColor = UIColor.Red.ColorWithAlpha(alpha: 0.8f);

            //    EnddateValue.TextColor = (UIColor.Red);
            //}
            //else if (currentEmployee.ReceiveNotifyInvoiceOverdueToday == false)
            //{
            //    MoneyToPayValue.TextColor = UIColor.Green.ColorWithAlpha(0.8f);
            //}

            if (currentEmployee.OldBill != 0)
            {
                OldBillLValue.TextColor = UIColor.Red.ColorWithAlpha(alpha: 0.8f); //(UIColor.Red);
            }

            if (currentEmployee.MoneyToPay == 0)
            {
                MoneyToPayValue.TextColor = (UIColor.Black);
            }
            if (currentEmployee.MoneyToPay != 0)
            {
                MoneyToPayValue.TextColor = UIColor.Green.ColorWithAlpha(0.8f);
            }


            if (currentEmployee.ReciveNotifyReadingToday == true)
            {
                ReportDateValue.TextColor = (UIColor.Red);
            }

            //if (currentEmployee.ReceiveNotifyNewInvoiceToday == true)
            //{
            //    MoneyToPayValue.TextColor = UIColor.Green.ColorWithAlpha(0.8f);
            //}
        }

    }
}