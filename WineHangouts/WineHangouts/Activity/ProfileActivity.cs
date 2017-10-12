using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using System.Net;
using Hangout.Models;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using AndroidHUD;
using Android.Views.InputMethods;
using Android.Provider;
using Android.Content.PM;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Java.IO;
using static Android.Graphics.Bitmap;

//using System.Drawing.Drawing2D;

namespace WineHangouts
{
	[Activity(Label = "Edit Profile", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class ProfileActivity : Activity, IPopupParent
    {
        string path = null;
		WebClient webClient;
		private int screenid = 8;
        public static readonly int PickImageId = 1000;
        protected override void OnCreate(Bundle bundle)
		{
			CheckInternetConnection();
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Profile);
			try
			{
                GC.Collect();
				LoggingClass.LogInfo("Entered into Profile Activity", screenid);
				ActionBar.SetHomeButtonEnabled(true);
				ActionBar.SetDisplayHomeAsUpEnabled(true);
				int userId = Convert.ToInt32(CurrentUser.getUserId());
				ServiceWrapper sw = new ServiceWrapper();
				var output = sw.GetCustomerDetails(userId).Result;
                EditText Firstname = FindViewById<EditText>(Resource.Id.txtFirstName);
                Button updatebtn = FindViewById<Button>(Resource.Id.UpdateButton);
                Spinner spn = FindViewById<Spinner>(Resource.Id.spinner);
                Spinner Prefered = FindViewById<Spinner>(Resource.Id.spinner1);
                EditText Mobilenumber = FindViewById<EditText>(Resource.Id.txtMobileNumber);
                EditText Lastname = FindViewById<EditText>(Resource.Id.txtLastName);
                EditText Email = FindViewById<EditText>(Resource.Id.txtEmail);
                EditText Address = FindViewById<EditText>(Resource.Id.txtAddress);
                EditText PinCode = FindViewById<EditText>(Resource.Id.txtZip);
                if ( CurrentUser.getUserId() == "0"|| CurrentUser.getUserId() == null)
                {
                    AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                    aler.SetTitle("Sorry");
                    aler.SetMessage("This feature is available only  for VIP Users");
                    aler.SetPositiveButton("Log in", delegate
                    {
                        string str = null;
                        CurrentUser.SaveGuestId(str);
                        var intent = new Intent(this, typeof(LoginActivity));
                        StartActivity(intent);
                    });
                    aler.SetNegativeButton("KnowMore", delegate
                    {
                        var uri = Android.Net.Uri.Parse("https://hangoutz.azurewebsites.net/index.html");
                        var intent = new Intent(Intent.ActionView, uri);
                        StartActivity(intent);       
                    });
                    aler.SetNeutralButton("Cancel", delegate
                    {
                        var intent = new Intent(this, typeof(Login));
                        StartActivity(intent);
                    });
                    Dialog dialog1 = aler.Create();
                    dialog1.Show();
                }
                else
                {
                Firstname.Text = output.customer.FirstName;
                Lastname.Text = output.customer.LastName;  
                string phno1 = output.customer.PhoneNumber;
				string phno2 = output.customer.Phone2;
                if (phno1 != null)
                {
                    Mobilenumber.Text = phno1;
                }
                else
                {
                    Mobilenumber.Text = phno2;
                }   
                Email.Text = output.customer.Email;    
                string Addres2 = output.customer.Address2;
				string Addres1 = output.customer.Address1;
				Address.Text = string.Concat(Addres1, Addres2);
                PinCode.Text = output.customer.Zip;  
                string state = output.customer.State;
				int Preferedstore = output.customer.PreferredStore;
				List<string> storelist = new List<string>();
				storelist.Add("--Select your preferred store--");
				storelist.Add("Wall");
				storelist.Add("Point Pleasant");
                    storelist.Add("Secaucus");
                    storelist.Add("All");
                List<string> StateList = new List<string>();
				StateList.Add("AL");
				StateList.Add("AK");
				StateList.Add("AZ");
				StateList.Add("AR");
				StateList.Add("CA");
				StateList.Add("CO");
				StateList.Add("CT");
				StateList.Add("DE");
				StateList.Add("FL");
				StateList.Add("GA");
				StateList.Add("HI");
				StateList.Add("ID");
				StateList.Add("IL");
				StateList.Add("IN");
				StateList.Add("IA");
				StateList.Add("KS");
				StateList.Add("KY");
				StateList.Add("LA");
				StateList.Add("ME");
				StateList.Add("MD");
				StateList.Add("MA");
				StateList.Add("MI");
				StateList.Add("MN");
				StateList.Add("MS");
				StateList.Add("MO");
				StateList.Add("MT");
				StateList.Add("NE");
				StateList.Add("NV");
				StateList.Add("NH");
				StateList.Add("NJ");
				StateList.Add("NM");
				StateList.Add("NY");
				StateList.Add("NC");
				StateList.Add("ND");
				StateList.Add("OH");
				StateList.Add("OK");
				StateList.Add("OR");
				StateList.Add("PA");
				StateList.Add("RI");
				StateList.Add("SC");
				StateList.Add("SD");
				StateList.Add("TN");
				StateList.Add("TX");
				StateList.Add("UT");
				StateList.Add("VT");
				StateList.Add("VA");
				StateList.Add("WA");
				StateList.Add("WV");
				StateList.Add("WI");
				StateList.Add("WY");
                    try    
                    {
                        int i = StateList.IndexOf(state.ToString());
                        spn.SetSelection(i);
                    }
                    catch { }
				int p = storelist.IndexOf(Prefered.SelectedItem.ToString());
				Prefered.SetSelection(Preferedstore);
               
               
					updatebtn.Click += async delegate
					{
                        if ((Email.Text.Contains("@")) == false || (Email.Text.Contains(".")) == false)
                        {
                            AndHUD.Shared.ShowErrorWithStatus(this, "Email is invalid", MaskType.Clear, TimeSpan.FromSeconds(2));
                        }
                        else if ((PinCode.Text.Length != 5))
                        {
                            AndHUD.Shared.ShowErrorWithStatus(this, "Zipcode is invalid", MaskType.Clear, TimeSpan.FromSeconds(2));
                        }
                        else if((Mobilenumber.Text=="")||(Mobilenumber.Text.Length!=10))
                        { AndHUD.Shared.ShowErrorWithStatus(this, "Enter valid mobile number", MaskType.Clear, TimeSpan.FromSeconds(2)); }
                        else
                        {
                            AndHUD.Shared.Show(this, "Please Wait...", Convert.ToInt32(MaskType.Clear));
                         
                            Customer customer = new Customer()
                            {
                                FirstName = Firstname.Text,
                                LastName = Lastname.Text,
                                PhoneNumber = Mobilenumber.Text,
                                Address1 = Address.Text,
                                Email = Email.Text,
                                CustomerID = userId,
                                State = spn.SelectedItem.ToString(),
                                Zip = PinCode.Text,
                                PreferredStore = Convert.ToInt32(Prefered.SelectedItemId)
                            };
                            CurrentUser.SavePrefered(Convert.ToInt32(Prefered.SelectedItemId));
                            LoggingClass.LogInfo("Clicked on update info", screenid);
                            var x = await sw.UpdateCustomer(customer);
                            if (x == 1)
                            {
                                var intent = new Intent(this, typeof(PotraitActivity));
                                StartActivity(intent);
                            }
                            AndHUD.Shared.Dismiss();
                            AndHUD.Shared.ShowSuccess(this, "Profile Updated", MaskType.Clear, TimeSpan.FromSeconds(2)); 
                        }
                    };
                    ProgressIndicator.Hide();
                    updatebtn.Dispose();    
				}
			}
			catch (Exception exe)
			{
				LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
				AlertDialog.Builder aler = new AlertDialog.Builder(this);
				aler.SetTitle("Sorry");
				aler.SetMessage("We're under maintainence");
				aler.SetNegativeButton("Ok", delegate { });
				Dialog dialog = aler.Create();
				dialog.Show();
			}
		
			ProgressIndicator.Hide();
		}
        public override void OnLowMemory()
        {
            GC.Collect();
        }
        public bool CheckInternetConnection()
        {

            string CheckUrl = "https://www.apple.com";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);
                iNetRequest.Timeout = 5000;
                WebResponse iNetResponse = iNetRequest.GetResponse();
                iNetResponse.Close();
                return true;

            }
            catch
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
        Boolean isValidEmail(String email)
        {
            return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                GC.Collect();
                MoveTaskToBack(true);
                Finish();
                LoggingClass.LogInfo("Exited from profile ", screenid);
              
                var intent = new Intent(this, typeof(PotraitActivity));
                LoggingClass.LogInfo("Clicked on options menu About", screenid);
                StartActivity(intent);
                return false;

            }
            return base.OnOptionsItemSelected(item);
        }
        public async override void OnBackPressed()
        {
            //var intent = new Intent(this, typeof(PotraitActivity));
            Finish();
            //StartActivity(intent);
            //MoveTaskToBack(true);
            ProgressIndicator.Hide();
        }

