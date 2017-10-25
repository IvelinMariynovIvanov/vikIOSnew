using Foundation;
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

       // public static UIStoryboard Storyboard1 = UIStoryboard.FromName("MainStoryboard", null);
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method


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


            return true;
           // return base.FinishedLaunching(application, launchOptions);
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

            // Fire trigger in one seconds
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);

            var requestID = "notificationRequest";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            //Here I set the Delegate to handle the user tapping on notification
            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

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
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}