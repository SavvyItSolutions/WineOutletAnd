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
using System.Diagnostics;
using Hangout.Models;
using AndroidHUD;

namespace WineHangouts
{
    class ReviewPopup
    {
        Context Parent;
        private string WineBarcode;
        private int ParentScreenId=17;
        public Review _editObj;
        string storeid;
        public ReviewPopup(Context parent, Review EditObj)
        {
            Parent = parent;
            _editObj = EditObj;
            WineBarcode = EditObj.Barcode;
            storeid = _editObj.PlantFinal;
        }
        private GestureDetector _detector;

        private class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                // TODO
                return true;
            }
        }
        public void CreatePopup(object sender, RatingBar.RatingBarChangeEventArgs e)
        {

            if (CurrentUser.getUserId() == null|| CurrentUser.getUserId() == "0")
            {
                AlertDialog.Builder aler = new AlertDialog.Builder(Parent, Resource.Style.MyDialogTheme);
                aler.SetTitle("Sorry");
                aler.SetMessage("This Feature is available for VIP Users only");
                aler.SetNegativeButton("Ok", delegate {
                   
                });
                Dialog dialog1 = aler.Create();
                dialog1.Show();
            }
            else
            try
            {
                Dialog editDialog = new Dialog(Parent);
                var rat = e.Rating;
                editDialog.SetContentView(Resource.Layout.EditReviewPopup);
                ServiceWrapper sw = new ServiceWrapper();
                Review review = new Review();
                ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.close);
                Button btnSubmitReview = editDialog.FindViewById<Button>(Resource.Id.btnSubmitReview);
                TextView Comments = editDialog.FindViewById<TextView>(Resource.Id.txtReviewComments);
                RatingBar custRating = editDialog.FindViewById<RatingBar>(Resource.Id.rating);
                custRating.Rating = rat;
                Comments.Text = _editObj.RatingText;
                int screenid = 9;
                close.SetScaleType(ImageView.ScaleType.CenterCrop);
                editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
                editDialog.Show();
                    editDialog.SetCanceledOnTouchOutside(false);
                    LoggingClass.LogInfo("Entered into CreatePopup", screenid);
                   
                    close.Click += delegate
                {
					LoggingClass.LogInfo("Closed PoPup", screenid);
					editDialog.Dismiss();
                };
                btnSubmitReview.Click += async delegate
                {
                        AndHUD.Shared.Show(Parent, "Saving Review...", Convert.ToInt32(MaskType.Clear));
                      //  ProgressIndicator.Show(Parent);
                        review.ReviewDate = DateTime.Now;
						review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
						review.Username = CurrentUser.getUserName();
						review.RatingText = Comments.Text;
						review.RatingStars = Convert.ToInt32(custRating.Rating);
						review.IsActive = true;
						review.Barcode =WineBarcode ;
						review.PlantFinal = storeid;
						LoggingClass.LogInfo("Submitted review---->" + review.RatingStars + " ---->" + review.RatingText + "---->" + review.PlantFinal + "---->" + review.Barcode, screenid);
						await sw.InsertUpdateReview(review);
						((IPopupParent)Parent).RefreshParent();
						ProgressIndicator.Hide();
						editDialog.Dismiss();
                        AndHUD.Shared.Dismiss();
                        AndHUD.Shared.ShowSuccess(Parent, "Sucessfully Saved", MaskType.Clear, TimeSpan.FromSeconds(2));
                };
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, ParentScreenId, exe.StackTrace.ToString());
            }
        }

        public void EditPopup(object sender, EventArgs e)
        {
            try
            {
                Dialog editDialog = new Dialog(Parent);
                int screenid = 18;
                editDialog.SetContentView(Resource.Layout.EditReviewPopup);
                ServiceWrapper sw = new ServiceWrapper();
                Review review = new Review();
                ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.close);
                Button btnSubmitReview = editDialog.FindViewById<Button>(Resource.Id.btnSubmitReview);
                TextView Comments = editDialog.FindViewById<TextView>(Resource.Id.txtReviewComments);
                RatingBar custRating = editDialog.FindViewById<RatingBar>(Resource.Id.rating);
                Comments.Text = _editObj.RatingText;
                custRating.Rating = _editObj.RatingStars;
				LoggingClass.LogInfo("Entered into EditPopup", screenid);
				close.SetScaleType(ImageView.ScaleType.CenterCrop);
                editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
                editDialog.Show();
                editDialog.SetCanceledOnTouchOutside(false);

                close.Click += delegate
                {
					LoggingClass.LogInfo("Closed PoPup", screenid);
					editDialog.Dismiss();
                };
                btnSubmitReview.Click += async delegate
                {
                    AndHUD.Shared.Show(Parent, "Saving Review...", Convert.ToInt32(MaskType.Clear));
                    review.ReviewDate = DateTime.Now;
                    review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
                    review.RatingText = Comments.Text;
                    review.RatingStars = Convert.ToInt32(custRating.Rating);
                    review.IsActive = true;
                    review.PlantFinal =storeid;
                    review.Barcode = WineBarcode;
                    try
                    {
                        await sw.InsertUpdateReview(review);
                        LoggingClass.LogInfo("Edited Review-----> "+ review.RatingText+"-----> "+review.RatingStars+ "----->"+review.Barcode+ "----->"+review.PlantFinal+"submitted",screenid);
                    }

                    catch (Exception exe)
                    {
                        LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                    }
                    ((IPopupParent)Parent).RefreshParent();
                    AndHUD.Shared.Dismiss();
                    AndHUD.Shared.ShowSuccess(Parent, "Sucessfully Saved", MaskType.Clear, TimeSpan.FromSeconds(2));
                    editDialog.Dismiss();
                };
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, ParentScreenId, exe.StackTrace.ToString());
            }
        }
        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Outside:

                    AlertDialog.Builder aler = new AlertDialog.Builder(Parent, Resource.Style.MyDialogTheme);
                    aler.SetTitle("Sorry");
                    aler.SetMessage("This Feature is available for VIP Users only");
                    aler.SetNegativeButton("Ok", delegate {

                    });
                    Dialog dialog1 = aler.Create();
                    dialog1.Show();
                    break;
            }
            return true;
        }
    }
    public interface IPopupParent
    {
       
        void RefreshParent();
    }
}