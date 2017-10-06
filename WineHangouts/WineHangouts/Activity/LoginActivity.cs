using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Hangout.Models;
using Android.Telephony;
using Android.Gms.Common;
using Android.Views;
using System.Diagnostics;
using Java.Util;
using Android.Graphics.Drawables;
using ZXing.Mobile;
using AndroidHUD;
using System.Net;
using Android.Graphics;

namespace WineHangouts

{

    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/Icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        public string otp = "";
        private int screenid = 1;
        public Button BtnLogin;
        public Button BtnResend;
        public Button BtnContinue;
        public Button update;
        public Button BtnUpdateEmail;
        public string gplaystatus = "";
        public TextView TxtScanresult;
        public EditText txtmail;
        public TextView Txtem;

        ServiceWrapper svc = new ServiceWrapper();
        CustomerResponse AuthServ = new CustomerResponse();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CheckInternetConnection();
            Stopwatch st = new Stopwatch();
            st.Start();
            //for direct login
            //CurrentUser.SaveUserName("Mohana","48732");
            //Preinfo("8902519310330");
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginView);
            var TaskA = new System.Threading.Tasks.Task(() =>
            {
                BlobWrapper.DownloadImages(Convert.ToInt32(CurrentUser.getUserId()));
            });
            TaskA.Start();
            //checking user id's exist or not.
            if ((CurrentUser.getUserId() != null || CurrentUser.getUserId() != "0") && (CurrentUser.getUserId()!= null) )
            {
                IntoApp();
            }
            else if (CurrentUser.GetGuestId() != null || CurrentUser.getUserId() == "0")
            {
                IntoApp();
            }
            else
            { 
                //if (CurrentUser.GetCardNumber() != null)
                //{
                //    Preinfo(CurrentUser.GetCardNumber());
                //}
                ImageButton BtnScanner = FindViewById<ImageButton>(Resource.Id.btnScanner);
                Button BtnGuestLogin = FindViewById<Button>(Resource.Id.btnGuestLogin);
                LoggingClass.LogInfo("Opened the app", screenid);
                BtnScanner.Click +=  delegate
                {
                   
                    try
                    {
                        MobileBarcodeScanner.Initialize(Application);
                        var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                        scanner.UseCustomOverlay = false;
                        var result ="8902519310330";// "900497354894";//await scanner.Scan();
                        if (result != null)
                        {
                            LoggingClass.LogInfo("User Tried to login with " + result, screenid);
                            Preinfo(result);
                            CurrentUser.SaveCardNumber(result);
                            txtmail.Visibility = ViewStates.Gone;
                            Txtem.Visibility = ViewStates.Gone;
                        }
                    }
                    catch (Exception exe)
                    {
                        LoggingClass.LogError(exe.Message, screenid, exe.StackTrace);
                    } 
                };

                BtnGuestLogin.Click += async delegate
                {
                    Intent intent = new Intent(this, typeof(Login));
                    ProgressIndicator.Show(this);
                    LoggingClass.LogInfo("User Tried to login with Guest Login ", screenid);
                    StartActivity(intent);
                    CustomerResponse csr = new CustomerResponse();
                    csr = await svc.InsertUpdateGuest("Didn't get the token");
                    CurrentUser.SaveGuestId(csr.customer.CustomerID.ToString());
                };
                TxtScanresult = FindViewById<TextView>(Resource.Id.txtScanresult);
                Txtem = FindViewById<TextView>(Resource.Id.textV);
                BtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
                txtmail = FindViewById<EditText>(Resource.Id.txtmail);
                BtnResend = FindViewById<Button>(Resource.Id.btnResend);
                update = FindViewById<Button>(Resource.Id.btnUpdateEmailclick);
                BtnContinue = FindViewById<Button>(Resource.Id.btnContinue);
                BtnUpdateEmail = FindViewById<Button>(Resource.Id.btnUpdateEmail);
                BtnResend.Visibility = ViewStates.Gone;
                update.Visibility = ViewStates.Gone;
                BtnLogin.Visibility = ViewStates.Gone;
                BtnContinue.Visibility = ViewStates.Gone;
                BtnUpdateEmail.Visibility = ViewStates.Gone;
                txtmail.Visibility = ViewStates.Gone;
                Txtem.Visibility = ViewStates.Gone;
                if (IsPlayServicesAvailable())
                {
                    var TaskB = new System.Threading.Tasks.Task(() =>
                    {
                        var intent = new Intent(this, typeof(RegistrationIntentService));
                        StartService(intent);
                    });
                    TaskB.Start();
                }
                var telephonyDeviceID = string.Empty;
                var telephonySIMSerialNumber = string.Empty;
                TelephonyManager telephonyManager = (TelephonyManager)this.ApplicationContext.GetSystemService(Context.TelephonyService);
                if (telephonyManager != null)
                {
                    if (!string.IsNullOrEmpty(telephonyManager.DeviceId))
                        telephonyDeviceID = telephonyManager.DeviceId;
                    if (!string.IsNullOrEmpty(telephonyManager.SimSerialNumber))
                        telephonySIMSerialNumber = telephonyManager.SimSerialNumber;
                }
                var androidID = Android.Provider.Settings.Secure.GetString(this.ApplicationContext.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
                var deviceUuid = new UUID(androidID.GetHashCode(), ((long)telephonyDeviceID.GetHashCode() << 32) | telephonySIMSerialNumber.GetHashCode());
                var DeviceID = deviceUuid.ToString();
                CurrentUser.SaveDeviceID(DeviceID);
                BtnScanner.Dispose();
            }
        }
        public void IntoApp()
        {
            int storename =  Convert.ToInt32(CurrentUser.GetPrefered());
            if (storename == 1)
            {
                Intent intent = new Intent(this, typeof(GridViewActivity));
                intent.PutExtra("MyData", "Wall Store");
                ProgressIndicator.Show(this);
                StartActivity(intent);
            }
            else if (storename == 2)
            {
                Intent intent = new Intent(this, typeof(GridViewActivity));
                intent.PutExtra("MyData", "Point Pleasant Store");
                ProgressIndicator.Show(this);
                StartActivity(intent);
            }
            else
            {
                Intent intent = new Intent(this, typeof(Login));
                ProgressIndicator.Show(this);
                StartActivity(intent);
            }
        }
        public async void Preinfo(string CardNumber)
        {
            AndHUD.Shared.Show(this, "Please Wait...", Convert.ToInt32(MaskType.Clear));
            try
            {
                BtnLogin.Visibility = ViewStates.Gone;
                BtnResend.Visibility = ViewStates.Gone;
                try
                {
                    //CurrentUser.
                    EmailVerification(false);
                }
                catch (Exception ex)
                {
                    LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);
                }
                AuthServ = await svc.AuthencateUser("test", CardNumber, CurrentUser.GetDeviceID());
                CurrentUser.SaveInternalCustometID(AuthServ.customer.CustomerID.ToString());
                LoggingClass.LogInfo("User Tried to login with " + CardNumber, screenid);
                if (CardNumber != null)
                {
                    CurrentUser.SaveCardNumber(CardNumber);
                }
                if (AuthServ != null)
                {
                    CurrentUser.SaveUserName(AuthServ.customer.FirstName + AuthServ.customer.LastName, AuthServ.customer.CustomerID.ToString());
                    CurrentUser.SavePrefered(AuthServ.customer.PreferredStore);
                    if (AuthServ.customer.Email != "" && AuthServ.customer.Email != null)
                    {
                        SendRegistrationToAppServer(CurrentUser.getDeviceToken());
                        if (AuthServ.ErrorDescription != null || AuthServ.ErrorDescription == "")
                        {
                            TxtScanresult.Text = AuthServ.ErrorDescription;
                        }
                        else
                        {
                            TxtScanresult.Text = " Hi " + AuthServ.customer.FirstName + AuthServ.customer.LastName + ",\nWe are sending a verification email to " + AuthServ.customer.Email + "..To proceed press continue. To change your emailAddress click on Update.";
                        }
                        TxtScanresult.SetTextColor(Android.Graphics.Color.Black);
                        BtnContinue.Visibility = ViewStates.Visible;
                        BtnUpdateEmail.Visibility = ViewStates.Invisible;
                        update.Visibility = ViewStates.Visible;
                        BtnContinue.Click += async delegate
                        {
                            {
                                AndHUD.Shared.Show(this, " Please Wait...", Convert.ToInt32(MaskType.Clear));
                                AuthServ = await svc.ContinueService(AuthServ);
                                ShowInfo(AuthServ);
                                AndHUD.Shared.Dismiss();
                            }
                        };  
                        update.Click += delegate
                        {
                            TxtScanresult.Text = "";
                            BtnContinue.Visibility = ViewStates.Gone;
                            update.Visibility = ViewStates.Gone;
                            txtmail.Visibility = ViewStates.Visible;
                            Txtem.Visibility = ViewStates.Visible;
                            BtnUpdateEmail.Visibility = ViewStates.Visible;
                        };
                        BtnUpdateEmail.Click += delegate
                        { 
                            {
                                BtnUpdateEmail_Click("please enter your new e-mail id.");  
                            }
                        };
                    }
                    else
                    {
                        if (AuthServ.ErrorDescription != null || AuthServ.ErrorDescription == "")
                        {
                            TxtScanresult.Text = AuthServ.ErrorDescription;
                        }
                        else
                        {
                            TxtScanresult.Text = "Hi " + AuthServ.customer.FirstName + AuthServ.customer.LastName + ", \nIt seems we do not have your email address! Please update it so we can send you a verification link that will activate your account.";

                        }
                        TxtScanresult.SetTextColor(Android.Graphics.Color.Red);
                        BtnContinue.Visibility = ViewStates.Gone;
                        BtnUpdateEmail.Visibility = ViewStates.Visible;
                        txtmail.Visibility = ViewStates.Visible;
                        Txtem.Visibility = ViewStates.Visible;
                        BtnUpdateEmail.Click += delegate
                        {
                            BtnUpdateEmail_Click("please enter your new e-mail id.");
                        };
                    }
                }
                else
                {

                    TxtScanresult.Text = "Sorry. Your Card number is not matching our records.\n Please re-scan Or Try app as Guest Log In.";
                    BtnResend.Visibility = ViewStates.Invisible;
                    BtnLogin.Visibility = ViewStates.Invisible;
                    TxtScanresult.SetTextColor(Android.Graphics.Color.Red);
                    AndHUD.Shared.Dismiss();
                }
                AndHUD.Shared.Dismiss();
            }
            catch (Exception exe)
            {

            }

        }
        private async void BtnUpdateEmail_Click(string Message)
        {
            try
            {

                txtmail.SetTextColor(Color.Black);

                if (txtmail.Text == null || txtmail.Text == "")
                {
                    TxtScanresult.SetTextColor(Android.Graphics.Color.Red);
                    TxtScanresult.Text = "Please enter your email.";
                }
                else if (txtmail.Text.Contains("@") != true && txtmail.Text.Contains(".") != true)
                {
                    TxtScanresult.SetTextColor(Android.Graphics.Color.Red);
                    TxtScanresult.Text = "Please enter valid email.";
                }
                else
                {
                    AndHUD.Shared.Show(this, "Updating...Please Wait...", Convert.ToInt32(MaskType.Clear));
                    AuthServ = await svc.UpdateMail(txtmail.Text, AuthServ.customer.CustomerID.ToString());
                    ShowInfo(AuthServ);
                    AndHUD.Shared.Dismiss();  
                } 
            }
            catch (Exception ex)
            {

            }

        }
        public async void ShowInfo(CustomerResponse AuthServ)
        {
            AndHUD.Shared.Show(this, "Please Wait...", Convert.ToInt32(MaskType.Clear));
            try
            {
                if (AuthServ.customer.Email != "" && AuthServ.customer.Email != null)
                {
                    TxtScanresult.Text = AuthServ.ErrorDescription;// " Hi " + AuthServ.customer.FirstName + authen.customer.LastName + ",\n We have sent an email at  " + authen.customer.Email + ".\n Please verify email to continue login. \n If you have not received email Click Resend Email.\n To get Email Id changed, contact store.";

                    TxtScanresult.SetTextColor(Android.Graphics.Color.Black);
                    BtnResend.Visibility = ViewStates.Visible;
                    txtmail.Visibility = ViewStates.Gone;
                    Txtem.Visibility = ViewStates.Gone;
                    BtnLogin.Visibility = ViewStates.Visible;
                    BtnContinue.Visibility = ViewStates.Gone;
                    BtnUpdateEmail.Visibility = ViewStates.Gone;
                    update.Visibility = ViewStates.Gone;
                    BtnResend.Click += async delegate
                        {
                            try
                            {       AndHUD.Shared.Show(this, "Sending verification email to " + AuthServ.customer.Email, Convert.ToInt32(MaskType.Clear));
                                    LoggingClass.LogInfo("Resend email " + AuthServ.customer.Email, screenid);
                                    await svc.ResendEMail(CurrentUser.GetCardNumber());
                                    AndHUD.Shared.ShowSuccess(this, "Sent", MaskType.Clear, TimeSpan.FromSeconds(2));
                                    AndHUD.Shared.Dismiss();
                            }
                            catch (Exception ex)
                            {
                            }
                        };
                    BtnLogin.Click += delegate
                    {
                        LoggingClass.LogInfo("Clicked on Login " + AuthServ.customer.CardNumber, screenid);
                        AndHUD.Shared.Show(this, "Checking Email Verification", Convert.ToInt32(MaskType.Clear));
                        EmailVerification(true);
                    };
                }
                else
                {
                    TxtScanresult.Text = AuthServ.ErrorDescription;
                    TxtScanresult.SetTextColor(Android.Graphics.Color.Red);
                }
                AndHUD.Shared.Dismiss();
            }

            catch (Exception ex)
            {
                LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);
            }

            AndHUD.Shared.Dismiss();
        }
        Boolean isValidEmail(String email)
        {
            return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
        }
        public async void SendRegistrationToAppServer(string token)
        {
            TokenModel _token = new TokenModel()
            {
                User_id = Convert.ToInt32(CurrentUser.getUserId()),
                DeviceToken = token,
                DeviceType = 1
            };
            LoggingClass.LogInfoEx("Token sent to db", screenid);
            int x = await svc.InsertUpdateToken1(_token);
        }
        public async void EmailVerification(Boolean Load)
        {
            if (Load == true)
            {
                AndHUD.Shared.Show(this, "Checking Email Verification", Convert.ToInt32(MaskType.Clear));
            }
                DeviceToken DO = new DeviceToken();
                try
                {
                //  DO = await svc.CheckMail(AuthServ.customer.CustomerID.ToString());
                DO = await svc.CheckMail(CurrentUser.GetInternalCustometID());

                if (DO.VerificationStatus == 1)
                    {
                    //  if (AuthServ.customer != null && AuthServ.customer.CustomerID != 0)
                    if (CurrentUser.GetInternalCustometID() != null &&Convert.ToInt32( CurrentUser.GetInternalCustometID() )!=0)
                    {
                           // LoggingClass.LogInfo("The User logged in with user id: " + CurrentUser.getUserId(), screenid);
                          
                            SendRegistrationToAppServer(CurrentUser.getDeviceToken());
                        // CurrentUser.SavePrefered(AuthServ.customer.PreferredStore);
                        int storename =Convert.ToInt32( CurrentUser.GetPrefered());///AuthServ.customer.PreferredStore;
                        if (storename == 1)
                            {
                                Intent intent = new Intent(this, typeof(GridViewActivity));
                                intent.PutExtra("MyData", "Wall Store");
                                ProgressIndicator.Show(this);

                                StartActivity(intent);
                            }
                            else if (storename == 2)
                            {
                                Intent intent = new Intent(this, typeof(GridViewActivity));
                                intent.PutExtra("MyData", "Point Pleasant Store");

                                ProgressIndicator.Show(this);
                                StartActivity(intent);
                            }
                            else
                            {
                                Intent intent = new Intent(this, typeof(Login));
                                ProgressIndicator.Show(this);
                                StartActivity(intent);
                            }
                            LoggingClass.LogInfoEx("User verified and Logging" + "---->" + CurrentUser.GetCardNumber(), screenid);
                            AndHUD.Shared.Dismiss();
                            AndHUD.Shared.ShowSuccess(Parent, "Success!", MaskType.Clear, TimeSpan.FromSeconds(2));
                        }
                        else
                        {
                            AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                                aler.SetTitle("Sorry");
                                aler.SetMessage("You entered wrong details or authentication failed");
                                aler.SetNegativeButton("Ok", delegate { });
                                Dialog dialog1 = aler.Create();
                                dialog1.Show();  
                        };
                    }
                    else
                    {
                        AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                        aler.SetMessage("Your email is not verified. please check email and verify.");
                        aler.SetNegativeButton("Ok", delegate { });
                        Dialog dialog = aler.Create();
                        dialog.Show();  
                    } 
                    AndHUD.Shared.Dismiss();
                }

                catch (Exception exe)
                {
                AndHUD.Shared.Dismiss();
                    LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                }
            AndHUD.Shared.Dismiss();  
            //BtnLogin.Dispose();
            //BtnResend.Dispose();
            //BtnContinue.Dispose();
            //BtnUpdateEmail.Dispose();
            //update.Dispose();

        }
        private bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                // Google Play Service check failed - display the error to the user:
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    // Give the user a chance to download the APK:

                    gplaystatus = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    gplaystatus = "Sorry, this device is not supported";
                    AlertDialog.Builder aler = new AlertDialog.Builder(this);
                    aler.SetTitle("Sorry");
                    aler.SetMessage(gplaystatus);
                    aler.SetNegativeButton("Ok", delegate { });
                    Dialog dialog3 = aler.Create();
                    dialog3.Show();
                    Finish();
                }
                return false;
            }
            else
            {
                gplaystatus = "Google Play Services is available.";
                return true;
            }
        } 
        public bool CheckInternetConnection()
        {
            string CheckUrl = "http://google.com";
            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);
                iNetRequest.Timeout = 5000;
                WebResponse iNetResponse = iNetRequest.GetResponse();
                iNetResponse.Close();
                return true;
            }
            catch (WebException ex)
            {
                AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                aler.SetTitle("Sorry");
                LoggingClass.LogInfo("Please check your internet connection", screenid);
                aler.SetMessage("Please check your internet connection");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();
                return false;
            }
        }
        protected override void OnPause()
        {
            base.OnPause();
          
        }
        protected override void OnResume()
        {
            EmailVerification(false);
            base.OnResume();


        }
        public  override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }
    }
}