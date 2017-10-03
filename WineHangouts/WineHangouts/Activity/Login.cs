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

namespace WineHangouts
{
    [Activity(Label = "Wine Outlet", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Login : TabActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
           
            SetContentView(Resource.Layout.Start);

            var intent = new Intent(this, typeof(Activitylo));
            intent.AddFlags(ActivityFlags.ClearTop);
            var spec = TabHost.NewTabSpec("WhatsOn");
            var draw = Resources.GetDrawable(Resource.Drawable.ic_tab_whats_on);
            spec.SetIndicator("", draw);
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            intent = new Intent(this, typeof(Activity12));
            intent.AddFlags(ActivityFlags.ClearTop);
            spec = TabHost.NewTabSpec("Cart");
            draw = Resources.GetDrawable(Resource.Drawable.ic_tab_speakers);
            spec.SetIndicator("", draw);
            spec.SetContent(intent);

            TabHost.AddTab(spec);

            intent = new Intent(this, typeof(Wineoutletweb));
            intent.AddFlags(ActivityFlags.ClearTop);
            spec = TabHost.NewTabSpec("Login");
            draw = Resources.GetDrawable(Resource.Drawable.ic_tab_sessions);
            spec.SetIndicator("", draw);
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            //intent = new Intent(this, typeof(MyScheduleActivity));
            //intent.AddFlags(ActivityFlags.ClearTop);
            //spec = TabHost.NewTabSpec("Contect");
            //draw = Resources.GetDrawable(Resource.Drawable.ic_tab_my_schedule);

            //spec.SetIndicator("", draw);
            //spec.SetContent(intent);
            //TabHost.AddTab(spec);

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            Intent intent = null;

            switch (item.ItemId)
            {

                case Resource.Id.action_settings:
                    ProgressIndicator.Show(this);
                    intent = new Intent(this, typeof(PotraitActivity));
                    //LoggingClass.LogInfo("Clicked on options menu Profile", screenid);
                    break;
                case Resource.Id.action_settings1:
                    //ProgressIndicator.Show(this);
                    try
                    { intent = new Intent(this, typeof(AboutActivity)); }
                    catch (Exception ex)
                    { }

                  //  LoggingClass.LogInfo("Clicked on options menu About", screenid);
                    break;

                case Resource.Id.action_settings2:
                    MoveTaskToBack(true);
                   // LoggingClass.LogInfo("Exited from App", screenid);
                    break;
                default://invalid option
                    return base.OnOptionsItemSelected(item);
            }
            if (intent != null)
            {
                StartActivity(intent);
            }
            try
            {
                //if (item.ItemId == Resource.Id.action_settings3)
                //{
                //    ProgressIndicator.Show(this);
                //    StartActivity(typeof(AutoCompleteTextActivity));
                //}
            }

            catch (Exception exe)
            {
               // LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                throw new Exception();
            }

            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
               // LoggingClass.LogInfo("Clicked on Exit", screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Drawable.options_menu, menu);
            return true;
        }
        public async override void OnBackPressed()
        {

            MoveTaskToBack(true);
            ProgressIndicator.Hide();
        }
    }
}