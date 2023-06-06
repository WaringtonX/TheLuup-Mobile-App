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
    class PeopeAdapter : RecyclerView.Adapter
    {
        private List<Users> Musers;
        private List<int> mReqID;
        private List<string> mReqStatus;
        private Context mContext;
        private Service1 client1 = new Service1();
        private string mUserId;
        public event EventHandler<int> ItemClick;
        public event EventHandler<int> ItemCancel;
        public PeopeAdapter(Context Context, List<Users> users,List<int> ReqID, List<string> ReqStatus,string UserId)
        {
            Musers = users;
            mContext = Context;
            mUserId = UserId;
            mReqID = ReqID;
            mReqStatus = ReqStatus;
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PeopleViewHolder vh = holder as PeopleViewHolder;
            vh.Name.Text = Musers[position].Username;
            vh.status.Text =  Musers[position].Name + " " + Musers[position].Surname.ToString();
            int uid = Musers[position].U_id;

            if (mReqID.Contains(uid))
            {
                vh.addfreind.Visibility = ViewStates.Gone;
                vh.cancelfreind.Visibility = ViewStates.Visible;
            }
            else
            {
                vh.addfreind.Visibility = ViewStates.Visible;
                vh.cancelfreind.Visibility = ViewStates.Gone;
            }

            Random r = new Random();
            int red = r.Next(255 - 0 + 1) + 0;
            int green = r.Next(255 - 0 + 1) + 0;
            int blue = r.Next(255 - 0 + 1) + 0;
            vh.card.SetCardBackgroundColor(Color.Rgb(red, green, blue));
            string name = Musers[position].Name;
            string firstword = name.Substring(0, 1);
            vh.carttittle.Text = firstword;
        }

        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }

        private void OnCancel(int obj)
        {
            if (ItemCancel != null)
                ItemCancel(this, obj);
        }

        public override int ItemCount
        {
            get { return Musers.Count; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.people_listitem, parent, false);
            PeopleViewHolder vh = new PeopleViewHolder(itemView, OnClick, OnCancel);
            return vh;
        }


        public void updateViews(List<Users> muse,List<int> reqid, List<string> ReqStatus)
        {
            // updateRequest();
            Musers = muse;
            mReqID = reqid;
            mReqStatus = ReqStatus;
            NotifyDataSetChanged();
        }
    }

    public class PeopleViewHolder : RecyclerView.ViewHolder
    {
        public CardView card { get; private set; }
        public TextView carttittle { get; private set; }
        public Button addfreind { get; private set; }
        public Button cancelfreind { get; private set; }
        public TextView Name { get; private set; }
        public TextView status { get; private set; }


        public PeopleViewHolder(View itemView, Action<int> listener, Action<int> listener2) : base(itemView)
        {
            // Locate and cache view references:
            card = itemView.FindViewById<CardView>(Resource.Id.car_people);
            carttittle = itemView.FindViewById<TextView>(Resource.Id.carpeople);
            Name = itemView.FindViewById<TextView>(Resource.Id.likerName);
            status = itemView.FindViewById<TextView>(Resource.Id.status);
            addfreind = itemView.FindViewById<Button>(Resource.Id.addFreind);
            cancelfreind = itemView.FindViewById<Button>(Resource.Id.CancelFreind);
            addfreind.Click += (sender, e) => listener(base.Position);
            cancelfreind.Click += (sender, e) => listener2(base.Position);
        }
    }
}