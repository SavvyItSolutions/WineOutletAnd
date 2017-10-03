using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Hangout.Models;
using AndroidHUD;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;
using Android.Provider;
using Android.Content.PM;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Android.Support.V4.Graphics.Drawable;

namespace WineHangouts
{
    [Activity(Label = "MyProfile", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
  
    public class PotraitActivity : Activity
    {
        public int uid;
        public string StoreName = "";
        public readonly bool indirect;
        public WebClient WebClient;
        private int screenid;
        public ImageView propicimage;
        string path = null;
        public static readonly int PickImageId = 1000;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ProfileCell);
            int userId = Convert.ToInt32(CurrentUser.getUserId());
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ServiceWrapper sw = new ServiceWrapper();
            var output = sw.GetCustomerDetails(userId).Result;
            RefreshParent();
            if (CurrentUser.getUserId() == "0" || CurrentUser.getUserId() == null)
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
           
                try
            {
                propicimage = FindViewById<ImageView>(Resource.Id.user_profile_photo);
                TextView Name = FindViewById<TextView>(Resource.Id.user_profile_name);
                TextView Email = FindViewById<TextView>(Resource.Id.user_profile_short_bio);
                TextView Mobile = FindViewById<TextView>(Resource.Id.user_mobile);
                TextView Address = FindViewById<TextView>(Resource.Id.user_Address);
                TextView PinCode = FindViewById<TextView>(Resource.Id.user_Zip);
                TextView Preferrd = FindViewById<TextView>(Resource.Id.user_Preferred);
                TextView State = FindViewById<TextView>(Resource.Id.user_State);
                TextView Card = FindViewById<TextView>(Resource.Id.user_Card);
                TextView Expiry = FindViewById<TextView>(Resource.Id.User_expiry);
                Button Drop = FindViewById<Button>(Resource.Id.drop_down_option_menu);
                ImageView ChangePRo = FindViewById<ImageView>(Resource.Id.add_friend);
               
                ChangePRo.Click += delegate
                {

                    AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                    aler.SetTitle("Please choose an option to upload profile picture");
                    aler.SetPositiveButton("Gallery", delegate
                    {
                        Intent = new Intent();
                        Intent.SetType("image/*");
                        Intent.SetAction(Intent.ActionGetContent);
                        StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
                    });

                    aler.SetNegativeButton("Camera", delegate
                    {
                        if (IsThereAnAppToTakePictures())
                        {
                            CreateDirectoryForPictures();
                            TakeAPicture();
                        }
                    });
                    aler.SetNeutralButton("Cancel", delegate
                    {

                    });
                    Dialog dialog1 = aler.Create();
                    dialog1.Show();
                };
              // Boolean indirect = true;
                if (indirect == true)
                {
                    System.Threading.Timer timer = null;
                    timer = new System.Threading.Timer((obj) =>
                    {



                        DownloadAsync(this, System.EventArgs.Empty);

                        timer.Dispose();
                    },
                                null, 2000, System.Threading.Timeout.Infinite);
                }
                else
                {
                    DownloadAsync(this, System.EventArgs.Empty);
                }
                Drop.Click += (s, arg) =>
                {
                    Intent intent = new Intent(this, typeof(ProfileActivity));
                    StartActivity(intent);
                    //PopupMenu menu = new PopupMenu(this, Drop);
                    //menu.Inflate(Resource.Drawable.options_menu_1);
                    //menu.Show();
                };
                string First = output.customer.FirstName;
                string Last = output.customer.LastName;
                Name.Text = string.Concat(First, Last);
                Card.Text = output.customer.CardNumber.ToString();

                Email.Text = output.customer.Email;

                PinCode.Text = output.customer.Zip.ToString();
                Preferrd.Text = output.customer.PreferredStore.ToString();
                if(Preferrd.Text=="0")
                {
                    Preferrd.Text = "-Select your preferred store-";
                    
                }
                else if(Preferrd.Text=="1")

                {
                    Preferrd.Text = AppConstants.WallStore;
                }
                else if (Preferrd.Text == "2")
                {
                    Preferrd.Text = AppConstants.PointPleasantStore;
                }
                    else
                {
                    Preferrd.Text = AppConstants.SecaucusStore;
                }

                State.Text = output.customer.State;
                Expiry.Text = output.customer.ExpireDate.ToString();
                string Addres2 = output.customer.Address2;
                string Addres1 = output.customer.Address1;
                Address.Text = string.Concat(Addres1, Addres2);
                string phno1 = output.customer.PhoneNumber;
                string phno2 = output.customer.Phone2;
                if (phno1 != null)
                {
                    Mobile.Text = phno1;
                }
                else
                {
                    Mobile.Text = phno2;
                }
              
            }
            catch(Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            ProgressIndicator.Hide();
        }
        public void RefreshParent()
        {
            ServiceWrapper svc = new ServiceWrapper();
            int userId = Convert.ToInt32(CurrentUser.getUserId());
            var output = svc.GetCustomerDetails(userId).Result;
          
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                var intent = new Intent(this, typeof(Login));
                StartActivity(intent);

            }
            return base.OnOptionsItemSelected(item);
        }
        public async void DownloadAsync(object sender, System.EventArgs ea)
        {
            try
            {

                WebClient = new WebClient();
                var url = new Uri("https://icsintegration.blob.core.windows.net/profileimages/" + Convert.ToInt32(CurrentUser.getUserId()) + ".jpg");
                byte[] imageBytes = null;
                try
                {
                    imageBytes = await WebClient.DownloadDataTaskAsync(url);


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
                           //Bitmap pro = addWhiteBorder(bitmap, 10);
                            propicimage.SetImageBitmap(bitmap);

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
            //matrix.PostRotate(90);
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
            //using (var fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
            //{
            //    await blob.UploadFromStreamAsync(fs);
            //    LoggingClass.LogInfo("Profile picture uploaded into blob", screenid);
            //}

            await blob.UploadFromByteArrayAsync(bitmapData, 0, bitmapData.Length);
            GC.Collect();

            //st.Stop();
            //LoggingClass.LogTime("Upload profile pic ", st.Elapsed.TotalSeconds.ToString());
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
            byte[] bitmapData;
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 60, stream);
                bitmapData = stream.ToArray();
            }
            return bitmapData;
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
                        catch (Exception exe)
                        {
                           // LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
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
                                propicimage.  SetImageBitmap(pro);
                                App.bitmap = null;
                            }

                            GC.Collect();
                        }
                        catch (Exception exe)
                        {
                            LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                        }
                    }
                }
            }
            catch (Exception exe) { LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString()); }
        }
        public async override void OnBackPressed()
        {
            //var intent = new Intent(this, typeof(Login));

            //StartActivity(intent);
             MoveTaskToBack(true);
            ProgressIndicator.Hide();
        }

    }
}