using Foundation;
using System;

using UIKit;




using System.Net.Http;
using System.Net;
using System.IO;
using BigTed;
using System.Threading;
using Xamarin.Forms;

namespace VikRuse
{
    public partial class SignalForAccidentController : UIViewController
    {
        #region Fields
        private  UIImagePickerController imagePickerForGallery;

        private UIImagePickerController imagePickerForCamera;

        private bool selectImageFromCamera = false;

        private string apiCommand = "api/postimage";

       // private UIAlertView alert;

        private MultipartFormDataContent form;

        private HttpClient client;

        private  HttpResponseMessage response;  //HttpResponseMessage

        private HttpClientHandler handler ;

        private CookieContainer cookies;

        private bool connection;

        private bool isDataValidated = false;

        private Stream stream;

        private string mFinalCryptPassword;

     //   private bool mIsFromGalleryPressed;

      //  private Uri mSaveImageUri; // Uri

        private bool isPicImagNull;


        private UINavigationItem mNavBar { get; set; }
        public UITextField MCity { get => this.mCity; set => this.mCity = value; }
        public UITextField MAddress { get => this.mAddress; set => this.mAddress = value; }
        public UITextField MDescription { get => this.mDescription; set => this.mDescription = value; }
        public UITextField MFullName { get => this.mFullName; set => this.mFullName = value; }
        public UITextField MPhoneNumber { get => this.mPhoneNumber; set => this.mPhoneNumber = value; }
        public UIImageView Pic { get => this.pic; set => this.pic = value; }

#endregion

        public SignalForAccidentController (IntPtr handle) : base (handle)
        {

        }

        public SignalForAccidentController(UINavigationItem mNavBar)
        {
            this.mNavBar = mNavBar;
        }

        public SignalForAccidentController()
        {

        }

        //public override void ViewDidAppear(bool animated)
        //{
        //    base.ViewDidAppear(animated);


        //    //choosePhotoButton.TouchUpInside += (s, e) =>
        //    //{
        //    //    SelectImageFromGallery();
        //    //};

        //    //CameraButton.TouchUpInside += SelectImageFromCamera;
        //}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            form = new MultipartFormDataContent();
            
            handler = new HttpClientHandler();

            cookies = new CookieContainer();


            response = new HttpResponseMessage();//HttpResponseMessage();   ///NSHttpUrlResponse

            client = new HttpClient(handler, false);

            addPicFromGalary.TouchUpInside += (s, e) =>
            {
                SelectImageFromGallery();
            };

            addPicFromCamera.TouchUpInside += SelectImageFromCamera;

            Sent.TouchUpInside += (s, e) =>
            {
                Sent_Click();
            };

            mError.Hidden = true;

            DismissKeyboardFromAllFields();

           // pic = null;
        }

        private void DismissKeyboardFromAllFields()
        {
            this.MCity.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();

                return true;
            };

            this.MAddress.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            this.MDescription.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            this.MFullName.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            this.MPhoneNumber.ShouldReturn += (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            // this.MPhoneNumber.ResignFirstResponder();

            var g = new UITapGestureRecognizer(() => View.EndEditing(true));
            g.CancelsTouchesInView = false; //for iOS5
            View.AddGestureRecognizer(g);

        }

        private void Sent_Click()
        {
            
           
            InvokeOnMainThread(() =>
            {
                mError.Text = string.Empty;
                mError.Hidden = true;

                Showrogress();
            });


            Thread thread = new Thread(SentAccindentSignelToApiAsync);
            thread.Start();

        }

    

