    using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Hangout.Models;
using System.Linq;
using Android.Util;
using System.Threading;
using Android.Support.V4.Widget;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using AndroidHUD;

namespace WineHangouts
{

    [Activity(Label = "Available Wines", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class GridViewActivity : Android.Support.V4.App.FragmentActivity
    {
        public string  WineBarcode;
        public string StoreName = "";
		private int screenid = 3;
        GridViewAdapter adapter;
        protected override void OnCreate(Bundle bundle)
        {
			Stopwatch st = new Stopwatch();
			st.Start();
			base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            try
            {
				
				CheckInternetConnection();
				if (StoreName == "")
                    StoreName = Intent.GetStringExtra("MyData");
                this.Title = StoreName;
                this.ActionBar.SetHomeButtonEnabled(true);
                this.ActionBar.SetDisplayShowTitleEnabled(true);
               //  ToolbartItems.Add(new ToolbarItem { Text = "BTN 1", Icon = "myicon.png" });
                BindGridData();
                SwipeRefreshLayout mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.PullDownRefresh);
                mSwipeRefreshLayout.Refresh  += async delegate {
                    //BindGridData();
                    LoggingClass.LogInfo("Refreshed grid view",screenid);
                    await SomeAsync();
                    mSwipeRefreshLayout.Refreshing = false;
                };

                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
               
                ProgressIndicator.Hide();
                LoggingClass.LogInfo("Entered into gridview activity", screenid);
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                ProgressIndicator.Hide();
                AlertDialog.Builder aler = new AlertDialog.Builder(this);
                aler.SetTitle("Sorry");
                aler.SetMessage("We're under maintainence");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();

            }

			st.Stop();
			LoggingClass.LogTime("Gridactivity",st.Elapsed.TotalSeconds.ToString());
        }
        public async Task SomeAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            BindGridData();
        }
        private void BindGridData()
        {
            int StoreId = 0;
            if (StoreName == AppConstants.WallStore)
                StoreId = 1;
            else if (StoreName == AppConstants.PointPleasantStore)
                StoreId = 2;
            else if(StoreName == AppConstants.SecaucusStore)
                StoreId = 3;
            try
            {
				
                int userId = Convert.ToInt32(CurrentUser.getUserId());
                ServiceWrapper sw = new ServiceWrapper();
                ItemListResponse output = sw.GetItemList(StoreId, userId).Result;

                List<Item> myArr = output.ItemList.ToList();
                if (myArr.Count == 0)
                {
                    AlertDialog.Builder aler = new AlertDialog.Builder(this);
                    aler.SetTitle("Secaucus Store");
                    aler.SetMessage("Coming Soon!");
                    aler.SetNegativeButton("Ok", delegate {
                        var intent = new Intent(this, typeof(Login)); 
                        StartActivity(intent);
                    });
                    LoggingClass.LogInfo("Clicked on Secaucus", screenid);
                    Dialog dialog = aler.Create();
                    dialog.Show();
                }
                LoggingClass.LogInfo("entered into "+StoreName, screenid);
				var gridview = FindViewById<GridView>(Resource.Id.gridview);
                adapter = new GridViewAdapter(this, myArr,StoreId);
				LoggingClass.LogInfoEx("Entered into Grid View Adapter", screenid);
				gridview.SetNumColumns(2);
                gridview.Adapter = adapter;
                gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    WineBarcode = myArr[args.Position].Barcode;
                    ProgressIndicator.Show(this);
                    var intent = new Intent(this, typeof(DetailViewActivity));
                    LoggingClass.LogInfo("Clicked on " + myArr[args.Position].Barcode + " to enter into wine details",screenid);
                    intent.PutExtra("WineBarcode", WineBarcode);
                    StartActivity(intent);
                  
                };
				
			}
            catch(Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
        }
		protected override void OnPause()
		{
			base.OnPause();
            if (StoreName == AppConstants.WallStore)
            {
                var intent = new Intent(this, typeof(GridViewActivity));
                intent.PutExtra(AppConstants.MyData, AppConstants.WallStore);
            }
            else
                if (StoreName == AppConstants.PointPleasantStore)
            {
                var intent = new Intent(this, typeof(GridViewActivity));
                intent.PutExtra(AppConstants.MyData, AppConstants.PointPleasantStore);
            }
            else
                if (StoreName == AppConstants.SecaucusStore)
            {
                var intent = new Intent(this, typeof(GridViewActivity));
                intent.PutExtra(AppConstants.MyData, AppConstants.SecaucusStore);
            }
            else { }

        }
        protected override void OnResume()
        {
            base.OnPause();
           

        }
        public  bool CheckInternetConnection()
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
			catch (WebException ex)
			{
				AlertDialog.Builder aler = new AlertDialog.Builder(this,Resource.Style.MyDialogTheme);
				aler.SetTitle("Sorry");
				LoggingClass.LogInfo("Please check your internet connection"+ex, screenid);
				aler.SetMessage("Please check your internet connection");
				aler.SetNegativeButton("Ok", delegate { });
				Dialog dialog = aler.Create();
				dialog.Show();
				return false;
			}
		}
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Drawable.options_menu_1,menu);
            return base.OnCreateOptionsMenu(menu);
        }
        private void MSwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            BindGridData();
            SwipeRefreshLayout mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.PullDownRefresh);
			//LoggingClass.LogInfo("Refreshed GridView", screenid);
			mSwipeRefreshLayout.Refreshing =false;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent intent = null;

            switch (item.ItemId)
            {

                case Resource.Id.action_settings3:
                    ProgressIndicator.Show(this);
                    intent = new Intent(this, typeof(AboutActivity));
                    break;
             
                  
            }
            if (intent != null)
            {
                StartActivity(intent);
            }
            if (item.ItemId == Android.Resource.Id.Home)
            {
                base.OnBackPressed();
              
                intent = new Intent(this, typeof(Login));
                StartActivity(intent);
                LoggingClass.LogInfo("Exited from Gridview Activity",screenid);
                ProgressIndicator.Hide();
				return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        public async override void OnBackPressed()
        {
           
            var intent = new Intent(this, typeof(Login));
            LoggingClass.LogInfo("Clicked on options menu About", screenid);
            StartActivity(intent);
            GC.Collect();

        }
        public override void OnLowMemory()
        {
            GC.Collect();
        }
    }
    
}

