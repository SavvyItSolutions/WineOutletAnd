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
using Android.Graphics;
using Android.Util;
using Hangout.Models;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Android.Support;
using System.Diagnostics;
using AndroidHUD;

namespace WineHangouts
{
    [Activity(Label = "Wine Details", MainLauncher = false, Icon = "@drawable/logo5", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class DetailViewActivity : Activity, IPopupParent
    {
        public int flag;
        public RatingBar RatingInput;
        public RatingBar AvgRating;
        public TextView WineName;
        public TextView WineDescription;
        public TextView Vintage;
        public TextView WineProducer;
        public TextView ErrorDescription;
        public ItemDetailsResponse myData;
        public int sku;
        private int screenid = 4;
        private int storeid = 0;
        public ListView commentsview;
        public reviewAdapter comments;
        WebClient webClient;
        ImageView HighImageWine;
        public string WineBarcode;
        List<Review> ReviewArray;
        ReviewPopup editPopup;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //AndHUD.Shared.Dismiss();
            CheckInternetConnection();
            Stopwatch st = new Stopwatch();
            st.Start();
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.detailedView);
            //         WineBarcode = Intent.GetStringExtra("WineBarcode");
            //storeid = Intent.GetIntExtra("storeid", 1);
            //ActionBar.SetHomeButtonEnabled(true);
            //ActionBar.SetDisplayHomeAsUpEnabled(true);
            //ServiceWrapper svc = new ServiceWrapper();
            //         myData = new ItemDetailsResponse();
            //ItemReviewResponse SkuRating = new ItemReviewResponse();
            //this.Title = "Wine Details";
            commentsview = FindViewById<ListView>(Resource.Id.listView2);
            WineName = FindViewById<TextView>(Resource.Id.txtWineName); //Assigning values to respected Textfields
            WineName.Focusable = false;
            WineProducer = FindViewById<TextView>(Resource.Id.txtProducer);
            WineProducer.Focusable = false;
            Vintage = FindViewById<TextView>(Resource.Id.txtVintage);
            Vintage.Focusable = false;
            WineDescription = FindViewById<TextView>(Resource.Id.txtWineDescription);
            WineDescription.Focusable = false;
            AvgRating = FindViewById<RatingBar>(Resource.Id.avgrating);
            AvgRating.Focusable = false;
            ErrorDescription = FindViewById<TextView>(Resource.Id.Error);
            ErrorDescription.Focusable = false;
            ErrorDescription.Visibility = ViewStates.Gone;
            TableRow tr5 = FindViewById<TableRow>(Resource.Id.tableRow5);
            WineBarcode = Intent.GetStringExtra("WineBarcode");
            storeid = 1;// Intent.GetIntExtra("storeid", 1);
            Review edit = new Review()
            {
                Barcode = WineBarcode,
                RatingText = "",
                PlantFinal = storeid.ToString()
            };

            ItemReviewResponse SkuRating = new ItemReviewResponse();
            ServiceWrapper svc = new ServiceWrapper();
            SkuRating = svc.GetItemReviewsByWineBarcode(WineBarcode).Result;
            var tempReview = SkuRating.Reviews.ToList().Find(x => x.ReviewUserId == Convert.ToInt32(CurrentUser.getUserId()));
            if (tempReview != null)
                edit.RatingText = tempReview.RatingText;
            editPopup = new ReviewPopup(this, edit);
            RatingInput = FindViewById<RatingBar>(Resource.Id.ratingInput);//Taking rating stars input
            RatingInput.RatingBarChange += editPopup.CreatePopup;

            flag = 1;
            Internal_ViewDidLoad();
            st.Stop();
            //AndHUD.Shared.Dismiss();
            try
            {
                ProgressIndicator.Hide();
            }
            catch (Exception exe) { LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString()); }
            LoggingClass.LogTime("Detail activity", st.Elapsed.TotalSeconds.ToString());
        }