        public void RefreshParent()
        {
            ServiceWrapper svc = new ServiceWrapper();
            int userId = Convert.ToInt32(CurrentUser.getUserId());
            var output = svc.GetCustomerDetails(userId).Result;
            Bitmap imageBitmap = BlobWrapper.ProfileImages(userId);
        }
        public string CreateDirectoryForPictures()
        {
            App._dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory("WineHangouts"), "winehangouts/wineimages");

            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
            path = App._dir.ToString();

            String NOMEDIA = ".nomedia";
            App._file = new Java.IO.File(path, NOMEDIA);
            if (!App._file.Exists())
            {
                App._file.CreateNewFile();
            }

            return path;
        }
        protected override void OnPause()
        {
            base.OnPause();
            LoggingClass.LogInfo("OnPause state in Profile activity", screenid);

        }

        protected override void OnResume()
        {
            base.OnResume();
            LoggingClass.LogInfo("OnResume state in Profile activity", screenid);
        }
        public async void UploadProfilePic(string path)
        {

            Microsoft.WindowsAzure.Storage.Auth.StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
            CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
            CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("profileimages");
            await container.CreateIfNotExistsAsync();
            CloudBlockBlob blob = container.GetBlockBlobReference(CurrentUser.getUserId() + ".jpg");
            LoggingClass.LogInfo("Updated profile picture", screenid);
            using (var fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
            {
                await blob.UploadFromStreamAsync(fs);
                LoggingClass.LogInfo("Profile picture uploaded into blob", screenid);
            }
        }

    }

}