        private void  SentAccindentSignelToApiAsync()
        {
            ConnectToApi connectToApi = new ConnectToApi();

             connection = connectToApi.CheckConnectionOfVikSite();
          //  connection = false;
        //    bool isDataValidated = false;

            InvokeOnMainThread(() =>
            {
            isDataValidated =

            MCity.Text.Trim().Length > 0
            && MAddress.Text.Trim().Length > 0
            && MDescription.Text.Trim().Length > 0
            && MPhoneNumber.Text.Trim().Length > 0
            && MFullName.Text.Trim().Length > 0;
            });

            if (isDataValidated)
            {
                #region old stuff
                //// casting imageview to bitmap
                //Android.Graphics.Drawables.BitmapDrawable bd =
                //    (Android.Graphics.Drawables.BitmapDrawable)pic.Drawable;

                //Android.Graphics.Bitmap bitmap = bd.Bitmap;

                //using (var stream = new MemoryStream())
                //{
                //    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                //    PostItem(stream);
                //}
                #endregion
                if (connection == true)
                {
 
                    InvokeOnMainThread(() => 
                    {
                        //Showrogress();
             
                           if (Pic.Image == null)
                            {
                                isPicImagNull = true;
                            }
                           else
                            {
                            isPicImagNull = false;
                            }
                    });
                    //Thread thread = new Thread(Showrogress);

                    //thread.Start();
                    // if (mSaveImageUri != null)
                    if (isPicImagNull == false)  // Pic.Image != null
                    {

                        //var stream = new StreamReader(stream);

                        //Stream stream = ContentResolver.OpenInputStream(mSaveImageUri);

                        // NSInputStream stream = new NSInputStream(pic.Image);


                        //stream = Pic.Image.AsJPEG().AsStream();   //AsPNG().AsStream();

                        //InvokeOnMainThread(()=>
                        //    {
                        //        stream = Pic.Image.AsJPEG().AsStream();   //AsPNG().AsStream();
                        //    }
                        // );

                        //NSInputStream stream =  new NSInputStream(mSaveImageUri);

                        InvokeOnMainThread(() =>
                        {
                            stream = Pic.Image.AsJPEG().AsStream();   //AsPNG().AsStream();
                            PostAccidentToDB(stream);
                        }
                        );

                        //PostAccidentToDB(stream);

                    }
                    //else if(pic.Image == null)
                    else
                    {
                        stream = null;
                        InvokeOnMainThread(() =>
                        {
                            PostAccidentToDB(stream);
                        }
                       );
                        // PostAccidentToDB(stream);

                    }
                }
                else
                {
                    // InvokeOnMainThread(() => RefreshProgressDialogAndToastWhenThereIsNoInternet());
                    InvokeOnMainThread(() =>
                    {              
                        mError.Hidden = false;
                        mError.Text = "Проверете интернет връзката";

                        BTProgressHUD.Dismiss();
                    });
                }
            }
            else
            {

                InvokeOnMainThread(() =>
                {
                    UpdateError();
                });
   
            }
        }

        private  void Showrogress()
        {
            BTProgressHUD.ShowContinuousProgress("Изпращане на сигнал ...", ProgressHUD.MaskType.Black);
        }


        private void UpdateError()
        {
            mError.Text = "Попълнете задължителните полета";
            mError.Hidden = false;

            BTProgressHUD.Dismiss();
        }

        private object RefreshProgressDialogAndToastWhenThereIsNoInternet()
        {
            throw new NotImplementedException();
        }

