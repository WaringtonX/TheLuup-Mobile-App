using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TheLuupApp.MainActivity;
using Android.App;
using Android.Content;
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
    class AdvertAdapterHorizontal : RecyclerView.Adapter
    {
        private List<LuppAdvertisemnet> mAdvertisemnets;
        private Context mContext;
        private Service1 client1 = new Service1();

        public AdvertAdapterHorizontal(Context Context, List<LuppAdvertisemnet> advertisemnets)
        {
            mAdvertisemnets = advertisemnets;
            mContext = Context;
        }

        public override int ItemCount
        {
            get { return mAdvertisemnets.Count; }
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AdvertAdapterViewHolder vh = holder as AdvertAdapterViewHolder;

            //Restuarennt
            int r_id = mAdvertisemnets[position].R_id;
            Restuarent restuarent = new Restuarent();
            foreach (Restuarent res in mRestuarent)
            {
                if (r_id.Equals(res.R_id))
                {
                    restuarent = res;
                }
            }

            vh.adressname.Text = restuarent.Name;
            vh.addresemail.Text = restuarent.Slogan;
            string RestLink = "http://theluup.co.za/images/resimages/" + restuarent.Image;
            Picasso.With(mContext)
                        .Load(RestLink)
                        .Fit()
                        .Into(vh.adimage);

            //Avdvert
            vh.addrestittle.Text = mAdvertisemnets[position].Adtittle;
            vh.addresSlogan.Text = mAdvertisemnets[position].Slogan;
            vh.addresprice.Text = "R" + mAdvertisemnets[position].Newprice;
            string advertlink = "http://theluup.co.za/images/advertimages/" + mAdvertisemnets[position].Image;
            Picasso.With(mContext)
                        .Load(advertlink)
                        .Fit()
                        .Into(vh.adrest); 
        }

        public void updateViewsTop(List<LuppAdvertisemnet> advertise)
        {
            // updateRequest();
            mAdvertisemnets = new List<LuppAdvertisemnet>();
            mAdvertisemnets.AddRange(advertise);
            NotifyDataSetChanged();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.layout_advertHorizontal_listitem, parent, false);
            AdvertAdapterViewHolder vh = new AdvertAdapterViewHolder(itemView);
            return vh;
        }

        public class AdvertAdapterViewHolder : RecyclerView.ViewHolder
        {
            public ImageView  adrest { get; private set; }
            public CircleImageView adimage { get; private set; }
            public TextView adressname { get; private set; }
            public TextView addresemail { get; private set; }
            public TextView addrestittle { get; private set; }
            public TextView addresSlogan { get; private set; }
            public TextView addresprice { get; private set; }

            public AdvertAdapterViewHolder(View itemView) : base(itemView)
            {
                // Locate and cache view references:
                adimage = itemView.FindViewById<CircleImageView> (Resource.Id.hadresimage);
                adrest = itemView.FindViewById<ImageView>(Resource.Id.hadcertImageview);
                adressname = itemView.FindViewById<TextView>(Resource.Id.hadresname);
                addresemail = itemView.FindViewById<TextView>(Resource.Id.hadresstatus);
                addrestittle = itemView.FindViewById<TextView>(Resource.Id.hadtittle);
                addresSlogan = itemView.FindViewById<TextView>(Resource.Id.hadslogan);
                addresprice = itemView.FindViewById<TextView>(Resource.Id.haddprice);
            }
        }
    }

}