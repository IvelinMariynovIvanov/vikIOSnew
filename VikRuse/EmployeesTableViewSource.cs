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
    //public class OnEditCustomerEventArgs : EventArgs
    //{
    //    private bool isThereANewCharge;
    //    private bool isThereALateBill;
    //    private bool isThereAReport;
    //    private int currentPossition;

    //    private int possUp;
    //    private int possDown;

    //    public OnEditCustomerEventArgs()
    //    {

    //    }

    //    public OnEditCustomerEventArgs(bool isThereANewCharge, bool isThereALateBill, bool isThereAReport, int currentPossition)
    //    {
    //        this.IsThereANewCharge = isThereANewCharge;
    //        this.IsThereALateBill = isThereALateBill;
    //        this.IsThereAReport = isThereAReport;
    //        this.CurrentPossition = currentPossition;
    //    }

    //    public bool IsThereANewCharge { get => isThereANewCharge; set => isThereANewCharge = value; }
    //    public bool IsThereALateBill { get => isThereALateBill; set => isThereALateBill = value; }
    //    public bool IsThereAReport { get => isThereAReport; set => isThereAReport = value; }
    //    public int CurrentPossition { get => currentPossition; set => currentPossition = value; }

    //    public int PossUp { get => possUp; set => possUp = value; }
    //    public int PossDown { get => possDown; set => possDown = value; }

    //}

    internal class EmployeesTableViewSource : UITableViewSource
    {

        private static string mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string mFilename = Path.Combine(mDocuments, "Customers.txt");


        private List<Customer> mEmployees;
        private UIStoryboard mStoryBoard;
        private ViewController mViewController;   
        private UINavigationController navController;

        private List<Customer> mCustomers;
        //private UIButton mDel;
        //private UIButton mEdit;



        public EmployeesTableViewSource(List<Customer> employees, UIStoryboard storyBoard, ViewController viewController, UINavigationController navController) //, UIButton delete, UIButton edit)
        {
            this.mEmployees = employees;
            this.mStoryBoard = storyBoard;
            this.mViewController = viewController;
            this.navController = navController;
            //this.mDel = delete;
            //this.mEdit = edit;
        }

        public EmployeesTableViewSource(List<Customer> mEmployees)
        {
            this.mEmployees = mEmployees;
        }

        

        //public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        //{

        //  //  base.CommitEditingStyle(tableView, editingStyle, indexPath);


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
             }));

             confirmCustomerDelete.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => this.Dispose()));

             //Present Alert
             navController.PresentViewController(confirmCustomerDelete, true, null);
             

              

                //if (mEmployees.Count == 0)
                //{
                //    this.mViewController.MFullUpdateText.Text = "Моля добавете абонати";
                //    //ViewController mainScreeen = this.mStoryBoard.InstantiateViewController("ViewController") as ViewController;

                //    //// mainScreeen.MFullUpdateText.Text = "Моля добавете абонати";
                //    //navController.PushViewController(mainScreeen, true);

                //}

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


            //cell.EditBtn.TouchUpInside += (sender, e) =>
            //{
            //    var row = ((UIButton)sender).Tag;
            //    Customer currentCustomer = mEmployees[indexPath.Row];

            //    var fragmentController = mStoryBoard.InstantiateViewController("DetailView");

            //    fragmentController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            //    mViewController.PresentViewController(fragmentController, true, null);
            //    //this.navController.PushViewController(fragmentController, true);

            //    // tableView.ReloadData();
            //};

            //   cell.UpdateCell(currentEmployee);

            return cell;



            //var width = tableView.ParentView.Width;
            //var height = tableView.RenderHeight;

            //var rect = new CGRect(cell.Bounds.X, cell.Bounds.Y, (nfloat)width, (nfloat)height);

            //var gradient = new CAGradientLayer
            //{
            //    Colors = new[] { UIColor.White.CGColor, UIColor.FromWhiteAlpha((nfloat)0.9, (nfloat)1.0).CGColor },
            //    Frame = rect
            //};
            //cell.Layer.InsertSublayer(gradient, 0);


            //cell.SeparatorInset = UIEdgeInsets.Zero;
            //tableView.SeparatorStyle = UITableViewCellStyle.Default;


            //cell.Layer.BorderWidth = 2.0f;
            //cell.Layer.BorderColor = UIColor.Gray.CGColor;

            //  return cell;

        }

        //private void DetailViewController_OnEditCustomerComplete(object sender, OnEditCustomerEventArgs e)
        //{
            
        //}

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return mEmployees.Count;
        }
    }
}