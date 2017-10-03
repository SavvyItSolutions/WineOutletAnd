using System;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;
using Hangout.Models;
using Android.Content;
using System.Collections.Generic;
using System.Diagnostics;
using Android.Support.V4.Widget;
using System.Threading.Tasks;
using AndroidHUD;

namespace WineHangouts
{
    [Activity(Label = "My Tastings", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MyTastingActivity : Activity, IPopupParent
    {
        public int customerid;
        private int screenid = 6;
		public Context parent;
        List<Tastings> myArr1;
        protected override void OnCreate(Bundle bundle)
        {
			Stopwatch st = new Stopwatch();
			st.Start();
			base.OnCreate(bundle);
            customerid = Convert.ToInt32(CurrentUser.getUserId());
            try
            {
                LoggingClass.LogInfo("Entered into My Tasting", screenid);
                this.ActionBar.SetHomeButtonEnabled(true);
                this.ActionBar.SetDisplayHomeAsUpEnabled(true);
                ServiceWrapper svc = new ServiceWrapper();
                var MYtastings = svc.GetMyTastingsList(customerid).Result;
                myArr1 = MYtastings.TastingList.ToList();
				if (MYtastings.TastingList.Count == 0)
				{
                    SetContentView(Resource.Layout.EmptyTaste);
                    TextView te = FindViewById<TextView>(Resource.Id.textView123a);   
                }
				else
				{
                    SetContentView(Resource.Layout.MyTasting);
                    BindGridData();
                    SwipeRefreshLayout mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.PullDownRefresh1);
                    mSwipeRefreshLayout.Refresh += async delegate {
                        //LoggingClass.LogInfo("Refreshed My Tasting", screenid);
                        await SomeAsync();
                        mSwipeRefreshLayout.Refreshing = false;
                    };

                    ActionBar.SetHomeButtonEnabled(true);
                    ActionBar.SetDisplayHomeAsUpEnabled(true);   
				}
				ProgressIndicator.Hide();
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
			st.Stop();
			LoggingClass.LogTime("Tastingactivity", st.Elapsed.TotalSeconds.ToString());
		}
        public async Task SomeAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            BindGridData();
        }
        private void BindGridData()
        {
            
            try
            {
                ServiceWrapper svc = new ServiceWrapper();

                var MYtastings = svc.GetMyTastingsList(customerid).Result;
                myArr1 = MYtastings.TastingList.ToList();
                ListView wineList = FindViewById<ListView>(Resource.Id.MyTasting);
                wineList.Clickable = true;
                MyTastingAdapter adapter = new MyTastingAdapter(this, MYtastings.TastingList.ToList());
                wineList.Adapter = adapter;
                wineList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    string WineBarcode = myArr1[args.Position].Barcode;
                    int storeID = myArr1[args.Position].PlantFinal;
                    LoggingClass.LogInfo("Clicked on " + myArr1[args.Position].Barcode + " to enter into wine from tasting  details", screenid);
                     ProgressIndicator.Show(this);
                    var intent = new Intent(this, typeof(DetailViewActivity));
                    intent.PutExtra("WineBarcode", WineBarcode);
                    intent.PutExtra("storeid", storeID);
                    StartActivity(intent);
                };
                ProgressIndicator.Hide();
                AndHUD.Shared.Dismiss();
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
        }
        private void MSwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            BindGridData();
            SwipeRefreshLayout mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.PullDownRefresh1);
            LoggingClass.LogInfo("Refreshed My Tasting", screenid);
            mSwipeRefreshLayout.Refreshing = false;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();  
                GC.Collect();
                LoggingClass.LogInfo("Exited from My Tasting", screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
		protected override void OnPause()
		{
			base.OnPause();
			LoggingClass.LogInfo("OnPause state in MyTasting ativity", screenid);
		}
        public async override void OnBackPressed()
        {
            //MoveTaskToBack(true);
            var intent = new Intent(this, typeof(Login)); 
            StartActivity(intent);
            GC.Collect();
        }
        protected override void OnResume()
		{
			base.OnResume();
		}
		private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        private int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
        public void RefreshParent()
        {
            ServiceWrapper svc = new ServiceWrapper();
            var MYtastings = svc.GetMyTastingsList(customerid).Result;
            ListView wineList = FindViewById<ListView>(Resource.Id.MyTasting);
            MyTastingAdapter adapter = new MyTastingAdapter(this, MYtastings.TastingList.ToList());
            wineList.Adapter = adapter;
            adapter.NotifyDataSetChanged();
        }
    }
}