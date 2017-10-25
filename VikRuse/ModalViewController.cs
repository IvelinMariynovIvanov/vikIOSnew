using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;

namespace VikRuse
{

    public class OnEditCustomerEventArgs : EventArgs
    {
        private bool isThereANewCharge;
        private bool isThereALateBill;
        private bool isThereAReport;
        private int currentPossition;

        private int possUp;
        private int possDown;

        public OnEditCustomerEventArgs()
        {

        }

        public OnEditCustomerEventArgs(bool isThereANewCharge, bool isThereALateBill, bool isThereAReport, int currentPossition)
        {
            this.IsThereANewCharge = isThereANewCharge;
            this.IsThereALateBill = isThereALateBill;
            this.IsThereAReport = isThereAReport;
            this.CurrentPossition = currentPossition;
        }

        public bool IsThereANewCharge { get => isThereANewCharge; set => isThereANewCharge = value; }
        public bool IsThereALateBill { get => isThereALateBill; set => isThereALateBill = value; }
        public bool IsThereAReport { get => isThereAReport; set => isThereAReport = value; }
        public int CurrentPossition { get => currentPossition; set => currentPossition = value; }

        public int PossUp { get => possUp; set => possUp = value; }
        public int PossDown { get => possDown; set => possDown = value; }

    }

    public partial class ModalViewController : UIViewController
    {
        private static string mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string mFilename = Path.Combine(mDocuments, "Customers.txt");
        private List<Customer> mCustomers;
       

        #region Fields

        //private bool mNewCharge;
        //private bool mLateCharge;
        //private bool mReport;

        private UIButton mEdit;
        private UIButton mCancel;

       // public int mPossUp;   //imageview
      //  public int mPossDown;
    //    public int mCurrentPoss;    // textview

        public int mCurrentPosition;
        public int mCustomresCount;



        public bool mIsNewCharge;
        public bool mIsLateCharge;
        public bool mIsReport;

        public int MCurrentPosition { get => mCurrentPosition; set => mCurrentPosition = value; }
        public int MCustomresCount { get => mCustomresCount; set => mCustomresCount = value; }
        public bool MIsNewCharge { get => mIsNewCharge; set => mIsNewCharge = value; }
        public bool MIsLateCharge { get => mIsLateCharge; set => mIsLateCharge = value; }
        public bool MIsReport { get => mIsReport; set => mIsReport = value; }

        #endregion


        public event EventHandler<OnEditCustomerEventArgs> OnEditCustomerComplete;

        public ModalViewController (IntPtr handle) : base (handle)
        {
           
        }

        public ModalViewController(int mCurrentPosition, int mCustomresCount, bool isNewCharge, bool isLateCharge, bool isReport) //: base()
        {
            this.MCurrentPosition = mCurrentPosition;
            this.MCustomresCount = mCustomresCount;
            this.MIsNewCharge = isNewCharge;
            this.MIsLateCharge = isLateCharge;
            this.MIsReport = isReport;



            mNewCharge = new UISwitch();
            mLateCharge = new UISwitch();
            mReport = new UISwitch();
        }

        public ModalViewController()
        {

            mNewCharge = new UISwitch();
            mLateCharge = new UISwitch();
            mReport = new UISwitch();
        }



        public override void ViewDidLoad()
        {


            base.ViewDidLoad();

            //mPossUp.SetImage(UIImage.FromFile("up.png"), UIControlState.Normal);

            //mPossDown.SetImage(UIImage.FromFile("down.png"), UIControlState.Normal);

            //mNewCharge = new UISwitch();
            //mLateCharge = new UISwitch();
            //mReport = new UISwitch();

            View.BackgroundColor = UIColor.Black.ColorWithAlpha(0.3f);
            View.Opaque = false;

            var listOfCustomersAsJsonString = File.ReadAllText(mFilename);

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



            //  #region check Ckeckbox Status

            //  ChechSwitchStatus();
            // #endregion

            ChechSwitchStatus();

            AttachEvents();

            CheckPossition();
        }

        private void CheckPossition()
        {
            if (mCurrentPosition == 0)  // check if the first position can be move up
            {
                mCurrentPoss.Text = (mCurrentPosition + 1).ToString();
                mPossUp.Hidden = true; //ViewStates.Invisible;
                mPossDown.Hidden = false;// ViewStates.Visible;

            }

            if (mCurrentPosition == mCustomresCount - 1) // check if the last position can be move down
            {
                mCurrentPoss.Text = (mCurrentPosition + 1).ToString();
                mPossDown.Hidden = true;// ViewStates.Invisible;

            }

            if (mCurrentPosition != 0 && mCurrentPosition != mCustomresCount - 1)
            {
                mCurrentPoss.Text = (mCurrentPosition + 1).ToString();
                mPossUp.Hidden = false;// ViewStates.Visible;
                mPossDown.Hidden = false; //ViewStates.Visible;
            }
        }

