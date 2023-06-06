
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util;
using TheLuupApp.adapters;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    public class UserChats : Fragment
    {
        private FloatingActionButton floatActionButton;
        private FloatingActionButton floatActionButtonCreateGroup;
        private Button btnchats, btngroups;
        private string user_id;
        private Service1 client1 = new Service1();
        private RecyclerView mRecyclerView, mRecyclerViewGroup;
        private RecyclerView.LayoutManager mLayoutManager;
        private UserChatsAdapter userChatsAdapter;
        private UserChatsGroupAdapter userGroupAdapter;
        private string UID;
        private bool isloaded = false;
        public  List<GroupChat> mClassChats;
        public  List<LFreinds> mClassFreinds;
        public  List<LFreinds> mClassFreindsLoad;
        private int userid;
        private MainActivity mainActivity = new MainActivity();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.userchats_layout, container, false);

            floatActionButton = view.FindViewById<FloatingActionButton>(Resource.Id.fabFreinds);
            floatActionButtonCreateGroup = view.FindViewById<FloatingActionButton>(Resource.Id.fabcreateGroup);
            btnchats = view.FindViewById<Button>(Resource.Id.btnchatss);
            btngroups = view.FindViewById<Button>(Resource.Id.btngroups);

            Bundle bundle = Arguments;
            if (bundle != null)
            {
                UID = bundle.GetString("user_id");
            }
            //
            //  LoadData();

            userid = Int32.Parse(UID);
            mClassFreindsLoad = new List<LFreinds>();
            mClassFreinds = new List<LFreinds>();
            mClassChats = new List<GroupChat>();

            mClassChats = myChats;
            mClassFreindsLoad = mFreindsLoad;

            userChatsAdapter = new UserChatsAdapter(Context, mClassFreindsLoad, UID);
            userGroupAdapter = new UserChatsGroupAdapter(Context, mClassChats);
            // Get our RecyclerView layout:
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewUserChats);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            // Plug the adapter into the RecyclerView:
            userChatsAdapter.ItemClick += userChatsAdapter_ItemClick;
            mRecyclerView.SetAdapter(userChatsAdapter);

            // Get our RecyclerView layout:
            mRecyclerViewGroup = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewUserGroupChats);
            mRecyclerViewGroup.SetLayoutManager(new LinearLayoutManager(Context));
            // Plug the adapter into the RecyclerView:
            userGroupAdapter.ItemClick += userGroupAdapter_ItemClick;
            mRecyclerViewGroup.SetAdapter(userGroupAdapter);



            floatActionButton.Click += (sender, e) =>
            {
                Intent log = new Intent(Context, typeof(LuupFreinds));
                log.PutExtra("user_id", UID);
                StartActivity(log);
            };

            floatActionButtonCreateGroup.Click += (sender, e) =>
            {
                Intent log = new Intent(Context, typeof(group_activity));
                log.PutExtra("user_id", UID);
                StartActivity(log);
            };

            btnchats.Text = "Chats " + "(" + mClassFreindsLoad.Count + ")";
            btngroups.Text = "Groups " + "(" + mClassChats.Count + ")";
            floatActionButtonCreateGroup.Visibility = ViewStates.Gone;
            floatActionButton.Visibility = ViewStates.Visible;
            btnchats.Click += (sender, e) =>
            {
                btnchats.SetBackgroundResource(Resource.Drawable.buttonshape2);
                btnchats.SetTextColor(Color.White);
                btngroups.SetBackgroundResource(Resource.Drawable.buttonshape4);
                btngroups.SetTextColor(Color.Gray);
                mRecyclerViewGroup.Visibility = ViewStates.Gone;
                mRecyclerView.Visibility = ViewStates.Visible;
                floatActionButtonCreateGroup.Visibility = ViewStates.Gone;
                floatActionButton.Visibility = ViewStates.Visible;
            };


            btngroups.Click += (sender, e) =>
            {
                btngroups.SetBackgroundResource(Resource.Drawable.buttonshape2);
                btngroups.SetTextColor(Color.White);
                btnchats.SetBackgroundResource(Resource.Drawable.buttonshape4);
                btnchats.SetTextColor(Color.Gray);
                mRecyclerViewGroup.Visibility = ViewStates.Visible;
                mRecyclerView.Visibility = ViewStates.Gone;
                floatActionButtonCreateGroup.Visibility = ViewStates.Visible;
                floatActionButton.Visibility = ViewStates.Gone;
            }; 

            

            return view;
        }

        private void userGroupAdapter_ItemClick(object sender, int e)
        {
            int position = e;
            GroupChat g = mClassChats[position];

            var intent = new Intent(Activity, typeof(groupChat));
            intent.PutExtra("Type", "Group");
            intent.PutExtra("muser_id", UID);
            intent.PutExtra("chatcode", g.Gcode);
            Activity.StartActivity(intent);
        }

        private void userChatsAdapter_ItemClick(object sender, int e)
        {
                   int position = e;

                   Users u = new Users();
                   foreach (Users us in musers)
                   {
                      if (mClassFreindsLoad[position].Uf_id.Equals(us.U_id))
                      {
                          u = us;
                      }
                   }

                    var intent = new Intent(Activity, typeof(Chat));
                    intent.PutExtra("Type", "OneToOne");
                    intent.PutExtra("user_id", u.U_id.ToString());
                    intent.PutExtra("muser_id", UID);
                    Activity.StartActivity(intent);

        }

        public override void OnResume()
        {
            base.OnResume();
            mClassChats = myChats;
            mClassFreindsLoad = mFreindsLoad;
            userChatsAdapter.updateViews(mClassFreindsLoad);
            userGroupAdapter.updateViews(mClassChats);
            btnchats.Text = "Chats " + "(" + mClassFreindsLoad.Count + ")";
            btngroups.Text = "Groups " + "(" + mClassChats.Count + ")";
            if (mainActivity.isChatsLoaded)
            {
    
             //   userChatsAdapter.NotifyDataSetChanged();
                /*if (mClassChats.Count.Equals(myChats.Count) && mClassFreindsLoad.Count.Equals(mFreindsLoad.Count))
                {

                }
                else
                {
                    mClassChats = myChats;
                    mClassFreindsLoad = mFreindsLoad;
                    userChatsAdapter.NotifyDataSetChanged();
                }*/

            }
                    
        }
       
    }
}