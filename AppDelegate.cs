using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;
using UIKit;
using UserNotifications;

namespace VikRuse
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        private string categoryID;

        private static System.Timers.Timer aTimer;

        private List<Customer> mGetCustomersFromDbToNotify;  //only with 5 properties which are needed for sending to api
        private List<Customer> mCustomers = new List<Customer>();

        private List<Customer> mCustomerFromApiToNotifyToday = new List<Customer>();
        private List<Customer> mCountНotifyReadingustomers = new List<Customer>();
        private List<Customer> mCountНotifyInvoiceOverdueCustomers = new List<Customer>();
        private List<Customer> mCountNewНotifyNewInvoiceCustomers = new List<Customer>();
        private List<Customer> mAllUpdateCustomerFromApi; 


        private static string mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string mCustomersFilename = Path.Combine(mDocuments, "Customers.txt");
        private string mNotificationFileName = Path.Combine(mDocuments, "Notification.txt");
        private string mHourFileName = Path.Combine(mDocuments, "Hour.txt");
        private string mDateFileName = Path.Combine(mDocuments, "Date.txt");

        private string mUpdateHour = string.Empty;
        private string mUpdateDate = string.Empty;

        // Minimum number of seconds between a background refresh
        // 15 minutes = 15 * 60 = 900 seconds
        private const double MINIMUM_BACKGROUND_FETCH_INTERVAL = 60;  // 900

        // public static UIStoryboard Storyboard1 = UIStoryboard.FromName("MainStoryboard", null);
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }
        private void SetMinimumBackgroundFetchInterval()
        {
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(MINIMUM_BACKGROUND_FETCH_INTERVAL);
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.

            //if (aTimer == null)
            //{
            //    StartTimer();

            //}
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.

            ViewController mainScreeen = new ViewController();

            mainScreeen.ViewDidLoad();



        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.

            //ViewController vc = new ViewController();

            //vc.ViewDidLoad();

        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            //UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(30);

            //// Request notification permissions from the user
            //UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            //{
            //    // Handle approval
            //});

            //// Watch for notifications while the app is active
            //UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

            // return true;


            //RegisterForNotifications();
            //ScheduleNotification();
            ////more code
            ////  return base.FinishedLaunching(application, launchOptions);

            //return true;



            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                    (approved, error) => { });

                // Watch for notifications while app is active
                UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            if (aTimer == null)
            {
                StartTimer();

            }


            return true;
           // return base.FinishedLaunching(application, launchOptions);
        }

        private void StartTimer()
        {
            // Create a timer and set a two second interval.
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 60000;   /// 10 sec

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
        }

        private  void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

            Thread thred = new Thread(AllJobDoneInService);

            thred.Start();

            

        }

        private void AllJobDoneInService()
        {
            //mCountНotifyReadingustomers = new List<Customer>();
            //mCountНotifyInvoiceOverdueCustomers = new List<Customer>();
            //mCountNewНotifyNewInvoiceCustomers = new List<Customer>();

            //mCustomerFromApiToNotifyToday = new List<Customer>();

            mCustomers = new List<Customer>();
            mAllUpdateCustomerFromApi = new List<Customer>();

            // get customers
            // mCustomers = GetCustomersFromPreferences();

            GetCustomersFromPreferences();


            ConnectToApi connectToApi = new ConnectToApi();

            bool connection = connectToApi.CheckConnectionOfVikSite();

            // if (mCustomers.Count > 0)
            //  {
            if (connection == true)
            {
                CheckIfThereisAnewMessageFromApi(connectToApi);

                foreach (var customer in mCustomers)
                {
                    bool isReceiveNotifyNewInvoiceCheck = false;
                    bool isReceiveNotifyInvoiceOverdueCheck = false;
                    bool isReciveNotifyReadingCheck = false;

                    isReceiveNotifyNewInvoiceCheck = customer.NotifyNewInvoice;
                    isReceiveNotifyInvoiceOverdueCheck = customer.NotifyInvoiceOverdue;
                    isReciveNotifyReadingCheck = customer.NotifyReading;


                    EncryptConnection encryp = new EncryptConnection();


                    string crypFinalPass = encryp.Encrypt();


                    // check if connection is ok
                    //  if (isAnyNotifycationCheck == true)
                    //   {
                    string billNumber = customer.Nomer;
                    string egn = customer.EGN;

                    //CREATE URL
                    // string url = "http://192.168.2.222/VIKWebApi/";


                    /// !!!!!!!!!!!!!!!!!!!!! here
                    //string realUrl = ConnectToApi.urlAPI + "api/abonats/" + crypFinalPass + "/" + billNumber + "/" + egn;

                    string realUrl = ConnectToApi.urlAPI + "api/abonats/" + crypFinalPass + "/" + billNumber + "/" + egn + "/"
                                   + ConnectToApi.updateByAutoService + "/";
                                  // + isReceiveNotifyNewInvoiceCheck + "/" + isReceiveNotifyInvoiceOverdueCheck + "/" + isReciveNotifyReadingCheck + "/";

                    //string realUrl = "http://192.168.2.222/VIKWebApi/" + "api/abonats/"
                    //   + crypFinalPass + "/" + billNumber + "/" + egn + "/" + ConnectToApi.updateByAutoService + "/"
                    //   + isReceiveNotifyNewInvoiceCheck + "/" + isReceiveNotifyInvoiceOverdueCheck + "/" + isReciveNotifyReadingCheck + "/";

                    var jsonResponse = connectToApi.FetchApiDataAsync(realUrl); //FetchApiDataAsync(realUrl);

                    //check the api
                    if (jsonResponse == null)
                    {
                        return;
                    }
                    // check in vikSite is there a customer with this billNumber (is billNumber correct)
                    else if (jsonResponse == "[]")
                    {
                        return;  ////

                    }

                    // check if billNumber is correct and get and save customer in phone
                    else if (jsonResponse != null)
                    {
                        Customer updateCutomerButNoNotify = connectToApi.GetCustomerFromApi(jsonResponse);

                        if (updateCutomerButNoNotify != null && updateCutomerButNoNotify.IsExisting == true)
                        //  (newCustomer != null && newCustomer.IsExisting == true)
                        {

                            updateCutomerButNoNotify.NotifyNewInvoice = customer.NotifyNewInvoice;
                            updateCutomerButNoNotify.NotifyInvoiceOverdue = customer.NotifyInvoiceOverdue;
                            updateCutomerButNoNotify.NotifyReading = customer.NotifyReading;

                            mAllUpdateCustomerFromApi.Add(updateCutomerButNoNotify);
                        }

                        else
                        {
                            return;
                        }
                    }
                }


                SelectWhichCustomersTobeNotified(mCountНotifyReadingustomers, mCountНotifyInvoiceOverdueCustomers, mCountNewНotifyNewInvoiceCustomers, mAllUpdateCustomerFromApi); // mCustomerFromApiToNotifyToday

                //SaveCustomersFromApiInPhone();


                SaveUpdatesInPhone();

                //Looper.Prepare();

                //MyNotification myNotification = new MyNotification(this);

                //myNotification.SentNotificationForOverdue(mCountНotifyInvoiceOverdueCustomers);


                SentNotificationForOverdue(mCountНotifyInvoiceOverdueCustomers);

                SentNoficationForNewInovoice(mCountNewНotifyNewInvoiceCustomers);

                SentNotificationForReading(mCountНotifyReadingustomers);
            }


            //ViewController vc = new ViewController();

            //vc.ViewDidLoad();

            //Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }
        private void SaveUpdatesInPhone()  //// string updateHour, string updateDate - parametri
        {
            DateTime updateHourAndDate = DateTime.Now;

            string DateFormatt = "HH:mm";
            string format = "dd.MM.yyyy";

            string shortReportDatetHour = updateHourAndDate.ToString(DateFormatt);

            
            mUpdateHour = updateHourAndDate.ToString(DateFormatt);  // + " часа, ";
            mUpdateDate = updateHourAndDate.ToString(format);

            //mHour.Text = updateHour;
            //mDate.Text = updateDate;

            // bool isUpdated = true;

            // convert the list to json
            var listOfCustomersAsJson = JsonConvert.SerializeObject(mAllUpdateCustomerFromApi);


            File.WriteAllText(mCustomersFilename, listOfCustomersAsJson);
            File.WriteAllText(mHourFileName, mUpdateHour ); // updateHour
            File.WriteAllText(mDateFileName, mUpdateDate ); // updateDate

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

        private void SelectWhichCustomersTobeNotified(List<Customer> countНotifyReadingustomers, List<Customer> countНotifyInvoiceOverdueCustomers, List<Customer> countNewНotifyNewInvoiceCustomers, List<Customer> mAllUpdateCustomerFromApi) //mCustomerFromApiNoNotifyToday
        {
            foreach (var customer in mAllUpdateCustomerFromApi)   //// mCustomerFromApiToNotifyToday
            {
            //    customer.ReceiveNotifyInvoiceOverdueToday = true;
            //    customer.ReceiveNotifyNewInvoiceToday = true;
            //    customer.ReciveNotifyReadingToday = true;

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
                      //  customer.ReceiveNotifyInvoiceOverdueToday = false;

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

        private void CheckIfThereisAnewMessageFromApi(ConnectToApi connectToApi)
        {
            EncryptConnection encryp = new EncryptConnection();


            string crypFinalPass = encryp.Encrypt();

            //// get message from preferences
            GrudMessageFromPreferemces grudMessage = new GrudMessageFromPreferemces();

            int lastMessageId = grudMessage.GetMessageFromPreferencesInPhone().MessageID;

            //realno !!!!!!!!!!!!!
            string messageUrl = ConnectToApi.urlAPI + "api/msg/";

            ///testovo
        //    string messageUrl = ConnectToApi.wtf + "api/msg/";

            string finalUrl = messageUrl + crypFinalPass + "/" + lastMessageId;

            var messageFromApiAsJsonString = connectToApi.FetchApiDataAsync(finalUrl);

            // check api response
            if (messageFromApiAsJsonString != null)
            {
                Message newMessage = new Message();

                newMessage = connectToApi.GetMessageFromApi(messageFromApiAsJsonString);

                if (newMessage.MessageID > lastMessageId)
                {
                    grudMessage.SaveMessageInPhone(newMessage);

                    //   SaveMessageInPhone(newMessage);

                    int messagesCount = newMessage.Messages.Count;

                    if (messagesCount > 0)
                    {
                        SentNotificationWithoutSubscribe(newMessage);
                    }
                }
            }
        }

        private  void GetCustomersFromPreferences()
        {
            var listOfCustomersAsJsonString = File.ReadAllText(mCustomersFilename);

            try
            {
      
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

        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            // Do Background Fetch
            var downloadSuccessful = false;

            try
            {
                // Download data
            //    await Client.Instance.BeerDrinkinClient.RefreshAll();
                downloadSuccessful = true;
            }
            catch (Exception ex)
            {
                //Insights.Report(ex);
            }

            if (downloadSuccessful)
            {
                completionHandler(UIBackgroundFetchResult.NewData);
            }
            else
            {
                completionHandler(UIBackgroundFetchResult.Failed);
            }
        }

        void RegisterForNotifications()
        {
            // Create action
            var actionID = "check";
            var title = "Check";
            var action = UNNotificationAction.FromIdentifier(actionID, title, UNNotificationActionOptions.Foreground);

            // Create category
            categoryID = "notification";
            var actions = new UNNotificationAction[] { };
            var intentIDs = new string[] { };
            //var categoryOptions = new UNNotificationCategoryOptions[] { };
            var category = UNNotificationCategory.FromIdentifier(categoryID, actions, intentIDs, UNNotificationCategoryOptions.None);

            // Register category
            var categories = new UNNotificationCategory[] { category };
            UNUserNotificationCenter.Current.SetNotificationCategories(new NSSet<UNNotificationCategory>(categories));

            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert|UNAuthorizationOptions.Badge| UNAuthorizationOptions.Sound,
            (a, err) => 
            {
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

            // Fire trigger in one seconds
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);

            var requestID = "notificationRequest";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            //Here I set the Delegate to handle the user tapping on notification
         //   UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => {
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