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
    public class MyHangouts : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.Hangouts);
            Button Top = FindViewById<Button>(Resource.Id.btnTop);
            Button Middle = FindViewById<Button>(Resource.Id.btnMiddle);
            Button Bottom = FindViewById<Button>(Resource.Id.btnBottom);
            Button Bottom1 = FindViewById<Button>(Resource.Id.btnBottom1);
            var metrics = Resources.DisplayMetrics;
            int height = 0;// = metrics.HeightPixels;
            height = (metrics.HeightPixels) - (int)((360 * metrics.Density) / 3);
            height = height / 4;
            height = height - 10;
            Top.LayoutParameters.Height = height;
            Middle.LayoutParameters.Height = height;
            Bottom.LayoutParameters.Height = height;
            Bottom1.LayoutParameters.Height = height;
            Top.SetBackgroundResource(Resource.Drawable.mt);
            Middle.SetBackgroundResource(Resource.Drawable.mr);
            Bottom.SetBackgroundResource(Resource.Drawable.mf);
            Bottom1.SetBackgroundResource(Resource.Drawable.ms);
            if (CurrentUser.getUserId()==null||CurrentUser.getUserId()=="0")
            {

                AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                aler.SetTitle("Sorry");
                aler.SetMessage("This Feature is available only for VIP Users ");
                aler.SetNegativeButton("KnowMore", delegate
                {
                    var uri = Android.Net.Uri.Parse("https://hangoutz.azurewebsites.net/index.html");
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                });
                aler.SetNeutralButton("Cancel", delegate
                {

                });
                Dialog dialog1 = aler.Create();
                dialog1.Show();
                Top.Click += (sender, e) =>
                {

                    Dialog dialog11 = aler.Create();
                    dialog1.Show();
                };
                Middle.Click += (sender, e) =>
                {

                    Dialog dialog12 = aler.Create();
                    dialog1.Show();
                };
                Bottom.Click += (sender, e) =>
                {

                    Dialog dialog13 = aler.Create();
                    dialog1.Show();
                };
                Bottom1.Click += (sender, e) =>
                {

                    Dialog dialog13 = aler.Create();
                    dialog1.Show();
                };
            }
            else
            {
                Top.Click += (sender, e) =>
                {
                    ProgressIndicator.Show(this);
                    var intent = new Intent(this, typeof(MyTastingActivity));
                    intent.PutExtra(AppConstants.MyData, AppConstants.MyTastings);
                    StartActivity(intent);
                };
                Middle.Click += (sender, e) =>
                {
                    ProgressIndicator.Show(this);
                    var intent = new Intent(this, typeof(MyReviewActivity));
                    intent.PutExtra(AppConstants.MyData, AppConstants.MyReviews);
                    StartActivity(intent);
                };
                Bottom.Click += (sender, e) =>
                {
                    ProgressIndicator.Show(this);
                    var intent = new Intent(this, typeof(MyFavoriteAvtivity));
                    intent.PutExtra(AppConstants.MyData, AppConstants.MyFavorites);
                    StartActivity(intent);
                };
                Bottom1.Click += (sender, e) =>
                {
                    CustomerResponse AuthServ = new CustomerResponse();
                    int storename = Convert.ToInt32(CurrentUser.GetPrefered());
                    if (storename == 1)
                    {
                        Intent intent = new Intent(this, typeof(GridViewActivity));
                        intent.PutExtra(AppConstants.MyData, AppConstants.WallStore);
                        ProgressIndicator.Show(this);

                        StartActivity(intent);
                    }
                    else if (storename == 2)
                    {
                        Intent intent = new Intent(this, typeof(GridViewActivity));
                        intent.PutExtra(AppConstants.MyData, AppConstants.PointPleasantStore);
                        ProgressIndicator.Show(this);
                        StartActivity(intent);
                    }
                    else if (storename == 3)
                    {

                        Intent intent = new Intent(this, typeof(GridViewActivity));
                        intent.PutExtra(AppConstants.MyData, AppConstants.SecaucusStore);
                        ProgressIndicator.Show(this);
                        StartActivity(intent);
                    }
                    else
                    {   
                        AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                        aler.SetMessage("Please Select your preferred store!");
                        aler.SetNegativeButton("Ok", delegate { });
                        Dialog dialog1 = aler.Create();
                        dialog1.Show();
                    }
                };

            }
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
            //LoggingClass.LogInfo("OnResume state in Gridview activity" + StoreName, screenid);
        }
        public override void OnLowMemory()
        {
            GC.Collect();
        }
    }
}