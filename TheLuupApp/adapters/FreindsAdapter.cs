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
using static TheLuupApp.MainActivity;

namespace TheLuupApp.adapters
{
    class FreindsAdapter : RecyclerView.Adapter
    {
        private List<LFreinds> mFFreinds;
        private Context mContext;
        private Service1 client1 = new Service1();
        private string mUserId;
        private Activity activity;
        public event EventHandler<int> ItemClick;
        public FreindsAdapter(Context Context, List<LFreinds> freinds, string UserId, Activity activity)
        {
            mFFreinds = freinds;
            mContext = Context;
            mUserId = UserId;
            this.activity = activity;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            FreindViewHolder vh = holder as FreindViewHolder;
            int uf_id= mFFreinds[position].Uf_id;
            Users u = new Users();
            foreach (Users us in musers)
            {
                if (uf_id.Equals(us.U_id))
                {
                    u = us;
                }
            }

            vh.Name.Text = u.Username;
            vh.status.Text = u.Name.ToString() + " " + u.Surname.ToString();
            //  string UserPhotoLink = "http://luuptempwebsite-001-site1.dtempurl.com/images/userimage/" + Musers[position].Image;

            Random r = new Random();
            int red = r.Next(255 - 0 + 1) + 0;
            int green = r.Next(255 - 0 + 1) + 0;
            int blue = r.Next(255 - 0 + 1) + 0;
            vh.card.SetCardBackgroundColor(Color.Rgb(red, green, blue));
            string name = u.Name;
            string firstword = name.Substring(0, 1);
            vh.carttittle.Text = firstword;

            vh.addfreind.Visibility = ViewStates.Gone;
            vh.cancelfreind.Visibility = ViewStates.Gone;
          
        }

        public override int ItemCount
        {
            get { return mFFreinds.Count; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                  Inflate(Resource.Layout.people_listitem, parent, false);
            FreindViewHolder vh = new FreindViewHolder(itemView, OnClick);
            return vh;
        }

        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }

        public class FreindViewHolder : RecyclerView.ViewHolder
        {
            public CardView card { get; private set; }
            public TextView carttittle { get; private set; }
            public CircleImageView restimage { get; private set; }
            public Button addfreind { get; private set; }
            public Button cancelfreind { get; private set; }
            public TextView Name { get; private set; }
            public TextView status { get; private set; }


            public FreindViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                // Locate and cache view references:
                card = itemView.FindViewById<CardView>(Resource.Id.car_people);
                carttittle = itemView.FindViewById<TextView>(Resource.Id.carpeople);
                Name = itemView.FindViewById<TextView>(Resource.Id.likerName);
                status = itemView.FindViewById<TextView>(Resource.Id.status);
                addfreind = itemView.FindViewById<Button>(Resource.Id.addFreind);
                cancelfreind = itemView.FindViewById<Button>(Resource.Id.CancelFreind);
                itemView.Click += (sender, e) => listener(base.Position);
            }
        }
    }
}