        private void RatingInput_RatingBarChange(object sender, RatingBar.RatingBarChangeEventArgs e)
        {

            if ((CurrentUser.getUserId() != null || CurrentUser.getUserId() != "0") && (CurrentUser.getUserId() != null))
            {
                editPopup.CreatePopup(sender, e);
            }
            else
            {

                AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                aler.SetTitle("Sorry");
                aler.SetMessage("This feature is available only for VIP Customers.");
                aler.SetPositiveButton("KnowMore", delegate
                {
                    var uri = Android.Net.Uri.Parse("https://hangoutz.azurewebsites.net/index.html");
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);



                });
                aler.SetNegativeButton("Ok", delegate
                {

                });
                Dialog dialog1 = aler.Create();
                dialog1.SetCanceledOnTouchOutside(false);
                dialog1.Show();
                RatingInput.Rating = 0;
            }
            RatingInput.RatingBarChange -= RatingInput_RatingBarChange;
        }

        public void Internal_ViewDidLoad()
        {
            try
            {
                WineBarcode = Intent.GetStringExtra("WineBarcode");
                storeid = 1;//Intent.GetIntExtra("storeid", 1);
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                ServiceWrapper svc = new ServiceWrapper();
                myData = new ItemDetailsResponse();
                ItemReviewResponse SkuRating = new ItemReviewResponse();
                this.Title = "Wine Details";
                try
                {
                    DownloadAsync(this, System.EventArgs.Empty, WineBarcode);
                    myData = svc.GetItemDetails(WineBarcode, 1).Result;
                    SkuRating = svc.GetItemReviewsByWineBarcode(WineBarcode).Result;
                    ReviewArray = SkuRating.Reviews.ToList();
                    var tempReview = SkuRating.Reviews.ToList().Find(x => x.ReviewUserId == Convert.ToInt32(CurrentUser.getUserId()));
                    if (tempReview != null)
                        editPopup._editObj.RatingText = tempReview.RatingText;
                    comments = new reviewAdapter(this, ReviewArray);
                    if (comments.Count == 0)
                    {
                        ErrorDescription.Visibility = ViewStates.Visible;
                        ErrorDescription.Text = SkuRating.ErrorDescription;
                        ErrorDescription.SetTextColor(Android.Graphics.Color.Black);
                    }
                    else
                        commentsview.Adapter = comments;
                    setListViewHeightBasedOnChildren1(commentsview);
                    WineName.Text = myData.ItemDetails.Name;

                    WineName.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
                    Vintage.Text = myData.ItemDetails.Vintage.ToString();
                    if (Vintage.Text == null || Vintage.Text == "0") { Vintage.Text = ""; } else { Vintage.Text = myData.ItemDetails.Vintage.ToString(); }
                    if (myData.ItemDetails.Producer == null || myData.ItemDetails.Producer == "")
                    {
                        WineProducer.Text = "Not Available";
                    }
                    else
                    {
                        WineProducer.Text = myData.ItemDetails.Producer;
                    }
                    if (myData.ItemDetails.Description == null || myData.ItemDetails.Description == "")
                    {
                        WineDescription.Text = "Not Available";
                    }
                    else
                    {
                        WineDescription.Text = myData.ItemDetails.Description;
                    }
                    AvgRating.Rating = (float)myData.ItemDetails.AverageRating;

                    //ReviewPopup editPopup = new ReviewPopup(this, edit);
                    //RatingBar RatingInput = FindViewById<RatingBar>(Resource.Id.ratingInput);//Taking rating stars input
                    //RatingInput.RatingBarChange += editPopup.CreatePopup;

                    var metrics = Resources.DisplayMetrics;
                    var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
                    var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);


                    HighImageWine = FindViewById<ImageView>(Resource.Id.WineImage);

                    BitmapFactory.Options options = new BitmapFactory.Options
                    {
                        InJustDecodeBounds = false,
                        OutHeight = 75,
                        OutWidth = 75

                    };
                    // ProgressIndicator.Hide();
                    LoggingClass.LogInfo("Entered into detail view" + WineBarcode, screenid);
                    // Bitmap result = BitmapFactory.DecodeResource(Resources, Resource.Drawable.placeholder_re, options);
                }
                catch (Exception exe)
                {
                    LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                    AlertDialog.Builder alert = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                    alert.SetTitle("Sorry");
                    alert.SetMessage("We're under maintainence");
                    alert.SetNegativeButton("Ok", delegate { });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                    try
                    {
                        ProgressIndicator.Hide();
                    }
                    catch (Exception exe1)
                    {
                        LoggingClass.LogError(exe1.Message, screenid, exe1.StackTrace.ToString());
                    }

                }
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
        }
        public bool CheckInternetConnection()
        {

            string CheckUrl = "http://google.com";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout = 5000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                // Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

                return true;

            }
            catch (WebException ex)
            {
                AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                aler.SetTitle("Sorry");
                LoggingClass.LogInfo("Please check your internet connection", screenid);
                aler.SetMessage("Please check your internet connection");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();
                return false;
            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                string Back= Intent.GetStringExtra("Back");
                if (Back == "Back")
                {
                    var intent = new Intent(this, typeof(Login));
                    StartActivity(intent);
                }
                else
                {
                    AndHUD.Shared.Dismiss();
                    base.OnBackPressed();
                    GC.Collect();
                }
                AndHUD.Shared.Dismiss();
                try
                {
                    ProgressIndicator.Hide();
                }
                catch (Exception ex) { }
                LoggingClass.LogInfo("Exited from Detail View", screenid);
                
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
        public async override void OnBackPressed()
        {
            string Back = Intent.GetStringExtra("Back");
            if (Back == "Back")
            {
                var intent = new Intent(this, typeof(Login));
                StartActivity(intent);
            }
            else
            {
                Finish();
            }
            // MoveTaskToBack(true);
            GC.Collect();
        }
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        public void setListViewHeightBasedOnChildren(ListView listView)
        {
            DetailsViewAdapter listAdapter = (DetailsViewAdapter)listView.Adapter;
            if (listAdapter == null)
                return;

            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Unspecified);
            int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(ViewGroup.LayoutParams.WrapContent, MeasureSpecMode.Exactly);
            int totalHeight = 0;
            View view = null;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                view = listAdapter.GetView(i, view, listView);
                if (i == 0)
                    view.LayoutParameters = new ViewGroup.LayoutParams(desiredWidth, WindowManagerLayoutParams.WrapContent);

                view.Measure(desiredWidth, heightMeasureSpec);
                totalHeight += view.MeasuredHeight;
            }
            ViewGroup.LayoutParams params1 = listView.LayoutParameters;
            params1.Height = totalHeight + (listView.DividerHeight * (listAdapter.Count - 1));
            listView.LayoutParameters = params1;
        }
        public void setListViewHeightBasedOnChildren1(ListView listView1)
        {
            reviewAdapter listAdapter = (reviewAdapter)listView1.Adapter;
            if (listAdapter == null)
                return;

            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(listView1.Width, MeasureSpecMode.Unspecified);
            int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(ViewGroup.LayoutParams.WrapContent, MeasureSpecMode.Exactly);
            int totalHeight = 0;
            View view = null;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                view = listAdapter.GetView(i, view, listView1);
                if (i == 0)
                    view.LayoutParameters = new ViewGroup.LayoutParams(desiredWidth, WindowManagerLayoutParams.WrapContent);

                view.Measure(desiredWidth, heightMeasureSpec);
                totalHeight += view.MeasuredHeight;
            }
            ViewGroup.LayoutParams params1 = listView1.LayoutParameters;
            params1.Height = totalHeight + (listView1.DividerHeight * (listAdapter.Count - 1));
            listView1.LayoutParameters = params1;
        }

        public int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
        public void RefreshParent()
        {
            Internal_ViewDidLoad();
            ErrorDescription.Visibility = ViewStates.Gone;
            ServiceWrapper svc = new ServiceWrapper();
            int wineid = Intent.GetIntExtra("WineID", 138);
            myData = svc.GetItemDetails(WineBarcode, storeid).Result;
            AvgRating.Rating = (float)myData.ItemDetails.AverageRating;
            //         var SkuRating = svc.GetItemReviewsByWineBarcode(WineBarcode).Result;
            //comments = new reviewAdapter(this, SkuRating.Reviews.ToList());
            //         commentsview.Adapter = comments;
            //comments.NotifyDataSetChanged();


        }

        public async void DownloadAsync(object sender, System.EventArgs ea, string WineBarcode)
        {
            webClient = new WebClient();
            string url = null;
            if (storeid == 1)
            {
                url = "https://icsintegration.blob.core.windows.net/barcodeppdetail/" + WineBarcode + ".jpg";
                LoggingClass.LogInfo("Download Async image in detail view" + WineBarcode + +',' + storeid, screenid);
            }
            else if (storeid == 2)
            {
                url = "https://icsintegration.blob.core.windows.net/barcodeppdetail/" + WineBarcode + ".jpg";
                LoggingClass.LogInfo("Download Async image in detail view" + WineBarcode + +',' + storeid, screenid);
            }
            byte[] imageBytes = null;
            //progressLayout.Visibility = ViewStates.Visible;
            try
            {
                imageBytes = await webClient.DownloadDataTaskAsync(url);
            }


            catch (TaskCanceledException)
            {
                //this.progressLayout.Visibility = ViewStates.Gone;
                return;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError("while downloading image of wine id" + WineBarcode + "  " + exe.Message, screenid, exe.StackTrace.ToString());
                //progressLayout.Visibility = ViewStates.Gone;
                //downloadButton.Click += downloadAsync;
                //downloadButton.Text = "Download Image";
                Bitmap imgWine = BlobWrapper.Bottleimages(WineBarcode, storeid);
                HighImageWine.SetImageBitmap(imgWine);
                if (imgWine == null)
                {
                    HighImageWine.SetImageResource(Resource.Drawable.bottle);
                }
                return;
            }

            try
            {
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string localFilename = "Wine.png";
                string localPath = System.IO.Path.Combine(documentsPath, localFilename);

                FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
                await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
                fs.Close();

                BitmapFactory.Options options = new BitmapFactory.Options()
                {
                    InJustDecodeBounds = true
                };
                await BitmapFactory.DecodeFileAsync(localPath, options);

                Bitmap bitmap = await BitmapFactory.DecodeFileAsync(localPath);
                if (bitmap == null)
                {
                    HighImageWine.SetImageResource(Resource.Drawable.bottle);
                }
                HighImageWine.SetImageBitmap(bitmap);
            }
            catch (Exception exe)
            {
                LoggingClass.LogError("While setting High resulution image" + exe.Message, screenid, exe.StackTrace.ToString());

            }

            //progressLayout.Visibility = ViewStates.Gone;
            //downloadButton.Click += downloadAsync;
            //downloadButton.Enabled = false;
            HighImageWine.Dispose();
            //downloadButton.Text = "Download Image";
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
    }

}

