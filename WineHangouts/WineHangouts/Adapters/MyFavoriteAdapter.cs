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
using System.Net;
using Hangout.Models;
using Java.Util;
using System.Globalization;

namespace WineHangouts
{
	[Activity(Label = "MyFavoriteAdapter")]
	public class MyFavoriteAdapter : BaseAdapter<Item>
	{
		private List<Item> myItems;
		private Context myContext;
        public int storeid;
        public override Item this[int position]
		{
			get
			{
				return myItems[position];
			}
		}
		public MyFavoriteAdapter(Context con, List<Item> strArr)
		{
            myContext = con;
			myItems = strArr;
		}
		public override int Count
		{
			get
			{
				return myItems.Count;
			}
		}

		public static class Cultures
		{
			public static readonly CultureInfo UnitedState =
				CultureInfo.GetCultureInfo("en-US");
		}

		public override long GetItemId(int position)
		{
			return position;
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = convertView;
			if (row == null)
				row = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyFavorite, null, false);
				TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
				TextView txtVintage = row.FindViewById<TextView>(Resource.Id.txtVintage);
				TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
				ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
                ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imgHeart);
				RatingBar rating = row.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
				rating.Rating = (float)myItems[position].AverageRating;
				txtName.Text = myItems[position].Name;
				txtPrice.Text = myItems[position].SalePrice.ToString("C", Cultures.UnitedState);
         
            txtVintage.Text = myItems[position].Vintage.ToString();
            if (txtVintage.Text == "0" || txtVintage.Text == null)
            {
                txtVintage.Text = "";
            }
            else
            {
                txtVintage.Text = myItems[position].Vintage.ToString();
            }
                heartImg.SetImageResource(Resource.Drawable.heart_empty);
				bool count = Convert.ToBoolean(myItems[position].IsLike);
				if (count == true)
				{
					heartImg.SetImageResource(Resource.Drawable.heart_full);
				}
				else
				{
					heartImg.SetImageResource(Resource.Drawable.heart_empty);
				}

				heartImg.Tag = position;

				if (convertView == null)
				{
					heartImg.Click +=  delegate
					{
						int actualPosition = Convert.ToInt32(heartImg.Tag);
						bool x;
						if (count == false)
						{
							heartImg.SetImageResource(Resource.Drawable.heart_full);
							
							x = true;
							count = true;
						}
						else
						{
							heartImg.SetImageResource(Resource.Drawable.heart_empty);
							
							x = false;
							count = false;
						}
						
                        var TaskA = new System.Threading.Tasks.Task(async () =>
                        {
                            SKULike like = new SKULike();
                            like.UserID = Convert.ToInt32(CurrentUser.getUserId());
                            like.SKU = Convert.ToInt32(myItems[actualPosition].SKU);
                            like.Liked = x;
                            myItems[actualPosition].IsLike = x;
                            like.BarCode = myItems[actualPosition].Barcode;
                            ServiceWrapper sw = new ServiceWrapper();
                            await sw.InsertUpdateLike(like);
                        });
                        TaskA.Start();
                    };
				}
				Bitmap imageBitmap;
            string url = myItems[position].SmallImageUrl;
            if (url == null)
            {
                url = myItems[position].Barcode + ".jpg";
            }

                imageBitmap = BlobWrapper.Bottleimages(url, storeid);
            if (imageBitmap != null)
            {
                float ratio = (float)400 / imageBitmap.Height;
                imageBitmap = Bitmap.CreateScaledBitmap(imageBitmap, Convert.ToInt32(imageBitmap.Width * ratio), 400, true);
                imgWine.SetImageBitmap(imageBitmap);
            }
            else
            { 
                imgWine.SetImageResource(Resource.Drawable.bottle);
            }
            txtName.Focusable = false;
            txtVintage.Focusable = false;
			txtPrice.Focusable = false;
			imgWine.Focusable = false;
			imgWine.Dispose();
			return row;
		}
		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int)((pixelValue) / myContext.Resources.DisplayMetrics.Density);
			return dp;
		}
        public  void OnLowMemory()
        {
            GC.Collect();
        }
    }
}
