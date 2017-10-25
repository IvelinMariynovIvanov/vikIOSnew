using System;

namespace VikRuse
{
    public class Customer
    {

        public string EGN { get; set; } // not showing
        public string Nomer { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }

        public bool NotifyNewInvoice { get; set; }
        public bool NotifyInvoiceOverdue { get; set; }
        public bool NotifyReading { get; set; }

        public bool ReceiveNotifyNewInvoiceToday { get; set; }
        public bool ReceiveNotifyInvoiceOverdueToday { get; set; }
        public bool ReciveNotifyReadingToday { get; set; }

        //public bool DidGetNewInoviceToday { get; set; }
        //public bool DidGetOverdueToday { get; set; }
        //public bool DidGetReadingToday { get; set; }
        //public bool DidGetAnyNotificationToday { get; set; }

        public double MoneyToPay { get; set; }
        public double OldBill { get; set; }

        public DateTime EndPayDate { get; set; }
        public DateTime StartReportDate { get; set; }
        public DateTime EndReportDate { get; set; }


        public Customer()
        {
                
        }
        public Customer(string eGN, string nomer)
        {
            EGN = eGN;
            Nomer = nomer;
        }

        public Customer(string eGN, string nomer, string fullName, string address, bool notifyNewInvoice, bool notifyInvoiceOverdue, bool notifyReading, bool receiveNotifyNewInvoiceToday, bool receiveNotifyInvoiceOverdueToday, bool reciveNotifyReadingToday, double moneyToPay, double oldBill, DateTime endPayDate, DateTime startReportDate, DateTime endReportDate)
        {
            EGN = eGN;
            Nomer = nomer;
            FullName = fullName;
            Address = address;
            NotifyNewInvoice = notifyNewInvoice;
            NotifyInvoiceOverdue = notifyInvoiceOverdue;
            NotifyReading = notifyReading;
            ReceiveNotifyNewInvoiceToday = receiveNotifyNewInvoiceToday;
            ReceiveNotifyInvoiceOverdueToday = receiveNotifyInvoiceOverdueToday;
            ReciveNotifyReadingToday = reciveNotifyReadingToday;
            MoneyToPay = moneyToPay;
            OldBill = oldBill;
            EndPayDate = endPayDate;
            StartReportDate = startReportDate;
            EndReportDate = endReportDate;
        }



    }
}