        private void PostAccidentToDB(Stream stream)
        {
            //handler = new HttpClientHandler();

            //cookies = new CookieContainer();

            //response = new HttpResponseMessage();

            handler.CookieContainer = cookies;



            //    client = new System.Net.Http.HttpClient(handler, false);
            //using(client)
            {
                try
                {

                    EncryptConnection encryp = new EncryptConnection();

                    mFinalCryptPassword = encryp.Encrypt();



                    //  InvokeOnMainThread(() =>
                     // {
                    //   MultipartFormDataContent form = new MultipartFormDataContent();

                    if (stream != null)
                    {

                        //  ByteArrayContent streamm = new ByteArrayContent();

                        StreamContent imageContent = new StreamContent(stream);
                        form.Add(imageContent, "image", "image.jpg");
                    }

                //    InvokeOnMainThread(() =>
                  //  {
                        StringContent number = new StringContent(MPhoneNumber.Text.ToString());
                        StringContent description = new StringContent(MDescription.Text.ToString());
                        StringContent city = new StringContent(MCity.Text.ToString());
                        StringContent address = new StringContent(MAddress.Text.ToString());
                        StringContent name = new StringContent(MFullName.Text.ToString());

                        StringContent key = new StringContent(mFinalCryptPassword);


                        form.Add(number, "number");
                        form.Add(description, "description");
                        form.Add(city, "city");
                        form.Add(address, "address");
                        form.Add(name, "name");
                        form.Add(key, "key");
                    //   });

                    //   });

                    /////////////////real url
                    //client.DownloadFileTaskAsync(new Uri(DownloadUrl), FileName);
                    response = client.PostAsync(ConnectToApi.urlAPI + apiCommand, form).Result;

                    // test url

                    //< key > NSAllowsArbitraryLoads </ key >
                    //< true />   

                    //string testUrl = "http://192.168.2.222/VIKWebApi/api/postimage";
                    //response = client.PostAsync(testUrl, form).Result;
                    //  response= await client.SendAsync(request);


                    if (response.StatusCode == HttpStatusCode.OK)  // response.IsSuccessStatusCode
                    {

                        InvokeOnMainThread(() =>
                        {
                            BTProgressHUD.Dismiss();
                            BTProgressHUD.Dismiss();
                            BTProgressHUD.Dismiss();

                            ViewController mainScreeen = this.Storyboard.InstantiateViewController("ViewController") as ViewController;
                            mainScreeen.mToastText = "сигнал";  //Успешно изпратихте сигнал

                            if (mainScreeen != null)
                            {
                                this.NavigationController.PushViewController(mainScreeen, true);
                            }

                        });

                        // BTProgressHUD.Dismiss();

                        //// var var =  BTProgressHUD.ShowToast("Успешно изпратихте сигнал", false, 3000);

                        // var mainActivitiy = Storyboard.InstantiateViewController("ViewController");

                        // this.NavigationController.PushViewController(mainActivitiy, true);

                        // InvokeOnMainThread(() =>
                        // {
                        //      BTProgressHUD.ShowToast("Успешно изпратихте сигнал", false, 3000);
                        //     //ShowToast("Успешно изпратихте сигнал", View);
                        // });

                        // BTProgressHUD.Dismiss();
                    }
                    //HttpResponseMessage response = 
                    //   client.PostAsync("http://192.168.2.222/VIKWebApi/api/postimage", form).Result;
                    else
                    {
                        InvokeOnMainThread(() =>
                        {
                                mError.Text = "Възникна грешка при изпращане";
                                mError.Hidden = false;

                                BTProgressHUD.Dismiss();
                        }
                        );
                    }

                }
                catch (Exception )
                {
                    InvokeOnMainThread(() =>
                    {

                         mError.Text = "Възникна грешка при изпращане";
                       // mError.Text = $"{ex.ToString()}";
                        mError.Hidden = false;

                        BTProgressHUD.Dismiss();
                    }
                    );
                }
            }
        }

        public void ShowToast(String message, UIView view)
        {
            UIView residualView = view.ViewWithTag(1989);
            if (residualView != null)
                residualView.RemoveFromSuperview();

            var viewBack = new UIView(new CoreGraphics.CGRect(83, 0, 300, 100));
            viewBack.BackgroundColor = UIColor.Black;
            viewBack.Tag = 1989;
            UILabel lblMsg = new UILabel(new CoreGraphics.CGRect(0, 20, 300, 60));
            lblMsg.Lines = 1;
            lblMsg.Text = message;
            lblMsg.TextColor = UIColor.White;
            lblMsg.TextAlignment = UITextAlignment.Center;
            viewBack.Center = view.Center;
            viewBack.AddSubview(lblMsg);
            view.AddSubview(viewBack);
            //roundtheCorner(viewBack);
            UIView.BeginAnimations("Toast");   ///Успешно изпратихте сигнал
            UIView.SetAnimationDuration(3.0f);
            viewBack.Alpha = 0.0f;
            UIView.CommitAnimations();
        }
        private void ShowProgressDialog()
        {
            throw new NotImplementedException();
        }

