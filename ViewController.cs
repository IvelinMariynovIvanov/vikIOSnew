using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UIKit;
using UserNotifications;
using static VikRuse.OnTapNotificationCustomEventArgs;
using BigTed;


namespace VikRuse
{
    public partial class ViewController : UIViewController
    {
        //public static UIStoryboard Storyboard = UIStoryboard.FromName("MainStoryboard", null);

      //  public event EventHandler<OnTapNotificationCustomEventArgs> OnTapNotificationPressed;

        private static string mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string mCustomersFilename = Path.Combine(mDocuments, "Customers.txt");
        private string mNotificationFileName = Path.Combine(mDocuments, "Notification.txt");
        private string mHourFileName = Path.Combine(mDocuments, "Hour.txt");
        private string mDateFileName = Path.Combine(mDocuments, "Date.txt");

        private string mStoryBoardName = Path.Combine(mDocuments, "StoryBoard.txt");

        private List<Customer> mAllUpdateCustomerFromApi = new List<Customer>();
        private List<Customer> mCustomerFromApiToNotifyToday = new List<Customer>();
        private List<Customer> mCustomers; // = new List<Customer>(); //

        private List<Customer> mCountНotifyReadingustomers = new List<Customer>();
        private List<Customer> mCountНotifyInvoiceOverdueCustomers = new List<Customer>();
        private List<Customer> mCountNewНotifyNewInvoiceCustomers = new List<Customer>();

        private string listOfCustomersAsJsonString = string.Empty;
        private string categoryID;

        private string mDate1;
        private string mHour1;

        private string storyBoardAsJsonString;

        public UIStoryboard mStoryboard;

        public UINavigationController mNavigationController;

        public UIStoryboard MStoryboard { get => mStoryboard; set => mStoryboard = value; }
        public UINavigationController MNavigationController { get => mNavigationController; set => mNavigationController = value; }

        private string MDate { get => mDate1; set => mDate1 = value; }

        private string MHour { get => mHour1; set => mHour1 = value; }

        public UILabel MFullUpdateText { get => mFullUpdateDateText; set => mFullUpdateDateText = value; }

       // private UILabel MabonatiObnoveniKum { get => mAbonatiObnoveniKum; set => mAbonatiObnoveniKum = value;}
    
         



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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            GetCustomersSavedInPhone();


          //  AppDelegate app = new AppDelegate();

            if (mCustomers.Count != 0)
            {

                MHour = GetUpdateHour();
                MDate = GetUpdateDate();

                MFullUpdateText.Text = "Абонати обновени към " + MHour + MDate;

            }
            else
            {
                mFullUpdateDateText.Text = "Моля добавете абонати";
            }


            EmployeesTableView.Source = new EmployeesTableViewSource(mCustomers, Storyboard, this, NavigationController);

            EmployeesTableView.ReloadData();
        }

        public override void ViewDidLoad()
        {

           base.ViewDidLoad();


           this.NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(0, 134, 255);
           this.NavigationController.NavigationBar.TintColor = UIColor.White;

            // Perform any additional setup after loading the view, typically from a nib.


            GetCustomersSavedInPhone();

            if (mCustomers.Count != 0)
            {
                //mHour.Text = GetUpdateHour();
                //mDate.Text = GetUpdateDate();

                MHour = GetUpdateHour();
                MDate = GetUpdateDate();

                MFullUpdateText.Text = "Абонати обновени към " + MHour + MDate;

               // MFullUpdateText.Text = DateTime.Now.ToShortTimeString();

            }
            else
            {
                //mHour.Hidden = true;
                //mDate.Hidden = true;
                // mObnoveniKum.Visibility = ViewStates.Gone;

                //MHour.Enabled = false;
                //MDate.Enabled = false;
                //MabonatiObnoveniKum.Text = "Моля добавете абонати";

                mFullUpdateDateText.Text = "Моля добавете абонати";


            }


            EmployeesTableView.Source = new EmployeesTableViewSource(mCustomers, Storyboard, this, NavigationController);

            //   EmployeesTableView.Source = new EmployeesTableViewSource(mCustomers);

            EmployeesTableView.RowHeight = UITableView.AutomaticDimension;
            EmployeesTableView.EstimatedRowHeight = 100f;
            EmployeesTableView.ReloadData();



            //if (OnTapNotificationPressed != null)
            //{
            //    OnTapNotificationPressed.Invoke
            //        (this, new OnTapNotificationCustomEventArgs(Storyboard, this.NavigationController));
            //}

            MStoryboard = this.Storyboard;
            MNavigationController = this.NavigationController;


            //this.OnTapNotificationPressed += (object sender1, OnTapNotificationCustomEventArgs e) =>
            //{
            //    MStoryboard = e.MStoryboard;

            //    mNavigationController = e.MNavigationController;

            //    //var mainActivitiy = mStoryboard.InstantiateViewController("ViewController");
            //    //this.mNavigationController.PushViewController(mainActivitiy, true);

            //};
        }

        

