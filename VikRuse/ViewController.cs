using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;
using UserNotifications;
using static VikRuse.OnTapNotificationCustomEventArgs;

namespace VikRuse
{
    public partial class ViewController : UIViewController
    {
        //public static UIStoryboard Storyboard = UIStoryboard.FromName("MainStoryboard", null);

        public event EventHandler<OnTapNotificationCustomEventArgs> OnTapNotificationPressed;

        private static string mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string mFilename = Path.Combine(mDocuments, "Customers.txt");
        private string mNotificationFileName = Path.Combine(mDocuments, "Notification.txt");

        private string mStoryBoardName = Path.Combine(mDocuments, "StoryBoard.txt");

        private List<Customer> mCustomers; // = new List<Customer>(); //
        private string listOfCustomersAsJsonString = string.Empty;
        private string categoryID;

        private string storyBoardAsJsonString;

        private UIStoryboard mStoryboard;

        private UINavigationController mNavigationController;

   

        //private UINavigationBar mBar
        //{
        //    get { return NavigationItem; }
        //}

        //   public IntPtr UINavigationBar { get; private set; }

        public ViewController()
        {

        }   

        public ViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            this.NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(0, 134, 255);
            this.NavigationController.NavigationBar.TintColor = UIColor.White;

            GetCustomersSavedInPhone();

    
            EmployeesTableView.Source = new EmployeesTableViewSource(mCustomers, Storyboard, this, NavigationController);

            //   EmployeesTableView.Source = new EmployeesTableViewSource(mCustomers);

            EmployeesTableView.RowHeight = UITableView.AutomaticDimension;
            EmployeesTableView.EstimatedRowHeight = 100f;
            EmployeesTableView.ReloadData();

            if (OnTapNotificationPressed != null)
            {
                OnTapNotificationPressed.Invoke
                    (this, new OnTapNotificationCustomEventArgs(Storyboard, this.NavigationController));
            }
        }

        public void OnTapNotification(OnTapNotificationCustomEventArgs e)
        {
            if (OnTapNotificationPressed != null)
            {
                OnTapNotificationPressed.Invoke(this, new OnTapNotificationCustomEventArgs(Storyboard, this.NavigationController));
            }
        }

        private void GetCustomersSavedInPhone()
        {
            try
            {
                listOfCustomersAsJsonString = File.ReadAllText(mFilename);

                if (listOfCustomersAsJsonString == null || listOfCustomersAsJsonString == string.Empty)
                {
                    mCustomers = new List<Customer>();
                }
                else
                {
                    mCustomers = JsonConvert.DeserializeObject<List<Customer>>(listOfCustomersAsJsonString);
                }

                if (mCustomers == null)
                {
                    mCustomers = new List<Customer>();
                }
            }
            catch (Exception e)
            {
                if (listOfCustomersAsJsonString == null)
                {
                    mCustomers = new List<Customer>();
                }
                else
                {
                    mCustomers = JsonConvert.DeserializeObject<List<Customer>>(listOfCustomersAsJsonString);
                }

                if (mCustomers == null)
                {
                    mCustomers = new List<Customer>();
                }
            }
        }


        //partial void UIButton2989_TouchUpInside(UIButton sender)
        //{
        //    var detailViewController = Storyboard.InstantiateViewController("DetailView");
        //    detailViewController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
        //    PresentViewController(detailViewController, true, null);
        //}

        partial void Camera_Activated(UIBarButtonItem sender)
        {
            NavigationItem.BackBarButtonItem = new UIBarButtonItem("  Сигнал за авария", UIBarButtonItemStyle.Plain, null);    
            var signal = Storyboard.InstantiateViewController("SignalController");
            this.NavigationController.PushViewController(signal, true);

        }

        partial void AddCustomer_Activated(UIBarButtonItem sender)
        {
            var navBar =  NavigationItem.BackBarButtonItem = new UIBarButtonItem("  Добави абонат", UIBarButtonItemStyle.Plain ,null);
            var addCustomer = Storyboard.InstantiateViewController("AddCustomer");
            this.NavigationController.PushViewController(addCustomer, true);
        }

        partial void SetnNotification(UIButton sender)
        {
            UserNotificationCenterDelegate userNotificationDelegate =
               new UserNotificationCenterDelegate(this.Storyboard, this.NavigationController);

            userNotificationDelegate.mStoryboard = this.Storyboard;
            userNotificationDelegate.mNavigationController = this.NavigationController;

            List<UserNotificationCenterDelegate> list = new List<UserNotificationCenterDelegate>();

            list.Add(userNotificationDelegate);

            var userNotificationDelegateAsJSON = JsonConvert.SerializeObject(list);
            File.WriteAllText(mNotificationFileName, userNotificationDelegateAsJSON);

        



            //var content = new UNMutableNotificationContent();
            //content.Title = "Notification Title";
            //content.Subtitle = "Notification Subtitle";
            //content.Body = "This is the message body of the notification.";
            //content.Badge = 1;


            //var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);

            //var requestID = "sampleRequest";
            //var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            //UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            //{
            //    if (err != null)
            //    {
            //        // Do something with error...
            //    }
            //});



            if (OnTapNotificationPressed != null)
            {
                OnTapNotificationPressed.Invoke
                    (this, new OnTapNotificationCustomEventArgs(this.Storyboard, this.NavigationController));
            }

            RegisterForNotifications();

            ScheduleNotification();

        }

        void RegisterForNotifications()
        {
            // Create action
            var actionID = "check";
            var title = "Check";
            var action = UNNotificationAction.FromIdentifier(actionID, title, UNNotificationActionOptions.Foreground);

            // Create category
            categoryID = "notification";
            var actions = new UNNotificationAction[] { action };
            var intentIDs = new string[] { };
            //var categoryOptions = new UNNotificationCategoryOptions[] { };
            var category = UNNotificationCategory.FromIdentifier(categoryID, actions, intentIDs, UNNotificationCategoryOptions.None);

            // Register category
            var categories = new UNNotificationCategory[] { category };
            UNUserNotificationCenter.Current.SetNotificationCategories(new NSSet<UNNotificationCategory>(categories));

            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert
                                                                  | UNAuthorizationOptions.Badge
                                                                  | UNAuthorizationOptions.Sound,
                                                                  (a, err) => {
                                                                      //TODO handle error
                                                                  });
        }

        void ScheduleNotification()
        {
            // Create content
            var content = new UNMutableNotificationContent();
            content.Title = "Title";
            content.Subtitle = "Subtitle";
            content.Body = "Body";
            content.Badge = 1;
            content.CategoryIdentifier = categoryID;
            content.Sound = UNNotificationSound.Default;


            var notification = new UILocalNotification();
            // Fire trigger in one seconds
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.1, false);

            var requestID = "notificationRequest";   ///sampleRequest
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            //Here I set the Delegate to handle the user tapping on notification
            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => 
            {
                if (err != null)
                {
                    // Report error
                    System.Console.WriteLine("Error: {0}", err);
                }
                else
                {
                    // Report Success
                    System.Console.WriteLine("Notification Scheduled: {0}", request);
                }
            });
        }
    }
}