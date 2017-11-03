using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace VikRuse
{
    public class OnTapNotificationCustomEventArgs : EventArgs
    {
        private UIStoryboard mStoryboard;

        private UINavigationController mNavigationController;

      //  public delegate void OnTapNotificationCustomEventArgsHandler(object sender, OnTapNotificationCustomEventArgsHandler e);

        public OnTapNotificationCustomEventArgs()
        {

        }

        public OnTapNotificationCustomEventArgs(UIStoryboard Storyboard, UINavigationController NavigationController)
        {
            MStoryboard = Storyboard;
            MNavigationController = NavigationController;
        }


        public UIStoryboard MStoryboard { get => mStoryboard; set => mStoryboard = value; }
        public UINavigationController MNavigationController { get => mNavigationController; set => mNavigationController = value; }


    }
}