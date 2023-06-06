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
using TheLuupApp.theluuprefrence1;
using Android.Graphics;

namespace TheLuupApp.adapters
{
    class UserChatsGroupAdapter : RecyclerView.Adapter
    {
        private List<GroupChat> mGroupChat;
        private Context mContext;
        private Service1 client1 = new Service1();
        public event EventHandler<int> ItemClick;
        // private Activity activity;



        public UserChatsGroupAdapter(Context Context,List<GroupChat> groupChat)
        {
            mGroupChat = groupChat;
            mContext = Context;
        }

        public override int ItemCount
        {
            get { return mGroupChat.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            UserChatsGroupViewHolder vh = holder as UserChatsGroupViewHolder;
              //GroupChat g = mGroupChat[position];
              vh.Name.Text = mGroupChat[position].Gname.ToString();
              vh.status.Text = "Active";

            Random r = new Random();
            int red = r.Next(255 - 0 + 1) + 0;
            int green = r.Next(255 - 0 + 1) + 0;
            int blue = r.Next(255 - 0 + 1) + 0;
            vh.card.SetCardBackgroundColor(Color.Rgb(red, green, blue));
            string name = mGroupChat[position].Gname;
            string firstword = name.Substring(0, 1);
            vh.carttittle.Text = firstword;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                  Inflate(Resource.Layout.usergroupchat_listitem, parent, false);
            UserChatsGroupViewHolder vh = new UserChatsGroupViewHolder(itemView, OnClick);
            return vh;
        }


        public void updateViews(List<GroupChat> mcahts)
        {
            // updateRequest();
            mGroupChat = mcahts;
            NotifyDataSetChanged();
        }

        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }

    }

    public class UserChatsGroupViewHolder : RecyclerView.ViewHolder
    {
        public CardView card { get; private set; }
        public TextView carttittle { get; private set; }
        public TextView Name { get; private set; }
        public TextView status { get; private set; }


        public UserChatsGroupViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            card = itemView.FindViewById<CardView>(Resource.Id.chats_cardviews_chats);
            carttittle = itemView.FindViewById<TextView>(Resource.Id.carttiiles);
            Name = itemView.FindViewById<TextView>(Resource.Id.gUlikerName);
            status = itemView.FindViewById<TextView>(Resource.Id.gUstatus);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}