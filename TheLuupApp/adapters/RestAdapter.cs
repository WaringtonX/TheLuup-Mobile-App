using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Refractored.Controls;
using Square.Picasso;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp.adapters
{
    class RestAdapter : RecyclerView.Adapter
    {
        private Service1 client1 = new Service1();
        
        private List<Restuarent>  mRestuarent;
        private string mUid;
        private Context mContext;
        public event EventHandler<int> ItemClick;

        public RestAdapter(Context context,List<Restuarent> restuarent,string Uid) 
        {
            mRestuarent = restuarent;
            mContext = context;
            mUid = Uid;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RestViewHolder vh = holder as RestViewHolder;
            vh.restname.Text = mRestuarent[position].Name;
            vh.restdistance.Text = mRestuarent[position].Location;
            string foodLink = "http://theluup.co.za/images/rescover/" + mRestuarent[position].Coverimage;
            string RestLink = "http://theluup.co.za/images/resimages/" + mRestuarent[position].Image;
            Picasso.With(mContext)
                        .Load(RestLink)
                        .Fit()
                        .Into(vh.restimage);

            Picasso.With(mContext)
                                   .Load(foodLink)
                                   .Fit()
                                   .Into(vh.foodtImage);

            

           /* vh.ItemView.Click += (sender, e) =>
            {
                Intent log = new Intent(mContext, typeof(MenuActivity));
                log.PutExtra("user_id", mUid);
                log.PutExtra("res_id", mRestuarent[position].R_id.ToString());
                mContext.StartActivity(log);
            }; */


        }

        

        public override int ItemCount
        {
            get { return mRestuarent.Count; }
        }

        public void updateViews(List<Restuarent> mrest)
        {
            mRestuarent = mrest;
            NotifyDataSetChanged();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.restuarent_listitem, parent, false);
            RestViewHolder vh = new RestViewHolder(itemView, OnClick);
            return vh;
        }

        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }

      

    }


    public class RestViewHolder : RecyclerView.ViewHolder
    {
        public ImageView foodtImage { get; private set; }
        public CircleImageView restimage { get; private set; }
        public TextView restname { get; private set; }
        public TextView restdistance { get; private set; }
        public CardView restcardview { get; private set; }



        public RestViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            foodtImage = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            restimage = itemView.FindViewById<CircleImageView>(Resource.Id.restimageView);
            restname = itemView.FindViewById<TextView>(Resource.Id.resttextView);
            restdistance = itemView.FindViewById<TextView>(Resource.Id.textView);
            restcardview = itemView.FindViewById<CardView>(Resource.Id.rest_cardview);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}