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
using Hangout.Models;

namespace WineHangouts
{
    [Activity(Label = "@string/ApplicationName", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MyLocations : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Location);
            Button Top = FindViewById<Button>(Resource.Id.btnTop);
            Button Middle = FindViewById<Button>(Resource.Id.btnMiddle);
            Button Bottom =FindViewById<Button>(Resource.Id.btnBottom);
            var metrics = Resources.DisplayMetrics;
            int height= 0;// = metrics.HeightPixels;
            height = (metrics.HeightPixels) - (int)((360 * metrics.Density) / 3);
            height = height / 3;
            height = height - 10;
            Top.LayoutParameters.Height = height;
            Middle.LayoutParameters.Height = height;
            Bottom.LayoutParameters.Height = height;
            Top.SetBackgroundResource(Resource.Drawable.wall1);        
            Middle.SetBackgroundResource(Resource.Drawable.pp1);       
            Bottom.SetBackgroundResource(Resource.Drawable.scacus1);
            Top.Click += (sender, e) =>
            {
                ProgressIndicator.Show(this);
                var intent = new Intent(this, typeof(GridViewActivity));
                intent.PutExtra(AppConstants.MyData,AppConstants.WallStore);
                StartActivity(intent);
            };
            Middle.Click += (sender, e) =>
            {
                ProgressIndicator.Show(this);
                var intent = new Intent(this, typeof(GridViewActivity));
                intent.PutExtra(AppConstants.MyData, AppConstants.PointPleasantStore);
                StartActivity(intent);
            };
            Bottom.Click += (sender, e) =>
            {
                ProgressIndicator.Show(this);
                var intent = new Intent(this, typeof(GridViewActivity));
                intent.PutExtra(AppConstants.MyData,AppConstants.SecaucusStore);
                StartActivity(intent);
            };
        }
        public async override void OnBackPressed()
        {
            MoveTaskToBack(true);
            ProgressIndicator.Hide();
        }
        protected override void OnPause()
        {
            base.OnPause();
            //LoggingClass.LogInfo("OnPause state in Gridview activity"+StoreName, screenid);
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}