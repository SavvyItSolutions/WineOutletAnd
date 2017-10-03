using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading;

namespace WineHangouts
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
          
            //Display Splash Screen for 4 Sec  
            Thread.Sleep(1000);
            //Start Activity1 Activity  
            StartActivity(typeof(LoginActivity));
        }
    }
}