        public  string GetUpdateDate()
        {
            try
            {
                var date = File.ReadAllText(mDateFileName);

                string format = "dd.MM.yyyy";

                if (date == null || date == string.Empty)
                {
                    return DateTime.Now.ToString(format);
                }

                var newDate = date;

                if (newDate == null || newDate == "(null)")
                {
                    return DateTime.Now.ToString(format);
                }
                return newDate;
            }
            catch(Exception e)
            {
                string format = "dd.MM.yyyy";

                return DateTime.Now.ToString(format);
            }
        }

        public  string GetUpdateHour()   
        {
            try
            {
                
            // read exisiting value
            var  hour = File.ReadAllText(mHourFileName);

            // if preferences return null, initialize listOfCustomers
            if (hour == null || hour == string.Empty)
            {
                string DateFormatt = "HH:mm";

                return DateTime.Now.ToString(DateFormatt) + " часа, ";
            }

            var newHour = (hour);

            if (newHour == null || newHour == "(null)")
            {
                string DateFormatt = "HH:mm";

                return DateTime.Now.ToString(DateFormatt);
            }

            return newHour + " часа, ";
            }
            catch(Exception e)
            {
                string DateFormatt = "HH:mm";
                return DateTime.Now.ToString(DateFormatt) + " часа, ";
            }
        }

        //public void OnTapNotification(OnTapNotificationCustomEventArgs e)
        //{
        //    if (OnTapNotificationPressed != null)
        //    {
        //        OnTapNotificationPressed.Invoke(this, new OnTapNotificationCustomEventArgs(Storyboard, this.NavigationController));
        //    }
        //}

      

