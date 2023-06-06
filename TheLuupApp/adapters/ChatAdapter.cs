using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp.adapters
{
    class ChatAdapter : RecyclerView.Adapter
    {
        private List<LChats> mChats;
        private Context mContext;
        private Service1 client1 = new Service1();
        private string mUserId;
        private Activity activity;
        private string mType;

        public ChatAdapter(Context Context, List<LChats> chats, string UserId, Activity activity, string Type)
        {
            mChats = chats;
            mContext = Context;
            mUserId = UserId;
            mType = Type;
            this.activity = activity;
        }

        public override int ItemCount
        {
            get { return mChats.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
             ChatsViewHolder vh = holder as ChatsViewHolder;

            if(mType == "OneToOne")
            {
                string uid = mChats[position].U_id.ToString();
                if (uid.Equals(mUserId))
                {
                    //vh.chatText.SetBackgroundResource(Resource.Drawable.chat_bubble_right);
                    /* RelativeLayout.LayoutParams layoutParams = (RelativeLayout.LayoutParams) vh.chatText.LayoutParameters;
                     layoutParams.AddRule(LayoutRules.AlignParentRight);
                     vh.chatText.LayoutParameters = layoutParams; */
                    DateTime date = mChats[position].Datetime;
                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-ZA");
                    DateTime d = Convert.ToDateTime(date.ToString(), enUS);
                    vh.chatText.Visibility = ViewStates.Invisible;
                    vh.chatTexts.Visibility = ViewStates.Visible;
                    vh.rtime.Visibility = ViewStates.Visible;
                    vh.ltime.Visibility = ViewStates.Invisible;
                    vh.chatTexts.Text = mChats[position].C_messagee;
                    vh.rtime.Text = "" + d.ToShortTimeString();
                }
                else
                {

                    DateTime date = mChats[position].Datetime;
                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-ZA");
                    DateTime d = Convert.ToDateTime(date.ToString(), enUS);
                    vh.chatTexts.Visibility = ViewStates.Invisible;
                    vh.chatText.Visibility = ViewStates.Visible;
                    vh.rtime.Visibility = ViewStates.Invisible;
                    vh.ltime.Visibility = ViewStates.Visible;
                    vh.chatText.Text = mChats[position].C_messagee;
                    vh.ltime.Text = "" + d.ToShortTimeString();
                }
            }
          
           
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.chat_listitem, parent, false);
            ChatsViewHolder vh = new ChatsViewHolder(itemView);
            return vh;
        }

        public void updateChat(List<LChats> newCaht)
        {
            mChats = newCaht;
            NotifyDataSetChanged();
        }
    }

    public class ChatsViewHolder : RecyclerView.ViewHolder
    {
        public TextView chatText { get; private set; }
        public TextView chatTexts { get; private set; }
        public TextView rtime { get; private set; }
        public TextView ltime { get; private set; }

        public ChatsViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            chatText = itemView.FindViewById<TextView>(Resource.Id.chatText);
            chatTexts = itemView.FindViewById<TextView>(Resource.Id.chatTexts);
            rtime = itemView.FindViewById<TextView>(Resource.Id.right);
            ltime = itemView.FindViewById<TextView>(Resource.Id.tleft);
        }
    }
}