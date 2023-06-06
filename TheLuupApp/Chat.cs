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
    [Activity(Label = "Chat", Theme = "@style/AppTheme.NoActionBar", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class Chat : Activity
    {
        private string user_id;
        private string muser_id;
        private string Chatcode;
        private TextView Name;
        private ImageButton attach;
        private TextView Name_sub;
        private Service1 client1 = new Service1();
        private EditText chatmessage;
        private FloatingActionButton sendMessasage;
        private List<LChats> mChats;
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private ChatAdapter chatAdapter;
        private Runnable uiThread;
        private Thread checkThread;

        public CardView card { get; private set; }
        public TextView carttittle { get; private set; }
        //private Java.Lang.Runnable uiThread;
        //private Java.Lang.Runnable drawThread;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_chat);
            // Create your application here

            Name = FindViewById<TextView>(Resource.Id.user_tit);
            Name_sub = FindViewById<TextView>(Resource.Id.user_sub);
            chatmessage = FindViewById<EditText>(Resource.Id.txtComment);
            sendMessasage = FindViewById<FloatingActionButton>(Resource.Id.btnComment);
            attach = FindViewById<ImageButton>(Resource.Id.btnComments);

            // Locate and cache view references:
            card = FindViewById<CardView>(Resource.Id.ccar_people);
            carttittle = FindViewById<TextView>(Resource.Id.ccarpeople);

            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }
          
            string type = Intent.GetStringExtra("Type");

           // Toast.MakeText(this, "Chat Type : " + type, ToastLength.Short).Show();

            if (type == "OneToOne")
            {
                user_id = Intent.GetStringExtra("user_id");
                muser_id = Intent.GetStringExtra("muser_id");


                mChats = new List<LChats>();
                //Toast.MakeText(this, "User ID : " + user_id, ToastLength.Short).Show();

                if ((user_id != null) && (muser_id != null))
                {
                    int uid = Int32.Parse(muser_id);
                    int ufid = Int32.Parse(user_id);
                    LFreinds ff = new LFreinds();
                   
                    foreach (LFreinds lFreinds in mFreinds)
                    {
                        if (uid.Equals(lFreinds.U_id) && ufid.Equals(lFreinds.Uf_id))
                        {
                            ff = lFreinds;
                        }
                    }
                    Chatcode = ff.Chatcode;
                }

                Users usrs = new Users();
                if (user_id != null)
                {
                    int uid = Int32.Parse(user_id);                  
                    foreach (Users us in musers)
                    {
                        if (uid.Equals(us.U_id))
                        {
                            usrs = us;
                        }
                    }
                    Name.Text = usrs.Name;
                }

                Random r = new Random();
                int red = r.Next(255 - 0 + 1) + 0;
                int green = r.Next(255 - 0 + 1) + 0;
                int blue = r.Next(255 - 0 + 1) + 0;
                card.SetCardBackgroundColor(Color.Rgb(red, green, blue));
                string name = usrs.Name;
                string firstword = name.Substring(0, 1);
                carttittle.Text = firstword;
                // Toast.MakeText(this, "Code = " + Chatcode+ " user_id : " + user_id + " uf_id : " + muser_id, ToastLength.Long).Show();
                LoadData();

                chatAdapter = new ChatAdapter(ApplicationContext, mChats, muser_id, this, "OneToOne");


                // Get our RecyclerView layout:
                mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewChats);
                mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
                // Plug the adapter into the RecyclerView:
                mRecyclerView.SetAdapter(chatAdapter);
                
            //mRecyclerView.SmoothScrollToPosition(mChats.Count - 1);
            /*  TimerCallback tmCallback = callback;
              Timer timer = new Timer(tmCallback, "test", 10000, 10000);
              Console.WriteLine("Press any key to exit the sample");
              Console.ReadLine(); */

            uiThread = new Runnable(new Action(delegate {
                    while (true)
                    {
                        Console.WriteLine("Called back with state");
                        List<LChats> ct = new List<LChats>();
                        ct.AddRange(client1.getChat(Chatcode));
                        if (ct.Count > mChats.Count)
                        {

                            Console.WriteLine("Current Size = " + mChats.Count + " New Size = " + ct.Count);
                            RunOnUiThread(() =>
                            {
                                mChats = ct;
                                chatAdapter.updateChat(mChats);
                                if (mChats.Count > 1)  
                                {
                                    mRecyclerView.SmoothScrollToPosition(mChats.Count - 1);
                                }
                                // mRecyclerView.SmoothScrollToPosition(mChats.Count - 1);
                            });
                        }
                        Thread.Sleep(500);

                    }
                }));
                checkThread = new Thread(uiThread);
                checkThread.Start();

                sendMessasage.Click += (sender, e) =>
                {
                    Toast.MakeText(this, "btnSendClicked", ToastLength.Short).Show();
                    string message = chatmessage.Text;
                    int uid = Int32.Parse(muser_id);

                    if (!TextUtils.IsEmpty(chatmessage.Text))
                    {
                        DateTime date = DateTime.UtcNow.ToLocalTime();
                        if (client1.AddChat(Chatcode, uid, true, uid, true, message, "message",date,true) == "Chat Added")
                        {
                            chatmessage.Text = "";
                            //LoadData();
                            //chatAdapter.updateChat(mChats);
                        }
                    }
                };

            }


        }

    


        public override void OnBackPressed()
        {
            base.OnBackPressed();
            // checkThread.Dispose();
            Finish();
        }

        private void LoadData()
        {
            mChats.Clear();

            foreach (List<LChats> lChats in mAllChats)
            {
                foreach (LChats chats in lChats)
                {
                    if (chats.Chatcode.Equals(Chatcode))
                    {
                        mChats.Add(chats);
                    }
                }
            }


        }

    }
}