        private void GetCustomersSavedInPhone()
        {
            try
            {
                listOfCustomersAsJsonString = File.ReadAllText(mCustomersFilename);

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

            //UserNotificationCenterDelegate userNotificationDelegate =
            //  new UserNotificationCenterDelegate(this.MStoryboard, this.MNavigationController);

            //userNotificationDelegate.mStoryboard = this.Storyboard;
            //userNotificationDelegate.mNavigationController = this.NavigationController;

            //List<UserNotificationCenterDelegate> list = new List<UserNotificationCenterDelegate>();

            //list.Add(userNotificationDelegate);

            //var userNotificationDelegateAsJSON = JsonConvert.SerializeObject(list);
            //File.WriteAllText(mNotificationFileName, userNotificationDelegateAsJSON);


            //userNotificationDelegate.OnTapNotificationPressed += (object sender1, OnTapNotificationCustomEventArgs e) =>
            //{
            //    MStoryboard = e.MStoryboard;

            //    MNavigationController = e.MNavigationController;

            //    //var mainActivitiy = mStoryboard.InstantiateViewController("ViewController");
            //    //this.mNavigationController.PushViewController(mainActivitiy, true);

            //};


            /////////////////////////////////////
          //  RegisterForNotifications();

            CreateNotification();
  

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

     private   void CreateNotification()
        {
            // Create content
            var content = new UNMutableNotificationContent();

            content.Title = "Title";
            content.Subtitle = "Subtitle";
            content.Body = "Body";
            content.Badge = 1;
           // content.CategoryIdentifier = categoryID;
            content.Sound = UNNotificationSound.Default;


            var notification = new UILocalNotification();
            // Fire trigger in one seconds
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.1, false);

            var requestID = "sampleRequest";   ///sampleRequest ,      notificationRequest
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            //Here I set the Delegate to handle the user tapping on notification
           // UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

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

        partial void RefreshBtn_Activated(UIBarButtonItem sender)
        {
            // RunOnUiThread(() => { ShowProgressDialog(); });

            //Show a HUD with a progress spinner and the text
            BTProgressHUD.Show("Обновяване");


            Thread thread = new Thread(RefreshCustomers);

            thread.Start();
        }

        private void RefreshCustomers()
        {
            GetCustomersSavedInPhone();
            
            //chek if there is no customers to refrsh
            if (mCustomers.Count == 0)
            {
                  // RunOnUiThread(() => RefreshProgressDialogAndToatWhenThereIsNoCustomers());

                ///////
                InvokeOnMainThread(() =>
                {
                    RefreshProgressDialogAndToatWhenThereIsNoCustomers();
                });

                return;
            }
            else if (mCustomers.Count != 0)
            {
                UpdateCustomers(mCustomers);

               // BTProgressHUD.Dismiss();
            }
        }
        private void RefreshProgressDialogAndToatWhenThereIsNoCustomers()
        {
            //mHour.Hidden = true ;
            //mDate.Hidden = true;
            // mObnoveniKum.Visibility = ViewStates.Gone;

            //MabonatiObnoveniKum.Text = "Моля добавете абонати";

            BTProgressHUD.Dismiss();

            MFullUpdateText.Text = "Моля добавете абонати";

            //  progress.Dismiss();
        }

        private void UpdateCustomers(List<Customer> mCustomers) //, ISharedPreferences pref)
        {
            ConnectToApi connectToApi = new ConnectToApi();

            bool connection = connectToApi.CheckConnectionOfVikSite();

            if (connection == true)
            {

                EncryptConnection encryp = new EncryptConnection();



                string crypFinalPass = encryp.Encrypt();

                //// get from preferences
                GrudMessageFromPreferemces grudMessage = new GrudMessageFromPreferemces();

                int lastMessageId = grudMessage.GetMessageFromPreferencesInPhone().MessageID;


                //realno !!!!!!!!!!!!!!
                string messageUrl = ConnectToApi.urlAPI + "api/msg/";

                ///teest
              //  string messageUrl = "http://192.168.2.222/VIKWebApi/api/msg/";

                string finalUrl = messageUrl + crypFinalPass + "/" + lastMessageId;

                var messageFromApiAsJsonString = connectToApi.FetchApiDataAsync(finalUrl);

                if (messageFromApiAsJsonString != null)
                {
                    Message newMessage = new Message();

                    newMessage = connectToApi.GetMessageFromApi(messageFromApiAsJsonString);

                    if (newMessage.MessageID > lastMessageId)
                    {
                        grudMessage.SaveMessageInPhone(newMessage);

                        int messagesCount = newMessage.Messages.Count;

                        if (messagesCount > 0)
                        {
                            SentNotificationWithoutSubscribe(newMessage);
                            
                        }
                    }
                }

                foreach (var customer in mCustomers)
                {
                    bool isReceiveNotifyNewInvoiceCheck = false;
                    bool isReceiveNotifyInvoiceOverdueCheck = false;
                    bool isReciveNotifyReadingCheck = false;

                    isReceiveNotifyNewInvoiceCheck = customer.NotifyNewInvoice;
                    isReceiveNotifyInvoiceOverdueCheck = customer.NotifyInvoiceOverdue;
                    isReciveNotifyReadingCheck = customer.NotifyReading;

                    // if(isAnyNotifycationCheck == true)
                    //   {
                    string billNumber = customer.Nomer.ToString();
                    string egn = customer.EGN.ToString();

                    //CREATE URL
                    // string url = "http://192.168.2.222/VIKWebApi/";

                    /// !!!!!!!!!!!!!!!!!!!!!!!!
                    // string realUrl = ConnectToApi.urlAPI + "api/abonats/" + crypFinalPass + "/" + billNumber + "/" + egn;

                     string realUrl = ConnectToApi.urlAPI + "api/abonats/" + crypFinalPass + "/" + billNumber + "/" + egn + "/" + ConnectToApi.updateByButtonRefresh + "/";
                    // string realUrl = "http://192.168.2.222/VIKWebApi/" + "api/abonats/" + crypFinalPass + "/" + billNumber + "/" + egn + "/"
                    // + ConnectToApi.updateByButtonRefresh + "/";

                    //string realUrl = ConnectToApi.urlAPI + "api/abonats/" + crypFinalPass + "/" + billNumber + "/" + egn + "/"
                    //              + ConnectToApi.updateByAddCutomerButton + "/"
                    //              + isReceiveNotifyNewInvoiceCheck + "/" + isReceiveNotifyInvoiceOverdueCheck + "/" + isReciveNotifyReadingCheck + "/";


                    var jsonResponse = connectToApi.FetchApiDataAsync(realUrl);
                   // string jsonResponse = null;

                    //check the api
                    if (jsonResponse == null)
                    {
                       // RunOnUiThread(() => RefreshProgressDialogAndToastWhenNoConnectioToApi());

                        return;
                    }

                    // check in vikSite is there a customer with this billNumber (is billNumber correct)
                    else if (jsonResponse == "[]")
                    {
                      //  RefreshProgressDialogAndToastWhenInputIsNotValid();

                        return;
                    }

                    // check is billNumber correct and get and save customer in phone
                    else if (jsonResponse != null)
                    {

                        Customer updateCutomerButNoNotify = connectToApi.GetCustomerFromApi(jsonResponse);

                        if (updateCutomerButNoNotify != null && updateCutomerButNoNotify.IsExisting == true)
                        //  (newCustomer != null && newCustomer.IsExisting == true)
                        {
                            updateCutomerButNoNotify.NotifyInvoiceOverdue = customer.NotifyInvoiceOverdue;
                            updateCutomerButNoNotify.NotifyNewInvoice = customer.NotifyNewInvoice;
                            updateCutomerButNoNotify.NotifyReading = customer.NotifyReading;

                            //if (updateCutomerButNoNotify.ReceiveNotifyInvoiceOverdueToday == true)
                            //{
                            //    updateCutomerButNoNotify.ReceiveNotifyInvoiceOverdueToday = true;
                            //}
                            //else
                            //{
                            //    updateCutomerButNoNotify.ReceiveNotifyInvoiceOverdueToday = customer.ReceiveNotifyInvoiceOverdueToday;
                            //}

                            //if (updateCutomerButNoNotify.ReceiveNotifyNewInvoiceToday == true)
                            //{
                            //    updateCutomerButNoNotify.ReceiveNotifyNewInvoiceToday = true;
                            //}
                            //else
                            //{
                            //    updateCutomerButNoNotify.ReceiveNotifyNewInvoiceToday = customer.ReceiveNotifyNewInvoiceToday;
                            //}

                            //if (updateCutomerButNoNotify.ReciveNotifyReadingToday == true)
                            //{
                            //    updateCutomerButNoNotify.ReciveNotifyReadingToday = true;
                            //}
                            //else
                            //{
                            //    updateCutomerButNoNotify.ReciveNotifyReadingToday = customer.ReciveNotifyReadingToday;
                            //}

                            mAllUpdateCustomerFromApi.Add(updateCutomerButNoNotify);     ////////////updateCutomerButNoNotify


                        }


                        else
                        {
                            InvokeOnMainThread(() =>
                            {
                                BTProgressHUD.Dismiss();
                               // RefreshProgressDialogAndToastWhenNoConnectioToApi();

                            });

                            return;
                        }
                    }
                    // }
                }

                #region setting the updating date

                string updateHour;
                string updateDate;

                GetUpdateDateAndHour(out updateHour, out updateDate);

                #endregion

                InvokeOnMainThread(() =>
                {
                    GetFinalUpdateDateHour();

                });

                SelectWhichCustomersTobeNotified(mCountНotifyReadingustomers, mCountНotifyInvoiceOverdueCustomers, mCountNewНotifyNewInvoiceCustomers, mAllUpdateCustomerFromApi ); // mCustomerFromApiToNotifyToday

                InvokeOnMainThread(() =>
                {

                    SaveUpdatesInPhone(MDate.ToString(), MHour.ToString());

                    this.ViewWillAppear(true);

                    //ViewController mainScreeen = this.Storyboard.InstantiateViewController("ViewController") as ViewController;

                    //this.NavigationController.PushViewController(mainScreeen, true);
                });
       

                SentNoficationForNewInovoice(mCountNewНotifyNewInvoiceCustomers);

                SentNotificationForOverdue(mCountНotifyInvoiceOverdueCustomers);

                SentNotificationForReading(mCountНotifyReadingustomers);


                InvokeOnMainThread(() =>
                {
                    BTProgressHUD.Dismiss();
                });


            }
            else
            {
                //   InvokeOnMainThread(() => RefreshProgresDialogAndToastWhenThereIsNoConnection());
                BTProgressHUD.Dismiss();

                return;
            }
        }

        private void SentNotificationWithoutSubscribe(Message newMessage)
        {

            var content = new UNMutableNotificationContent();

            content.Title = "Съобщение от ВиК Русе";

            foreach (var item in newMessage.Messages)
            {
                //  Generate a message summary for the body of the notification:
                content.Subtitle = ($"{item.ToString()}");
                
            }

            content.Sound = UNNotificationSound.Default;


            var notification = new UILocalNotification();
            // Fire trigger in one seconds
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.1, false);

            var requestID = "WithoutSubscribe";   ///sampleRequest ,      notificationRequest
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            //Here I set the Delegate to handle the user tapping on notification
            // UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

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

        private void SentNotificationForReading(List<Customer> countНotifyReadingustomers)
        {
            if (countНotifyReadingustomers.Count > 0)
            {
                var content = new UNMutableNotificationContent();

                // Set the title and text of the notification:
                content.Title = "Ден на отчитане";

                 List<string> notificationContent = new List<string>();
               // content.Body = string.Empty;

                foreach (var item in countНotifyReadingustomers)
                {
                    // Generate a message summary for the body of the notification:
                    string format = "dd.MM.yyyy";
                    string date = item.StartReportDate.ToString(format);

                    string currentRowContent = ($"Аб. номер: {item.Nomer.ToString()}, {date}" + System.Environment.NewLine);

                    notificationContent.Add(currentRowContent);
                 //   content.Body = ($"Аб. номер: {item.Nomer.ToString()}, {date}" + System.Environment.NewLine);

                }
                string fullContent = string.Empty;
                foreach (var item in notificationContent)
                {
                    fullContent = fullContent + item;
                }

                content.Body = fullContent;

                content.Sound = UNNotificationSound.Default;


                var notification = new UILocalNotification();
                // Fire trigger in one seconds
                var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.1, false);

                var requestID = "reading";   ///sampleRequest ,      notificationRequest
                var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

                //Here I set the Delegate to handle the user tapping on notification
                // UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

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

        private void SentNotificationForOverdue(List<Customer> countНotifyInvoiceOverdueCustomers)
        {
            if (countНotifyInvoiceOverdueCustomers.Count > 0)
            {
                //string countНotifyInvoiceOverdueCustomersAsString = JsonConvert.SerializeObject(countНotifyInvoiceOverdueCustomers);

                // Create content
                var content = new UNMutableNotificationContent();

                content.Title = "Нова Просрочване";

                List<string> notificationContent = new List<string>();

                foreach (var item in countНotifyInvoiceOverdueCustomers)
                {
                    // Generate a message summary for the body of the notification:

                    string format = "dd.MM.yyyy";
                    string date = item.EndPayDate.ToString(format);

                    string currentRowContent = ($"Аб. номер: {item.Nomer.ToString()}, {date}" + System.Environment.NewLine);

                    notificationContent.Add(currentRowContent);
  
                }

                string fullContent = string.Empty;

                foreach (var item in notificationContent)
                {
                    fullContent = fullContent + item;
                }

                content.Body = fullContent;

                content.Sound = UNNotificationSound.Default;


                var notification = new UILocalNotification();
                // Fire trigger in one seconds
                var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.1, false);

                var requestID = "overdue";   ///sampleRequest ,      notificationRequest
                var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

                //Here I set the Delegate to handle the user tapping on notification
                // UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

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


        private void SentNoficationForNewInovoice(List<Customer> countNewНotifyNewInvoiceCustomers)
        {
            if (countNewНotifyNewInvoiceCustomers.Count > 0)
            {
             
                // Create content
                var content = new UNMutableNotificationContent();

                content.Title = "Нова фактура";

                List<string> notificationContent = new List<string>();

                foreach (var item in countNewНotifyNewInvoiceCustomers)
                {
                    // Generate a message summary for the body of the notification:

                    string money = item.MoneyToPay.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("bg-BG"));

                    string currentRowContent = ($"Аб. номер: {item.Nomer.ToString()}, {money}" + Environment.NewLine);

                    notificationContent.Add(currentRowContent);

                    // bulideer.SetContentText($"Аб. номер: {item.Nomer.ToString()}, {money}");
                }

                string fullContent = string.Empty;

                foreach (var item in notificationContent)
                {
                    fullContent = fullContent + item;
                }

                content.Body = fullContent;

                content.Sound = UNNotificationSound.Default;


                var notification = new UILocalNotification();
                // Fire trigger in one seconds
                var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.1, false);   //////////////////false

                var requestID = "newInovoice";   ///sampleRequest ,      notificationRequest
                var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

                //Here I set the Delegate to handle the user tapping on notification
                // UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

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

        private  void SelectWhichCustomersTobeNotified(List<Customer> countНotifyReadingustomers, List<Customer> countНotifyInvoiceOverdueCustomers, List<Customer> countNewНotifyNewInvoiceCustomers, List<Customer> mAllUpdateCustomerFromApi) //mCustomerFromApiNoNotifyToday
        {
            foreach (var customer in mAllUpdateCustomerFromApi)   //// mCustomerFromApiToNotifyToday
            {
                //customer.ReceiveNotifyInvoiceOverdueToday = true;
                //customer.ReceiveNotifyNewInvoiceToday = true;
                //customer.ReciveNotifyReadingToday = true;

                //// isAnyNotifycationCheck
                bool haveToRecieveNotificationToday =
                    (customer.ReceiveNotifyInvoiceOverdueToday == true ||
                    customer.ReceiveNotifyNewInvoiceToday == true ||
                    customer.ReciveNotifyReadingToday == true);

                if (haveToRecieveNotificationToday == true)
                {

                    if (customer.ReceiveNotifyNewInvoiceToday == true && customer.NotifyNewInvoice == true)
                    {
                       // customer.ReceiveNotifyNewInvoiceToday = false;

                        countNewНotifyNewInvoiceCustomers.Add(customer);
                    }
                    if (customer.ReceiveNotifyInvoiceOverdueToday == true && customer.NotifyInvoiceOverdue == true)
                    {
                       // customer.ReceiveNotifyInvoiceOverdueToday = false;

                        countНotifyInvoiceOverdueCustomers.Add(customer);
                    }
                    if (customer.ReciveNotifyReadingToday == true && customer.NotifyReading == true)
                    {
                       // customer.ReciveNotifyReadingToday = false;

                        countНotifyReadingustomers.Add(customer);
                    }
                }
            }
        }


        private void SaveUpdatesInPhone(string updateHour, string updateDate)
        {
            DateTime updateHourAndDate = DateTime.Now;

            string DateFormatt = "HH:mm";
            string format = "dd.MM.yyyy";

            string shortReportDatetHour = updateHourAndDate.ToString(DateFormatt);

           
           updateHour = updateHourAndDate.ToString(DateFormatt);  // + " часа, ";
           updateDate = updateHourAndDate.ToString(format);
            
            //mHour.Text = updateHour;
            //mDate.Text = updateDate;

            // bool isUpdated = true;

            // convert the list to json
            var listOfCustomersAsJson = JsonConvert.SerializeObject(mAllUpdateCustomerFromApi);


            File.WriteAllText(mCustomersFilename, listOfCustomersAsJson);
            File.WriteAllText(mHourFileName, updateHour);
            File.WriteAllText(mDateFileName, updateDate);

        }

        private void GetUpdateDateAndHour(out string updateHour, out string updateDate)
        {
            updateHour = string.Empty;
            updateDate = string.Empty;

            string localParamHour;
            string localParamDate;

            InvokeOnMainThread(() => 
            {
                GetUpdateDateAndHourForMainThread(out localParamHour, out localParamDate);
            });

        }

        private void GetUpdateDateAndHourForMainThread(out string updateHour, out string updateDate)
        {

            DateTime updateHourAndDate = DateTime.Now;

            string DateFormatt = "HH:mm";

            string shortReportDatetHour = updateHourAndDate.ToString(DateFormatt);

            updateHour = updateHourAndDate.ToString(DateFormatt) + " часа, ";
            updateDate = updateHourAndDate.ToShortDateString();

            MHour = updateHour;
            MDate = updateDate;

        }

        private void GetFinalUpdateDateHour()
        {

            MDate = MDate.ToString();
            MHour = MHour.ToString();
        }


    }
}