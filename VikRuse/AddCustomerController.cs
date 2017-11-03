using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;

namespace VikRuse
{
    public partial class AddCustomerController : UIViewController
    {
        private List<Customer> mCustomers;
        private string mBillNumber;
        private string mEgn;

        private bool mSuccessfullyAddCustomer = false;

        private static string mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string  mFilename = Path.Combine(mDocuments, "Customers.txt");
        private string mHourFileName = Path.Combine(mDocuments, "Hour.txt");
        private string mDateFileName = Path.Combine(mDocuments, "Date.txt");

        private string listOfCustomersAsJsonString = string.Empty;

        public AddCustomerController (IntPtr handle) : base (handle)
        {
           
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //UINavigationBar.Appearance.BarTintColor = UIColor.Blue;
            //UINavigationBar.Appearance.TintColor = UIColor.White;

            Error.Hidden = true;

            mCustomers = new List<Customer>();
        }

        partial void AddCustomer_TouchUpInside(UIButton sender)
        {
            Error.Hidden = true;
            //var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var filename = Path.Combine(documents, "Customers.txt");

            // File.WriteAllText(filename, string.Empty);


            //// get customers from file

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

            mBillNumber = (BillNumber.Text);
            mEgn = (Egn.Text);

            if(mCustomers.Count == 0)
            {
                AddOneCustomer();
            }

            if(mCustomers.Count < 5)
            {
                bool isThisCustomerAlredyExist = false;

                foreach (var customer in mCustomers)
                {
                    if(customer.Nomer == mBillNumber)
                    {
                        isThisCustomerAlredyExist = true;

                        InvokeOnMainThread(() => 
                        {
                            Error.Hidden = false;
                            Error.Text = "Абоната е вече добавен";
                        });
 
                    }
                }

                if(isThisCustomerAlredyExist ==false)
                {
                    AddOneCustomer();
                }
            }
            else
            {
                InvokeOnMainThread(() =>
                {
                    Error.Hidden = false;
                    Error.Text = "Можете да добавяте пет абоната";
                });
            }

     

        }

        private void AddOneCustomer()
        {
            if(mBillNumber.ToString().Trim().Length > 3 && mEgn.ToString().Trim().Length > 9)
            {
                EncryptConnection encryp = new EncryptConnection();

                string crypFinalPass = encryp.Encrypt();

                ConnectToApi connectToApi = new ConnectToApi();

                string localParamBillNumber = mBillNumber.ToString();   // to use RefreshErrorAndProgresBarWhenSuccsesfullyAddACustomer

                //check the connection
                bool connection = connectToApi.CheckConnectionOfVikSite();

                // check if connection is ok
                if (connection == true)
                {

                    string realUrl = ConnectToApi.urlAPI + "api/abonats/" + crypFinalPass
                        + "/" + mBillNumber.ToString() + "/" + mEgn.ToString() + "/" + ConnectToApi.updateByAddCutomerButton + "/";

                    var jsonResponse = connectToApi.FetchApiDataAsync(realUrl);

                    //check the api
                    if (jsonResponse == null)
                    {
                        InvokeOnMainThread(() =>
                        {
                            Error.Hidden = false;
                            Error.Text = "Грешка при извличане на данните";
                        });

                        return;
                    }
                    // check in vikSite is there a customer with this billNumber (is billNumber correct)
                    else if (jsonResponse == "[]")
                    {
                        InvokeOnMainThread(() =>
                        {
                            Error.Hidden = false;
                            Error.Text = "Несъщесвуващ абонат";
                        });
                    }

                    // check is billNumber correct and get and save customer in phone
                    else if (jsonResponse != null)
                    {
                        Customer newCustomer = connectToApi.GetCustomerFromApi(jsonResponse);

                        if (newCustomer != null)
                        {
                            mCustomers.Add(newCustomer);

                            // convert the list to json
                            var listOfCustomersAsJson = JsonConvert.SerializeObject(this.mCustomers);

                            //ISharedPreferencesEditor editor = pref.Edit();

                            bool isAddedAnewCustomer = true;
                            bool isAlreadyBeenUpdated = false;

                            string isAddedAnewCustomerAsString = JsonConvert.SerializeObject(isAddedAnewCustomer);
                            string isAlreadyBeenUpdatedAsString = JsonConvert.SerializeObject(isAlreadyBeenUpdated);

                            DateTime updateHourAndDate = DateTime.Now;

                            string DateFormatt = "HH:mm";
                            string format = "dd.MM.yyyy";

                            string shortReportDatetHour = updateHourAndDate.ToString(DateFormatt);


                            var  updateHour = updateHourAndDate.ToString(DateFormatt);  // + " часа, ";
                            var  updateDate = updateHourAndDate.ToString(format);

                            File.WriteAllText(mFilename, listOfCustomersAsJson);
                            File.WriteAllText(mHourFileName, updateHour);
                            File.WriteAllText(mDateFileName, updateDate);

                            ViewController mainScreeen = this.Storyboard.InstantiateViewController("ViewController") as ViewController;

                            if (mainScreeen != null)
                            {
                                
                                this.NavigationController.PushViewController(mainScreeen, true);
                              //  mainScreeen.EmployeesTableView.re
                            }



                                //RunOnUiThread(() =>
                                //{
                                //    RefreshErrorAndProgresBarWhenSuccsesfullyAddACustomer(localParamBillNumber);
                                //});


                            }
                        else
                        {
                            //    RunOnUiThread(() =>
                            //    {
                            //        RefreshErrorAndProgressBarWhenCanNotConnectToApi();
                            //    });
                        }
                    }
                }
                // check if connection is not ok
                else
                {
                    InvokeOnMainThread(() =>
                    {
                        Error.Hidden = false;
                        Error.Text = "Проверете интернет връзката";
                    });

                    return;   // nqma6e return
                }
            }
            else
            {
                InvokeOnMainThread(() =>
                {
                    Error.Hidden = false;
                    Error.Text = "Некоректни данни";
                });
            }  
        }
    }
}