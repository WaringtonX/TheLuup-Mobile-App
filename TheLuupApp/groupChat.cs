using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using TheLuupApp.adapters;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    [Activity(Label = "groupChat", Theme = "@style/AppTheme.NoActionBar", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class groupChat : AppCompatActivity
    {
        private string user_id;
        private string muser_id;
        private TextView Name;
        private ImageButton attach;
        private TextView Name_sub;
        private Service1 client1 = new Service1();
        private EditText chatmessage;
        private FloatingActionButton sendMessasage;
        private List<LChats> mChats;
        private List<GroupChat> mgroupChats;
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private List<string> dupliccate = new List<string>();
        private GroupChatAdapter groupChatAdapter;
        private string groupnames;
        private string gch;
        private Android.Support.V7.Widget.Toolbar toolbar;
        public CardView card { get; private set; }
        public TextView carttittle { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_groupchat);
            // Create your application here

            Name = FindViewById<TextView>(Resource.Id.guser_tit);
            Name_sub = FindViewById<TextView>(Resource.Id.guser_sub);
            chatmessage = FindViewById<EditText>(Resource.Id.gtxtComment);
            sendMessasage = FindViewById<FloatingActionButton>(Resource.Id.gbtnComment);
            attach = FindViewById<ImageButton>(Resource.Id.gbtnComments);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBar3);
            SetSupportActionBar(toolbar);


            // Locate and cache view references:
            card = FindViewById<CardView>(Resource.Id.gcar_people);
            carttittle = FindViewById<TextView>(Resource.Id.gcarpeople);
            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }

            string type = Intent.GetStringExtra("Type");

            Toast.MakeText(this, "Chat Type : " + type, ToastLength.Short).Show();

            if (type == "Group")
            {
                muser_id = Intent.GetStringExtra("muser_id");
                gch = Intent.GetStringExtra("chatcode");
                int uids = Int32.Parse(muser_id);
                mChats = new List<LChats>();

                mgroupChats = new List<GroupChat>();
                foreach (List<GroupChat> gpass in mallSingleGrpupChats)
                {
                    foreach (GroupChat group in gpass)
                    {
                        if (group.Gcode.Equals(gch))
                        {
                            mgroupChats.Add(group);
                        }
                    }
                }

                string gn = mgroupChats[0].Gname;
                if (gch != null)
                {                  
                    Name.Text = gn;
                    for (int i = 0; i < mgroupChats.Count; i++)
                    {
                        int u_id = mgroupChats[i].Uid;
                        Users usrs = new Users();
                        foreach (Users us in musers)
                        {
                            if (u_id.Equals(us.U_id))
                            {
                                usrs = us;
                            }
                        }

                        if (i < mgroupChats.Count - 1)
                        {
                            string names = usrs.Name;
                            if (dupliccate.Contains(names))
                            {

                            }else
                            {
                                dupliccate.Add(names);
                                groupnames += names + " , ";
                            }
                            
                        }
                        else
                        {
                            string names = usrs.Name;
                            if (dupliccate.Contains(names))
                            {

                            }
                            else
                            {
                                dupliccate.Add(names);
                                groupnames += names;
                            }
                        }
                    }
                    Name_sub.Text = groupnames;
                }


                Random r = new Random();
                int red = r.Next(255 - 0 + 1) + 0;
                int green = r.Next(255 - 0 + 1) + 0;
                int blue = r.Next(255 - 0 + 1) + 0;
                card.SetCardBackgroundColor(Color.Rgb(red, green, blue));
                string name = gn;
                string firstword = name.Substring(0, 1);
                carttittle.Text = firstword;
                LoadDatas();

                groupChatAdapter = new GroupChatAdapter(ApplicationContext, mChats, muser_id, this, "Group");


                // Get our RecyclerView layout:
                mRecyclerView = FindViewById<RecyclerView>(Resource.Id.grecyclerViewChats);
                mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
                // Plug the adapter into the RecyclerView:
                mRecyclerView.SetAdapter(groupChatAdapter);
               
                //  mRecyclerView.SmoothScrollToPosition(mChats.Count - 1);

                var uiThread = new Java.Lang.Runnable(new Action(delegate {
                    while (true)
                    {
                        Console.WriteLine("Called back with state");
                        List<LChats> ct = new List<LChats>();
                        ct.AddRange(client1.getChat(gch));
                        if (ct.Count > mChats.Count)
                        {

                            Console.WriteLine("Current Size = " + mChats.Count + " New Size = " + ct.Count);
                            RunOnUiThread(() =>
                            {     
                                mChats = ct;
                                groupChatAdapter.updateChat(mChats);
                                if(mChats.Count > 1)
                                {
                                    mRecyclerView.SmoothScrollToPosition(mChats.Count - 1);
                                }
                                // mRecyclerView.SmoothScrollToPosition(mChats.Count - 1);
                            });
                        }
                        Thread.Sleep(500);

                    }
                }));
                var checkThread = new Thread(uiThread);
                checkThread.Start();


                sendMessasage.Click += (sender, e) =>
                {
                    string message = chatmessage.Text;
                    int uid = Int32.Parse(muser_id);
                    if (!TextUtils.IsEmpty(chatmessage.Text))
                    {
                       // DateTime date = DateTime.UtcNow;
                        DateTime now = DateTime.UtcNow.ToLocalTime();
                        if (client1.AddChat(gch, uids, true, uids, true, message, "message", now, true) == "Chat Added")
                        {
                            chatmessage.Text = "";
                        }
                    }

                };

                attach.Click += (sender, e) =>
                {

                    Intent log = new Intent(this, typeof(SelectMenu));
                    log.PutExtra("user_id", muser_id);
                    log.PutExtra("from", "CharSend");
                    log.PutExtra("groupdCode", gch);
                    StartActivity(log);

                };


            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.groupmenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_leavegroup)
            {
                int u_id = Int32.Parse(muser_id);
                if (client1.UserLeaveGroup(u_id, true,gch) == "Success")
                {
                    Toast.MakeText(this, "You Left The Group", ToastLength.Short).Show();
                    UpdateUserChats();
                    Finish();
                }                 
            }

            return base.OnOptionsItemSelected(item);
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }

        private void LoadDatas()
        {
            mChats.Clear();
            foreach (List<LChats> lChats in mAllChats)
            {
                foreach (LChats chats in lChats)
                {
                    if (chats.Chatcode.Equals(gch))
                    {
                        mChats.Add(chats);
                    }
                }
            }
            // mChats.AddRange(client1.getChat(gch));
        }

    }

}