        private void ChechSwitchStatus()
        {
            if (MIsNewCharge == true)
            {
                mNewCharge.On = true;
            }
            else if (MIsNewCharge == false)
            {
                mNewCharge.On = false;
            }

            if (MIsLateCharge == true)
            {
                mLateCharge.On = true;
            }
            else if (MIsLateCharge == false)
            {
                mLateCharge.On = false;
            }

            if (MIsReport == true)
            {
                mReport.On = true;
            }
            else if (MIsReport == false)
            {
                mReport.On = false;
            }
        }

        private void AttachEvents()
        {
            mNewCharge.TouchUpInside += MNewCharge_TouchUpInside;
            mLateCharge.TouchUpInside += MLateCharge_TouchUpInside;
            mReport.TouchUpInside += MReport_TouchUpInside;
            mPossUp.TouchUpInside += MPossUp_TouchUpInside;
            mPossDown.TouchUpInside += MPossDown_TouchUpInside;
        }

        private void MPossDown_TouchUpInside(object sender, EventArgs e)
        {
            mCurrentPoss.Text = ((++mCurrentPosition) + 1).ToString();

            if (mCurrentPosition == 0)
            {
                mPossUp.Hidden = true;
                mPossDown.Hidden = false;
            }
            else if (mCurrentPosition != 0 && mCurrentPosition != mCustomresCount - 1)
            {
                mPossUp.Hidden = false;
                mPossDown.Hidden = false;
            }
            else if (mCurrentPosition == mCustomresCount - 1)
            {
                mPossDown.Hidden = true;
                mPossUp.Hidden = false;
            }
        }

        private void MPossUp_TouchUpInside(object sender, EventArgs e)
        {
            mCurrentPoss.Text = ((--mCurrentPosition) + 1).ToString();

            if (mCurrentPosition == 0)
            {
                mPossUp.Hidden = true;
                mPossDown.Hidden = false;

            }
            else if (mCurrentPosition != 0 && mCurrentPosition != mCustomresCount - 1)
            {
                mPossUp.Hidden = false;
                mPossDown.Hidden = false;
            }
            else if (mCurrentPosition == mCustomresCount - 1)
            {
                mPossDown.Hidden = true;
            }
        }

        private void MReport_TouchUpInside(object sender, EventArgs e)
        {
          if(mReport.On == true)
            {
                mIsReport = true;
            }
          else
            {
                mIsReport = false;
            }
        }

        private void MLateCharge_TouchUpInside(object sender, EventArgs e)
        {
           if(mLateCharge.On == true)
            {
                mIsLateCharge = true;
            }
           else
            {
                mIsLateCharge = false;
            }
        }

        private void MNewCharge_TouchUpInside(object sender, EventArgs e)
        {
          if(mNewCharge.On == true)
            {
                mIsNewCharge = true;
            }
          else
            {
                mIsNewCharge = false;
            }
        }

        private void TapAction(UITapGestureRecognizer sender)
        {
            DismissViewController(true, null);
        }

        

        partial void Cancel_TouchUpInside(UIButton sender)
        {
            DismissViewController(true, null);
        }

        partial void Save_TouchUpInside(UIButton sender)
        {

            if (OnEditCustomerComplete != null)
            {
                OnEditCustomerComplete.Invoke
               (this, new OnEditCustomerEventArgs(mIsNewCharge, mIsLateCharge, mIsReport, mCurrentPosition));
            }

            DismissViewController(true, null);

            //var currentCustomer = mCustomers[MCurrentPosition];

            //currentCustomer.NotifyNewInvoice = this.MIsNewCharge;
            //currentCustomer.NotifyInvoiceOverdue = this.MIsLateCharge;
            //currentCustomer.NotifyReading = this.MIsReport;

            //Customer updateCustomer = mCustomers[MCurrentPosition];

            //updateCustomer.NotifyNewInvoice = this.MIsNewCharge;
            //updateCustomer.NotifyInvoiceOverdue = this.MIsLateCharge;
            //updateCustomer.NotifyReading = this.MIsReport;



            //mCustomers.RemoveAt(MCurrentPosition); //new count -1
            //mCustomers.Insert(MCurrentPosition, updateCustomer); // put in the same posstion

            //var listOfCustomersAsJson = JsonConvert.SerializeObject(this.mCustomers);
            //File.WriteAllText(mFilename, listOfCustomersAsJson);
        }
    }
}