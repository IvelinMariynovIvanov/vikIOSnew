using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;
using iAd;
using System.IO;
using Newtonsoft.Json;

namespace VikRuse
{
    //[JsonObject]
    public  class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public UIStoryboard mStoryboard;

        public UINavigationController mNavigationController;

        private List<UserNotificationCenterDelegate> mNotificationList;

        private UserNotificationCenterDelegate mNotification;

        private static string mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string mNotificationFileName = Path.Combine(mDocuments, "Notification.txt");

        public event EventHandler<OnTapNotificationCustomEventArgs> OnTapNotificationPressed;

        private string mHourFileName = Path.Combine(mDocuments, "Hour.txt");
        private string mDateFileName = Path.Combine(mDocuments, "Date.txt");
        

        public UserNotificationCenterDelegate()
        {
          
        }

        public UserNotificationCenterDelegate(UIStoryboard storyboard, UINavigationController navigationController)
        {
            this.mStoryboard = storyboard;
            this.mNavigationController = navigationController;

            //this.OnTapNotificationPressed += (object sender, OnTapNotificationCustomEventArgs e) =>
            //{
            //    mStoryboard = e.MStoryboard;

            //    mNavigationController = e.MNavigationController;

            //    //var mainActivitiy = mStoryboard.InstantiateViewController("ViewController");
            //    //this.mNavigationController.PushViewController(mainActivitiy, true);

            //};
        }


        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, [BlockProxy(typeof(AdAction))] Action completionHandler)
        {


            if (OnTapNotificationPressed != null)
            {
                OnTapNotificationPressed.Invoke
                    (this, new OnTapNotificationCustomEventArgs(mStoryboard, mNavigationController));
            }

            // Take action based on Action ID
            switch (response.ActionIdentifier)
            {
                case "reply":
                    // Do something
                    break;
                default:
                    // Take action based on identifier
                    if (response.IsDefaultAction)
                    {
                        // Handle default action...

                        DateTime updateHourAndDate = DateTime.Now;

                        string DateFormatt = "HH:mm";
                        string format = "dd.MM.yyyy";

                        string shortReportDatetHour = updateHourAndDate.ToString(DateFormatt);


                       var updateHour = updateHourAndDate.ToString(DateFormatt);  // + " часа, ";
                       var updateDate = updateHourAndDate.ToString(format);

                        File.WriteAllText(mHourFileName, updateHour);
                        File.WriteAllText(mDateFileName, updateDate);
                    }
                    else if (response.IsDismissAction)
                    {
                        // Handle dismiss action
                        //var requests = new string[] { "notificationRequest" };
                        //UNUserNotificationCenter.Current.RemoveDeliveredNotifications(requests);
                    }
                    break;
            }

            completionHandler();
        }

   
        private void GetNotificationSavedInPhone()
        {
            string userNotificationDelegateAsJSON = File.ReadAllText(mNotificationFileName);

            try
            {
                if (userNotificationDelegateAsJSON == null || userNotificationDelegateAsJSON == string.Empty)
                {
                    mNotificationList = new List<UserNotificationCenterDelegate>();
                }
                else
                {
                    mNotificationList = JsonConvert.DeserializeObject<List<UserNotificationCenterDelegate>>(userNotificationDelegateAsJSON);

                    mNotification = mNotificationList[0];
                }

                if (mNotificationList == null)
                {
                    mNotificationList = new List<UserNotificationCenterDelegate>();
                }
            }
            catch (Exception )
            {
                if (userNotificationDelegateAsJSON == null)
                {
                    mNotificationList = new List<UserNotificationCenterDelegate>();
                }
                else
                {
                    mNotificationList = JsonConvert.DeserializeObject<List<UserNotificationCenterDelegate> >(userNotificationDelegateAsJSON);

                    mNotification = mNotificationList[0];
                }

                if (mNotificationList == null)
                {
                    mNotificationList = new List<UserNotificationCenterDelegate>();
                }
            }
        }
       

      
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification


            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.


         //   StateManager.CurrentState = StateManager.State.Foreground;
            completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);

            // completionHandler(UNNotificationPresentationOptions.Alert);
        }
        
    }
}