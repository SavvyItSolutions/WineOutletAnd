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
	//[Activity(Label = "Alert")]
	public class AlertActivity : Activity
{
        public int screenid = 25;
        //	protected override void OnCreate(Bundle savedInstanceState)
        //	{
        //		base.OnCreate(savedInstanceState);

        //		// Create your application here
        //	}
        //	public void IncorrectDetailsAlert()
        //	{
        //		AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
        //		LoggingClass.LogInfo("Entered Incorrect Details", screenid);
        //		aler.SetTitle("Sorry");
        //		aler.SetMessage("Incorrect Details");
        //		aler.SetNegativeButton("Ok", delegate { });
        //		Dialog dialog = aler.Create();
        //		dialog.Show();
        //	}
        //	public void ThankuYouAlert()
        //	{
        //           AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
        //           aler.SetTitle("Sorry");
        //           aler.SetMessage("This Feature is available for VIP Users only");
        //           aler.SetPositiveButton("Login", delegate
        //           {
        //               var intent = new Intent(this, typeof(LoginActivity));
        //               StartActivity(intent);
        //           });
        //           aler.SetNegativeButton("KnowMore", delegate
        //           {
        //               var uri = Android.Net.Uri.Parse("https://hangoutz.azurewebsites.net/index.html");
        //               var intent = new Intent(Intent.ActionView, uri);
        //               StartActivity(intent);

        //           });
        //           aler.SetNeutralButton("Cancel", delegate
        //           {

        //           });
        //           Dialog dialog1 = aler.Create();
        //           dialog1.Show();
        //       }
        //	public void IncorrectUserNameAlert()
        //	{
        //		AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
        //		aler.SetTitle("Sorry");
        //		aler.SetMessage("Please Enter Correct username");
        //		aler.SetNegativeButton("Ok", delegate { });
        //		Dialog dialog = aler.Create();
        //		dialog.Show();
        //	}
        //	public void CheckInternetAlert()
        //	{
        //		AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
        //		aler.SetTitle("Sorry");
        //		LoggingClass.LogInfo("Please check your internet connection", screenid);
        //		aler.SetMessage("Please check your internet connection");
        //		aler.SetNegativeButton("Ok", delegate { });
        //		Dialog dialog = aler.Create();
        //		dialog.Show();
        //	}
        //	public void UndermaintenenceAlert()
        //	{
        //		AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
        //		aler.SetTitle("Sorry");
        //		LoggingClass.LogInfo("We're under maintanence", screenid);
        //		aler.SetMessage("We're under maintanence");
        //		aler.SetNegativeButton("Ok", delegate { });
        //		Dialog dialog = aler.Create();
        //		dialog.Show();
        //	}
        public void UnAuthourised()
        {
            AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
            aler.SetTitle("Invalid Credentials");
            LoggingClass.LogInfo("Invalid Credentials", screenid);
            aler.SetMessage("BarCode is invalid. Please try again!");
            aler.SetNegativeButton("Ok", delegate { });
            Dialog dialog = aler.Create();
            dialog.Show();
        }
    }
}