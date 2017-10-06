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
using System.Diagnostics;
using Android.Content.PM;

namespace WineHangouts
{
    [Activity(Label = "About Us" ,ScreenOrientation = ScreenOrientation.Portrait)]
    public class AboutActivity : Activity
    {
        private int screenid = 11;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                SetContentView(Resource.Layout.About);
              LoggingClass.LogInfo("Entered into About Us", screenid);
            }
            catch
            { }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                LoggingClass.LogInfo("Exited from About Us", screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
        protected override void OnPause()
        {
            base.OnPause();
            LoggingClass.LogInfo("OnPause state in About activity", screenid);

        }
        protected override void OnResume()
        {
            base.OnResume();
            LoggingClass.LogInfo("OnResume state in About activity", screenid);
        }
        public override void OnLowMemory()
        {
            GC.Collect();
        }
    }
}