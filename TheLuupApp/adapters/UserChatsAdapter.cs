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
    class UserChatsAdapter : RecyclerView.Adapter
    {
        private List<LFreinds> mFreinds;
        private Context mContext;
        private Service1 client1 = new Service1();
        private string mUserId;
        private int gPosition =  0;
        public event EventHandler<int> ItemClick;
        // private Activity activity;



        public UserChatsAdapter(Context Context, List<LFreinds> freinds, string UserId)
        {
            mFreinds = freinds;
            mContext = Context;
            mUserId = UserId;
        }

        public override int ItemCount
        {
            get { return mFreinds.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            UserChatsViewHolder vh = holder as UserChatsViewHolder;

                Users u = new Users();
                foreach (Users us in musers)
                {
                    if (mFreinds[position].Uf_id.Equals(us.U_id))
                    {
                        u = us;
                        vh.Name.Text = u.Username.ToString();
                    }
                }

            Random r = new Random();
            int red = r.Next(255 - 0 + 1) + 0;
            int green = r.Next(255 - 0 + 1) + 0;
            int blue = r.Next(255 - 0 + 1) + 0;
            vh.card.SetCardBackgroundColor(Color.Rgb(red, green, blue));
            string name = u.Name;
            string firstword = name.Substring(0, 1);
            vh.carttittle.Text = firstword;
            //vh.status.Text = u.Email.ToString();            
        }

      
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                  Inflate(Resource.Layout.userchat_listitem, parent, false);
            UserChatsViewHolder vh = new UserChatsViewHolder(itemView, OnClick);
            return vh;
        }


        public void updateViews(List<LFreinds> myFreinds)
        {
            // updateRequest();
            mFreinds = myFreinds;
            NotifyDataSetChanged();
        }

        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }

    }

    public class UserChatsViewHolder : RecyclerView.ViewHolder
    {
        public CardView card { get; private set; }
        public TextView carttittle { get; private set; }
        public TextView Name { get; private set; }
        public TextView status { get; private set; }


        public UserChatsViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            card = itemView.FindViewById<CardView>(Resource.Id.chat_cardview_chat);
            carttittle = itemView.FindViewById<TextView>(Resource.Id.carttiile);
            Name = itemView.FindViewById<TextView>(Resource.Id.UlikerName);
            status = itemView.FindViewById<TextView>(Resource.Id.Ustatus);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}