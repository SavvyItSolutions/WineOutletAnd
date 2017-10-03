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
		Stopwatch st;
        string path = null;
		ImageView propicimage;
		WebClient webClient;
		private int screenid = 8;
        ImageView gifImageView;
        Boolean indirect = true;
        public static readonly int PickImageId = 1000;
        protected override void OnCreate(Bundle bundle)
		{
			CheckInternetConnection();
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Profile);
			//st.Start();
			try
			{
                GC.Collect();
				///LoggingClass.UploadErrorLogs(LoggingClass.CreateDirectoryForLogs());
				LoggingClass.LogInfo("Entered into Profile Activity", screenid);
				ActionBar.SetHomeButtonEnabled(true);
				ActionBar.SetDisplayHomeAsUpEnabled(true);
				int userId = Convert.ToInt32(CurrentUser.getUserId());
				ServiceWrapper sw = new ServiceWrapper();
				var output = sw.GetCustomerDetails(userId).Result;
				//propicimage = FindViewById<ImageView>(Resource.Id.propic);
    //            RefreshParent();  
    //                propicimage.SetImageResource(Resource.Drawable.Loading);
    //                if (indirect == true)
    //                {
    //                    System.Threading.Timer timer = null;
    //                    timer = new System.Threading.Timer((obj) =>
    //                    {
                          


    //                        DownloadAsync(this, System.EventArgs.Empty);

    //                        timer.Dispose();
    //                    },
    //                                null, 2000, System.Threading.Timeout.Infinite);
    //                }
    //                else
    //                {
    //                    DownloadAsync(this, System.EventArgs.Empty);
    //                }   
    //            ImageButton changepropic = FindViewById<ImageButton>(Resource.Id.btnChangePropic);
				//changepropic.Click += delegate
				//{

    //                AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
    //                aler.SetTitle("Please choose an option to upload profile picture");
    //                aler.SetPositiveButton("Gallery", delegate
    //                {
    //                    Intent = new Intent();
    //                    Intent.SetType("image/*");
    //                    Intent.SetAction(Intent.ActionGetContent);
    //                    StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
    //                });

    //                aler.SetNegativeButton("Camera", delegate
    //                {
    //                    if (IsThereAnAppToTakePictures())
    //                    {
    //                        CreateDirectoryForPictures();
    //                        TakeAPicture();
    //                    }
    //                });
    //                aler.SetNeutralButton("Cancel", delegate
    //                {
                        
    //                });
    //                Dialog dialog1 = aler.Create();
    //                dialog1.Show();
    //            };
				//changepropic.Dispose();
                EditText Firstname = FindViewById<EditText>(Resource.Id.txtFirstName);
                Button updatebtn = FindViewById<Button>(Resource.Id.UpdateButton);
                Spinner spn = FindViewById<Spinner>(Resource.Id.spinner);
                Spinner Prefered = FindViewById<Spinner>(Resource.Id.spinner1);
                //TextView card = FindViewById<TextView>(Resource.Id.txtcard1);
               // TextView exp = FindViewById<TextView>(Resource.Id.txtexp);
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
                    //Firstname.FocusableInTouchMode = false;
                    //Firstname.Click += delegate
                    //{
                    //    Firstname.FocusableInTouchMode = true;
                    //};
                    //card.Text = output.customer.CardNumber;
                
                //exp.Text = output.customer.ExpireDate.ToString();
                //if (exp.Text == null || exp.Text == "")
                //{ exp.Text = "--"; }
                //else { exp.Text = output.customer.ExpireDate.ToString("yyyy/MM/dd"); }

               
				Lastname.Text = output.customer.LastName;
                    //Lastname.FocusableInTouchMode = false;
                    //Lastname.Click += delegate
                    //{
                    //    Lastname.FocusableInTouchMode = true;
                    //};

                    Mobilenumber.FocusableInTouchMode = false;
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
                    //Mobilenumber.Click += delegate
                    //{
                    //    Mobilenumber.FocusableInTouchMode = true;
                    //};


                    Email.Text = output.customer.Email;
                    //Email.FocusableInTouchMode = false;
                    //Email.Click += delegate
                    //{
                    //    Email.FocusableInTouchMode = true;
                    //};


                    string Addres2 = output.customer.Address2;
				string Addres1 = output.customer.Address1;
				Address.Text = string.Concat(Addres1, Addres2);
                    //Address.FocusableInTouchMode = false;
                    //Address.Click += delegate
                    //{
                    //    Address.FocusableInTouchMode = true;
                    //};
                    //EditText City = FindViewById<EditText>(Resource.Id.txtCity);
                    //City.Text = output.customer.CardNumber;
                    //if (CurrentUser.getUserId() != null)
                    //{
                    //	City.Enabled = false;
                    //}
                    //else { City.Enabled = true; }

                    PinCode.Text = output.customer.Zip;
                    //PinCode.FocusableInTouchMode = false;
                    //PinCode.Click += delegate
                    //{
                    //    PinCode.FocusableInTouchMode = true;
                    //};

                    //spn.SetSelection(4);

                    string state = output.customer.State;
				int Preferedstore = output.customer.PreferredStore;
				List<string> storelist = new List<string>();
				storelist.Add("--Select your preferred store--");
				storelist.Add("Wall");
				storelist.Add("Point Pleasant");
				storelist.Add("All");
              //   gifImageView = FindViewById<ImageView>(Resource.Id.gifImageView1);
                //gifImageView.StartAnimation();
            
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
                    catch (Exception ex) { }
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
                                //State = State.Text,
                                State = spn.SelectedItem.ToString(),

                                //City = City.Text
                                //CardNumber = City.Text,
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
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                if (resultCode.ToString() == "Canceled")
                {
                    LoggingClass.LogInfo("Cancelled from camera", screenid);
                    Intent intent = new Intent(this, typeof(ProfileActivity));
                    StartActivity(intent);
                }
                else
                {
                    if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
                    {
                        Android.Net.Uri uri = data.Data;
                        try
                        {


                            Resize(uri);

                            propicimage.SetImageURI(uri);

                        }
                        catch (Exception ex)
                        {
                            propicimage.SetImageURI(uri);
                        }

                    }
                    else
                    {
                        try
                        {
                            GC.Collect();
                            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
                            mediaScanIntent.SetData(contentUri);
                            SendBroadcast(mediaScanIntent);

                            int height = Resources.DisplayMetrics.HeightPixels;
                            int width = propicimage.Height;
                            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
                            if (App.bitmap != null)
                            {
                                Bitmap pro = addWhiteBorder(App.bitmap, 14);
                                Bitmap selectedimage = Resize(pro, 400, 400);
                                byte[] bitmapData = getBytesFromBitmap(selectedimage);
                                var TaskA = new System.Threading.Tasks.Task(() =>
                                {
                                    UploadProfilePicimagebutes(bitmapData);
                                });
                                TaskA.Start();
                                propicimage.SetImageBitmap(pro);
                                App.bitmap = null;
                            }

                            GC.Collect();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        private Android.Graphics.Bitmap NGetBitmap(Android.Net.Uri uriImage)
        {
            Android.Graphics.Bitmap mBitmap = null;
            try
            {


                mBitmap = Android.Provider.MediaStore.Images.Media.GetBitmap(this.ContentResolver, uriImage);

            }
            catch { }
            return mBitmap;

        }
        public byte[] getBytesFromBitmap(Bitmap bitmap)
        {
            //ByteArrayOutputStream stream = new ByteArrayOutputStream();
            //bitmap.Compress(CompressFormat.Jpeg, 70, stream);
            //return stream.ToByteArray();

            byte[] bitmapData;
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 60, stream);
                bitmapData = stream.ToArray();
            }
            return bitmapData;
        }
        public string GetRealPathFromURI(Android.Net.Uri contentUri)
        {
            var mediaStoreImagesMediaData = "_data";
            string[] projection = { mediaStoreImagesMediaData };
            Android.Database.ICursor cursor = this.ManagedQuery(contentUri, projection,
                                                                null, null, null);
            int columnIndex = cursor.GetColumnIndexOrThrow(mediaStoreImagesMediaData);
            cursor.MoveToFirst();
            return cursor.GetString(columnIndex);
        }
        public bool CheckInternetConnection()
        {

            string CheckUrl = "http://google.com";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout = 5000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                //Console.WriteLine("...connection established..." + iNetRequest.ToString());
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

        public async void DownloadAsync(object sender, System.EventArgs ea)
        {
            try
            {

                webClient = new WebClient();
                var url = new Uri("https://icsintegration.blob.core.windows.net/profileimages/" + Convert.ToInt32(CurrentUser.getUserId()) + ".jpg");
                byte[] imageBytes = null;
                try
                {
                    imageBytes = await webClient.DownloadDataTaskAsync(url);


                }
                catch (TaskCanceledException)
                {
                    //this.progressLayout.Visibility = ViewStates.Gone;
                    return;
                }
                catch (Exception exe)
                {
                    LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                }
                if (imageBytes != null)
                {

                    try
                    {
                        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                        string localFilename = CurrentUser.getUserId() + ".jpg";
                        string localPath = System.IO.Path.Combine(documentsPath, localFilename);

                        FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
                        await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
                        //Console.WriteLine("Saving image in local path: " + localPath);
                        fs.Close();
                        BitmapFactory.Options options = new BitmapFactory.Options()
                        {
                            InJustDecodeBounds = true
                        };
                        Bitmap bit = await BitmapFactory.DecodeFileAsync(localPath, options);

                        Bitmap bitmap = await BitmapFactory.DecodeFileAsync(localPath);
                        if (bitmap != null)
                        {
                            Bitmap pro = addWhiteBorder(bitmap, 10);
                            propicimage.SetImageBitmap(pro);

                        }
                        else
                        {
                            propicimage.SetImageResource(Resource.Drawable.ProfileEmpty);
                        }
                    }
                    catch (Exception exe)
                    {
                        LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                    }
                    st.Stop();
                    LoggingClass.LogTime("Download aSync image profile", st.Elapsed.TotalSeconds.ToString());

                }
                else
                {
                    propicimage.SetImageResource(Resource.Drawable.ProfileEmpty);
                }
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
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
              
                var intent = new Intent(this, typeof(PotraitActivity
));
                LoggingClass.LogInfo("Clicked on options menu About", screenid);
                StartActivity(intent);
                return false;

            }
            return base.OnOptionsItemSelected(item);
        }
        public async override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(PotraitActivity));

            StartActivity(intent);
            MoveTaskToBack(true);
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
        private void TakeAPicture()
        {

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new Java.IO.File(App._dir, String.Format(Convert.ToInt32(CurrentUser.getUserId()) + ".jpg", Guid.NewGuid()));
            path += "/" + CurrentUser.getUserId() + ".jpg";
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);


        }
        public Bitmap ResizeAndRotate(Bitmap image, int width, int height)
        {
            var matrix = new Matrix();
            var scaleWidth = ((float)width) / image.Width;
            var scaleHeight = ((float)height) / image.Height;
            matrix.PostRotate(90);
            matrix.PreScale(scaleWidth, scaleHeight);
            return Bitmap.CreateBitmap(image, 0, 0, image.Width, image.Height, matrix, true);
        }
        public Bitmap Resize(Bitmap image, int width, int height)
        {
            var matrix = new Matrix();
            var scaleWidth = ((float)width) / image.Width;
            var scaleHeight = ((float)height) / image.Height;
            matrix.PreScale(scaleWidth, scaleHeight);
            GC.Collect();
            return Bitmap.CreateBitmap(image, 0, 0, image.Width, image.Height, matrix, true);

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
        private Bitmap addWhiteBorder(Bitmap bmp, int borderSize)
        {
            Bitmap bmpWithBorder = Bitmap.CreateBitmap(bmp.Width + borderSize * 2, bmp.Height + borderSize * 2, bmp.GetConfig());
            Canvas canvas = new Canvas(bmpWithBorder);
            canvas.DrawColor(Color.Black);
            canvas.DrawBitmap(bmp, borderSize, borderSize, null);
            GC.Collect();
            return bmpWithBorder;

        }
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
        public async void UploadProfilePicimagebutes(byte[] bitmapData)
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

            await blob.UploadFromByteArrayAsync(bitmapData, 0, bitmapData.Length);
            GC.Collect();

            st.Stop();
            LoggingClass.LogTime("Upload profile pic ", st.Elapsed.TotalSeconds.ToString());
        }

        private void Resize(Android.Net.Uri uri)
        {
            Bitmap image = NGetBitmap(uri);
            Bitmap pro = addWhiteBorder(image, 14);
            Bitmap selectedimage = Resize(pro, 400, 400);
            byte[] bitmapData1 = getBytesFromBitmap(selectedimage);
            var TaskA = new System.Threading.Tasks.Task(() =>
            {
                UploadProfilePicimagebutes(bitmapData1);
            });
            TaskA.Start();


        }
        public void Resize1(string path)
        {
            Bitmap propic = BitmapFactory.DecodeFile(path);

            string dir_path = CreateDirectoryForPictures();
            dir_path = dir_path + "/" + Convert.ToInt32(CurrentUser.getUserId()) + ".jpg";

            Bitmap resized = Resize(propic, 350, 350);
            var filePath = System.IO.Path.Combine(dir_path);
            var stream = new FileStream(filePath, FileMode.Create);
            resized.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            stream.Close();
            UploadProfilePic(filePath);



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
            //st.Stop();
            //LoggingClass.LogTime("Upload profile pic ", st.Elapsed.TotalSeconds.ToString());
        }



    }

}