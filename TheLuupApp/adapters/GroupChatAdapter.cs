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
using Android.Views.Animations;
using Android.Widget;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp.adapters
{
    class GroupChatAdapter : RecyclerView.Adapter
    {
        private List<LChats> mmChats;
        private Context mContext;
        private Service1 client1 = new Service1();
        private string mUserId;
        private Activity activity;
        private string mType;

        public GroupChatAdapter(Context Context, List<LChats> chats, string UserId, Activity activity, string Type)
        {
            mmChats = chats;
            mContext = Context;
            mUserId = UserId;
            mType = Type;
            this.activity = activity;
        }

        public override int ItemCount
        {
            get { return mmChats.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
           GroupViewHolder vh = holder as GroupViewHolder;

            if(mType == "Group")
            {
                int uid = mmChats[position].U_id;
                Users u = new Users();
                foreach (Users us in musers)
                {
                    if (uid.Equals(us.U_id))
                    {
                        u = us;
                    }
                }
                
                if (mmChats[position].C_type.Equals("Attachment"))
                {
                    vh.uname.Text = u.Name + " " + u.Surname;
                    vh.groupchatText.Visibility = ViewStates.Invisible;
                    vh.receiptImage.Visibility = ViewStates.Visible;
                    vh.imagetime.Visibility = ViewStates.Visible;
                    vh.ttime.Visibility = ViewStates.Gone;
                    DateTime date = mmChats[position].Datetime;
                    vh.imagetime.Text = date.ToShortTimeString();

                    vh.receiptImage.Click += (sender, e) =>
                    {
                      //  Animation animation = AnimationUtils.LoadAnimation(mContext, Resource.Animation.bounce_new);
                     //   vh.receiptImage.StartAnimation(animation);
                        Intent log = new Intent(mContext, typeof(CartActivity));
                        log.SetFlags(ActivityFlags.NewTask);
                        log.PutExtra("user_id", mmChats[position].U_id.ToString());
                        log.PutExtra("user_cartcode", mmChats[position].C_messagee);
                        mContext.StartActivity(log);
                    };
                }
                else
                {
                    vh.imagetime.Visibility = ViewStates.Gone;
                    vh.ttime.Visibility = ViewStates.Visible;
                    vh.groupchatText.Visibility = ViewStates.Visible;
                    vh.receiptImage.Visibility = ViewStates.Gone;
                    vh.uname.Text = u.Name + " " + u.Surname;
                    vh.groupchatText.Text = mmChats[position].C_messagee;
                    DateTime date = mmChats[position].Datetime;
                    vh.ttime.Text =  date.ToShortTimeString();
                }

            }

        }



        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.group_chat_listitem, parent, false);
            GroupViewHolder vh = new GroupViewHolder(itemView);
            return vh;
        }

        public void updateChat(List<LChats> newCaht)
        {
            mmChats = newCaht;
            NotifyDataSetChanged();
        }

    }

    public class GroupViewHolder : RecyclerView.ViewHolder
    {
        public TextView groupchatText { get; private set; }
        public TextView uname { get; private set; }
        public TextView ttime { get; private set; }
        public TextView imagetime { get; private set; }
        public ImageView receiptImage { get; private set; }

        public GroupViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            groupchatText = itemView.FindViewById<TextView>(Resource.Id.ggroupchatText);
            uname = itemView.FindViewById<TextView>(Resource.Id.gusername);
            imagetime = itemView.FindViewById<TextView>(Resource.Id.imagetime);
            receiptImage = itemView.FindViewById<ImageView>(Resource.Id.gchaticonReciep);
            ttime = itemView.FindViewById<TextView>(Resource.Id.gtime);
        }
    }
}