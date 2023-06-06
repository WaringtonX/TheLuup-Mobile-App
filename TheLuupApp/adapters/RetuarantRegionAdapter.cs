using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ColinDodd.GradientLayout;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp.adapters
{
    class RetuarantRegionAdapter : RecyclerView.Adapter
    {
        private List<string> mRegions;
        private string mUid;
        private Context mContext;
        public event EventHandler<int> ItemClick;

        public RetuarantRegionAdapter(Context context, List<string> Regions)
        {
            mRegions = Regions;
            mContext = context;
        }

        public override int ItemCount
        {
            get { return mRegions.Count; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.region_listitem, parent, false);
            RestViewHolder vh = new RestViewHolder(itemView, OnClick);
            return vh;
        }

        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RestViewHolder vh = holder as RestViewHolder;
            vh.regioname.Text = mRegions[position];
            List<Restuarent> tempretuarent = new List<Restuarent>();
            foreach (Restuarent res in mRestuarent)
            {
                if (res.Region.Equals(mRegions[position]))
                {
                    tempretuarent.Add(res);
                }
            }

            if(tempretuarent.Count > 1)
            {
                vh.regnumrestuarents.Text = "" + tempretuarent.Count + " Restuarants";
            }else
            {
                vh.regnumrestuarents.Text = "" + tempretuarent.Count + " Restuarant";
            }

            Random r = new Random();
            int red = r.Next(255 - 0 + 1) + 0;
            int green = r.Next(255 - 0 + 1) + 0;
            int blue = r.Next(255 - 0 + 1) + 0;

            Random r2 = new Random();
            int red2 = r.Next(255 - 0 + 1) + 0;
            int green2 = r.Next(255 - 0 + 1) + 0;
            int blue2 = r.Next(255 - 0 + 1) + 0;

            vh.gradlayout.SetStartColor(Color.Rgb(red, green, blue))
                    .SetEndColor(Color.Rgb(red2, green2, blue2))
                    .SetOrientation(GradientDrawable.Orientation.TrBl);
            //vh.restcardview.SetCardBackgroundColor(getRandomColorCode());
        }

       

        public int getRandomColorCode()
        {

            Random random = new Random();

            return Color.Argb(255, random.Next(256), random.Next(256), random.Next(256));

        }

        public class RestViewHolder : RecyclerView.ViewHolder
        {
            public TextView regioname { get; private set; }
            public TextView regnumrestuarents { get; private set; }
            public CardView restcardview { get; private set; }
            public GradientRelativeLayout gradlayout { get; private set; }


            public RestViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                // Locate and cache view references:
                regioname = itemView.FindViewById<TextView>(Resource.Id.regioname);
                regnumrestuarents = itemView.FindViewById<TextView>(Resource.Id.numrestinregion);
                restcardview = itemView.FindViewById<CardView>(Resource.Id.rest_cardview_region);
                gradlayout = itemView.FindViewById<GradientRelativeLayout>(Resource.Id.mygradientlayout);
                itemView.Click += (sender, e) => listener(base.Position);
            }
        }
    }
}