        private void UnlockSentButton()
        {
            if (mCity.Text.Trim().Length > 0
             && mAddress.Text.Trim().Length > 0
             && mDescription.Text.Trim().Length > 0
             && mPhoneNumber.Text.Trim().Length > 0)
            {
                Sent.Enabled = true;
            }
            else
            {
                Sent.Enabled = false;
            }
        }

        private void SelectImageFromCamera(object sender, EventArgs e)
        {
            selectImageFromCamera = true;

            imagePickerForCamera = new UIImagePickerController();
           // imagePicker.PrefersStatusBarHidden();

            imagePickerForCamera.SourceType = UIImagePickerControllerSourceType.Camera;

            // set what media types
            imagePickerForCamera.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.Camera);

            //Add event handlers when user finished Capturing image or Cancel
            imagePickerForCamera.FinishedPickingMedia += Handle_FinishedPickingMedia;
            imagePickerForCamera.Canceled += ImagePickerForCamera_Canceled;
           // imagePickerForCamera.Canceled += Handle_Canceled;


            // show the picker
            NavigationController.PresentModalViewController(imagePickerForCamera, true);
        }

        private void ImagePickerForCamera_Canceled(object sender, EventArgs e)
        {
            imagePickerForCamera.DismissModalViewController(true);

            selectImageFromCamera = false;
        }

        private void SelectImageFromGallery()
        {
            // create a new picker controller
            imagePickerForGallery = new UIImagePickerController();

            // set our source to the photo library
            imagePickerForGallery.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;

            // set what media types
            imagePickerForGallery.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

            imagePickerForGallery.FinishedPickingMedia += Handle_FinishedPickingMedia;
            imagePickerForGallery.Canceled += ImagePickerForGallery_Canceled;
          //  imagePickerForGallery.Canceled += Handle_Canceled;

            // show the picker
            NavigationController.PresentModalViewController(imagePickerForGallery, true);
            //UIPopoverController picc = new UIPopoverController(imagePicker);
        }

        private void ImagePickerForGallery_Canceled(object sender, EventArgs e)
        {
            imagePickerForGallery.DismissModalViewController(true);
        }

        //private void Handle_Canceled(object sender, EventArgs e)
        //{
        //    if(selectImageFromCamera == true)
        //    {
        //        imagePickerForCamera.DismissModalViewController(true);
        //    }
        //    else
        //    imagePickerForGallery.DismissModalViewController(true);
        //}

        protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            // determine what was selected, video or image
            bool isImage = false;

            switch (e.Info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image":
                    Console.WriteLine("Image selected");
                    isImage = true;
                    break;
                case "public.video":
                    Console.WriteLine("Video selected");
                    break;
            }

            // get common info (shared between images and video)
            NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
            if (referenceURL != null)
                Console.WriteLine("Url:" + referenceURL.ToString());

            // if it was an image, get the other image info
            if (isImage)
            {
                // get the original image
                UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;

                var fdsf = e.Info[UIImagePickerController.ImageUrl] ;

                if (originalImage != null)
                {
                    // do something with the image
                    Console.WriteLine("got the original image");
                    Pic.Image = originalImage; // display

                    //Uri result;
                    //Uri.TryCreate(Pic.Image.ToString(), result)

                   // mSaveImageUri = fdsf ;
                }
            }

            // dismiss the picker

            if (selectImageFromCamera == true)
            {
                imagePickerForCamera.DismissModalViewController(true);
   
            }
            else
            {
                imagePickerForGallery.DismissModalViewController(true);
            }

            selectImageFromCamera = false;
        }

       
    }
}