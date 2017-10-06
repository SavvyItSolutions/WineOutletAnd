using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Views;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;
using Android.Util;
using System.Diagnostics;

namespace WineHangouts
{

    public static class App
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Bitmap bitmap;

    }

    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Theme = "@android:style/Theme.Dialog", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ProfilePicturePickDialog : Activity
    {

        //private ImageView _imageView;
        public string path;
        private int screenid = 14;
        protected override void OnCreate(Bundle bundle)
        {
            //base.OnCreate(bundle);
            //Window.RequestFeature(WindowFeatures.NoTitle);
            //SetContentView(Resource.Layout.ProfilePickLayout);
            //LoggingClass.LogInfo("Entered into ProfilePicturePickDialog", screenid);
            //if (IsThereAnAppToTakePictures())
            //{
            //    CreateDirectoryForPictures();
            //    ImageButton BtnCamera = FindViewById<ImageButton>(Resource.Id.btnCamera);
            //    LoggingClass.LogInfo("clicked on camera", screenid);
            //    // _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            //    BtnCamera.Click += TakeAPicture;
            //}
            //ImageButton btnGallery = FindViewById<ImageButton>(Resource.Id.imgbtnGallery);
            //btnGallery.Click += delegate
            //{
            //    LoggingClass.LogInfo("Clicked on gallery picking ", screenid);
            //    Intent intent = new Intent(this, typeof(ProfilePictureGallery));
            //    StartActivity(intent);
            //};
        }
        public string CreateDirectoryForPictures()
        {
            App._dir = new Java.IO.File(Environment.GetExternalStoragePublicDirectory("WineHangouts"), "winehangouts/wineimages");

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
    }
}
