using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using CoreGraphics;
using CoreAnimation;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

namespace VikRuse
{
   
    internal class EmployeesTableViewSource : UITableViewSource
    {

        private static string mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string mFilename = Path.Combine(mDocuments, "Customers.txt");


        private List<Customer> mEmployees;
        private UIStoryboard mStoryBoard;
        private ViewController mViewController;   
        private UINavigationController navController;


        public EmployeesTableViewSource(List<Customer> employees, UIStoryboard storyBoard, ViewController viewController, UINavigationController navController) 
        {
            this.mEmployees = employees;
            this.mStoryBoard = storyBoard;
            this.mViewController = viewController;
            this.navController = navController;
        }
        
  
        public EmployeesTableViewSource(List<Customer> mEmployees)
        {
            this.mEmployees = mEmployees;
        }

        /// <summary>
        /// This method swipe and delete row
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="editingStyle"></param>
        /// <param name="indexPath"></param>
        //public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        //{

        //    //  base.CommitEditingStyle(tableView, editingStyle, indexPath);


        //    switch (editingStyle)
        //    {
        //        case UITableViewCellEditingStyle.Delete:

        //            // remove the item from the underlying data source
        //            mEmployees.RemoveAt(indexPath.Row);

        //            // delete the row from the table
        //            tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);

        //            var listOfCustomersAsJson = JsonConvert.SerializeObject(this.mEmployees);

        //            File.WriteAllText(mFilename, listOfCustomersAsJson);

        //            break;
        //    }
        //}



        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("bg-BG");

            EmployeeCell cell = (EmployeeCell)tableView.DequeueReusableCell("Custom_Cell", indexPath);

            Customer currentEmployee = mEmployees[indexPath.Row];

            cell.UpdateCell(currentEmployee);

            // setTag to button to identify in which row button is pressed 
            cell.DeleteBtn.Tag = indexPath.Row;
            cell.EditBtn.Tag = indexPath.Row;

            //assign action
            cell.DeleteBtn.TouchUpInside += (sender, e) =>
            {
             //Create Alert
             var confirmCustomerDelete = UIAlertController.Create("Потвърждавате ли изтриването ?", 
                                                                     $"Изтрий клиент " +
                                                                     $"{currentEmployee.FullName.ToString()}",
                                                                      UIAlertControllerStyle.Alert);

             //Add Actions to alert
             confirmCustomerDelete.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, alert =>
             {
                 var row = ((UIButton)sender).Tag;
                 var currentCustomer = mEmployees[indexPath.Row];

                 mEmployees.RemoveAt((int)row);

                 var listOfCustomersAsJson = JsonConvert.SerializeObject(this.mEmployees);

                 File.WriteAllText(mFilename, listOfCustomersAsJson);

                 tableView.ReloadData();

                 if (mEmployees.Count == 0)
                 {
                    
                     InvokeOnMainThread(() =>
                     {

                         mViewController.MFullUpdateText.Text = "Моля добавете абонати";
                     }
                     );                   
                 }
             }));

             confirmCustomerDelete.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => this.Dispose()));

             //Present Alert
             navController.PresentViewController(confirmCustomerDelete, true, null);

            };

            cell.EditBtn.TouchUpInside += (sender, e) =>
            {
                var row = ((UIButton)sender).Tag;
                var currentCustomer = mEmployees[indexPath.Row];

                var detailViewController1 = mStoryBoard.InstantiateViewController("DetailView");


                //var detailViewController = mStoryBoard.InstantiateViewController("TestModalvViewMiniController");

                //var detailViewController = new ModalViewController(Handle);

                //var detailViewController = new ModalViewController(indexPath.Row, mEmployees.Count,
                //                      currentCustomer.NotifyNewInvoice, currentCustomer.NotifyInvoiceOverdue, currentCustomer.NotifyReading);

                var detailViewController = new ModalViewController();
                detailViewController = (ModalViewController)detailViewController1;

                detailViewController.MCurrentPosition = indexPath.Row;
                detailViewController.MCustomresCount = mEmployees.Count;
                detailViewController.MIsNewCharge = currentCustomer.NotifyNewInvoice;
                detailViewController.MIsLateCharge = currentCustomer.NotifyInvoiceOverdue;
                detailViewController.MIsReport = currentCustomer.NotifyReading;

                detailViewController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
                mViewController.PresentViewController(detailViewController, true, null); //mViewController, navController

               // navController.PushViewController(detailViewController, true);

                detailViewController.OnEditCustomerComplete += (object sender1, OnEditCustomerEventArgs e1) =>
                {
                    currentCustomer.NotifyNewInvoice = e1.IsThereANewCharge;
                    currentCustomer.NotifyInvoiceOverdue = e1.IsThereALateBill;
                    currentCustomer.NotifyReading = e1.IsThereAReport;

                    Customer updateCustomer = mEmployees[indexPath.Row];

                    updateCustomer.NotifyNewInvoice = e1.IsThereANewCharge;
                    updateCustomer.NotifyInvoiceOverdue = e1.IsThereALateBill;
                    updateCustomer.NotifyReading = e1.IsThereAReport;

                    mEmployees.RemoveAt(indexPath.Row); //new count -1
                    mEmployees.Insert(e1.CurrentPossition, updateCustomer); // put in the same posstion

                    var listOfCustomersAsJson = JsonConvert.SerializeObject(this.mEmployees);
                    File.WriteAllText(mFilename, listOfCustomersAsJson);

                    tableView.ReloadData();
                };

            };

            return cell;

        }

   

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return mEmployees.Count;
        }
    }
}