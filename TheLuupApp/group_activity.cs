using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Refractored.Controls;
using Square.Picasso;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    [Activity(Label = "group_activity" ,Theme = "@style/AppTheme.NoActionBar", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class group_activity : AppCompatActivity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private List<LFreinds> mFreinds;
        private GroupFAdapter groupFAdapter;
        private Service1 client1 = new Service1();
        private string UID;
        private FloatingActionButton floatActionButton;
        private TextInputLayout textInputLayout;
        private TextInputEditText textInputEditText;
        private Android.Support.V7.Widget.Toolbar mToolbar;
        int count = 0;
        public static bool isSelected = false;
        public static List<int> UserID = new List<int>();
        private static Random random = new Random();
        public ProgressDialog mFacultyDialog;
        private Handler handler;
        private string name;
        private string chatCode;
        private int u_id;
        private CoordinatorLayout rootView;
        private int position = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.group_layout);
            rootView = FindViewById<CoordinatorLayout>(Resource.Id.snackviewgroup);
            // Create your application here

            mFreinds = new List<LFreinds>();
            textInputLayout = FindViewById<TextInputLayout>(Resource.Id.statusUpdate);
            textInputEditText = FindViewById<TextInputEditText>(Resource.Id.groupName);
            //ToolBar
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.group_toobar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Create Group";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }

            UID = Intent.GetStringExtra("user_id");
            u_id = Int32.Parse(UID);
            textInputLayout.Visibility = ViewStates.Invisible;
            if (count == 0)
            {
                LoadData();

                groupFAdapter = new GroupFAdapter(ApplicationContext, mFreinds, UID, this);

                // Get our RecyclerView layouta:
                mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewGroup);
                mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
                // Plug the adapter into the RecyclerView:
                mRecyclerView.SetAdapter(groupFAdapter);

                groupFAdapter.NotifyDataSetChanged();

            }

            
            floatActionButton = FindViewById<FloatingActionButton>(Resource.Id.fabLayouts);

            floatActionButton.Click += (sender,s) => {
                if (count == 1)
                {
                    name = textInputEditText.Text;
                    chatCode = RandomString();
                    if(!TextUtils.IsEmpty(textInputEditText.Text))
                    {
                        mFacultyDialog = new ProgressDialog(this);
                        mFacultyDialog.SetTitle("Creating group...");
                        mFacultyDialog.SetMessage("please wait...");
                        mFacultyDialog.Indeterminate = true;
                        mFacultyDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                        mFacultyDialog.SetCancelable(false);
                        mFacultyDialog.Show();

                        Action action = () =>
                        {
                            CreaetGroup();
                        };

                        handler = new MyHandlers(this);
                        handler.Post(action);

                    }
                   
                }

                if (isSelected)
                {
                    mRecyclerView.Visibility = ViewStates.Invisible;
                    textInputLayout.Visibility = ViewStates.Visible;
                    SupportActionBar.Title = "Group Name";
                    count = 1;
                    floatActionButton.SetImageResource(Resource.Drawable.baseline_add_24);
                }
            
            };
        }

        private async void CreaetGroup()
        {
                   Message msg = new Message();
                   msg = handler.ObtainMessage();

                    if (client1.AddGroup(chatCode, name, u_id, true) == "Group Added")
                    {
                    }

                    for (int i = 0; i < UserID.Count; i++)
                    {
                        if (client1.AddGroup(chatCode, name, UserID[i], true) == "Group Added")
                        {
                              
                          }
                    }

            UpdateUserChats();
            Toast.MakeText(this, "Group Successfully Created!", ToastLength.Short).Show();
            msg.Arg1 = 0;
            handler.SendMessage(msg);
        }

        public override void OnBackPressed()
        {
            textInputLayout.Visibility = ViewStates.Invisible;
            mRecyclerView.Visibility = ViewStates.Visible;
            SupportActionBar.Title = "Create Group";
            floatActionButton.SetImageResource(Resource.Drawable.baseline_east_24);
            count = 0;
            if(count == 0)
            {
                Finish();
            }
        }

        private void LoadData()
        {
            int userid = Int32.Parse(UID);
            mFreinds.AddRange(client1.getFreinds(userid, true));
        }


        private static string RandomString()
        {
            int length = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public class GroupFAdapter : RecyclerView.Adapter
        {
            private List<LFreinds> mFreinds;
            private Context mContext;
            private Service1 client1 = new Service1();
            private string mUserId;
            private Activity activity;

            public GroupFAdapter(Context Context, List<LFreinds> freinds, string UserId, Activity activity)
            {
                mFreinds = freinds;
                mContext = Context;
                mUserId = UserId;
                this.activity = activity;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                GroupFViewHolder vh = holder as GroupFViewHolder;
                Users u = client1.getUser(mFreinds[position].Uf_id, true);
                vh.Name.Text = u.Name.ToString() + " " + u.Surname.ToString();
                vh.status.Text = u.Email.ToString();
            

                vh.checkeds.Click += (sender, e) =>
                {
                    Toast.MakeText(mContext, u.Name.ToString(), ToastLength.Short).Show();
                    isSelected = true;
                    UserID.Add(mFreinds[position].Uf_id);
                };

                Random r = new Random();
                int red = r.Next(255 - 0 + 1) + 0;
                int green = r.Next(255 - 0 + 1) + 0;
                int blue = r.Next(255 - 0 + 1) + 0;
                vh.card.SetCardBackgroundColor(Color.Rgb(red, green, blue));
                string name = u.Name;
                string firstword = name.Substring(0, 1);
                vh.carttittle.Text = firstword;

                /* vh.addfreind.Click += (sender, e) =>
                   {
                       // Toast.MakeText(mContext, "User ID : " + u.U_id, ToastLength.Short).Show();
                       activity.Finish();
                       var intent = new Intent(activity, typeof(Chat));
                       intent.PutExtra("user_id", u.U_id.ToString());
                       intent.PutExtra("muser_id", mUserId);
                       intent.SetFlags(ActivityFlags.NewTask);
                       activity.StartActivity(intent);
                   }; */
            }

            public override int ItemCount
            {
                get { return mFreinds.Count; }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemView = LayoutInflater.From(parent.Context).
                      Inflate(Resource.Layout.group_listItem, parent, false);
                GroupFViewHolder vh = new GroupFViewHolder(itemView);
                return vh;
            }

        }


        public class GroupFViewHolder : RecyclerView.ViewHolder
        {
            public CardView card { get; private set; }
            public TextView carttittle { get; private set; }
            public CheckBox checkeds { get; private set; }
            public TextView Name { get; private set; }
            public TextView status { get; private set; }


            public GroupFViewHolder(View itemView) : base(itemView)
            {
                // Locate and cache view references:
                card = itemView.FindViewById<CardView>(Resource.Id.ggcard_request);
                carttittle = itemView.FindViewById<TextView>(Resource.Id.ggcardtittlereq);
                Name = itemView.FindViewById<TextView>(Resource.Id.likerName);
                status = itemView.FindViewById<TextView>(Resource.Id.status);
                checkeds = itemView.FindViewById<CheckBox>(Resource.Id.checkeed);
            }
        }

    }

    public class MyHandlers : Handler
    {
        private group_activity group_Activity;

        public MyHandlers(group_activity group_activity)
        {
            this.group_Activity = group_activity;
        }

        public override void HandleMessage(Message msg)
        {
            switch (msg.Arg1)
            {
                case 0:
                    //true
                    group_Activity.mFacultyDialog.Dismiss();
                    group_Activity.Finish();
                    break;
                default:
                    break;
            }
            base.HandleMessage(msg);
        }
    }
}