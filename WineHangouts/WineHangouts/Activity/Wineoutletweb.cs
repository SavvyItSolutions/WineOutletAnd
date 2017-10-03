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
using Android.Webkit;

namespace WineHangouts
{
    [Activity(Label = "Wineoutletweb")]
    public class Wineoutletweb : Activity
    {
        public WebView web_view;
        public class HelloWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.LoadUrl(url);
                return false;
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            this.Title = "Wine";
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WineoutletWeb);

            web_view = FindViewById<WebView>(Resource.Id.webView1);
            web_view.Settings.JavaScriptEnabled = true;
            web_view.SetWebViewClient(new HelloWebViewClient());
            web_view.LoadUrl("http://www.wineoutlet.com/");
           
            // Create your application here
        }
        public bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            view.LoadUrl(url);
            return false;
        }
        public override bool OnKeyDown(Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            if (keyCode == Keycode.Back && web_view.CanGoBack())
            {
                web_view.GoBack();
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
        public async override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(Login));
           
            StartActivity(intent);
            // MoveTaskToBack(true);
            ProgressIndicator.Hide();
        }
        protected override void OnPause()
        {
            base.OnPause();
    

        }

        protected override void OnResume()
        {
            base.OnResume();
          
        }
    }
}