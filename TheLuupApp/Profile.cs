using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using TheLuupApp.adapters;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    public class Profile : Fragment
    {
        private RecyclerView mRecyclerView;
        private EditText Input_Search;
        private RecyclerView.LayoutManager mLayoutManager;
        private PeopeAdapter peopeAdapter;
        private Service1 client1 = new Service1();
        private string UID;
        private static List<Users> myusers;
        private static List<Users> MAxmyusers;
        public static List<LFreinds> mcurrentuser;
        private int userid;
        public List<int> mf_id;
        public List<string> mf_status;
        private FloatingActionButton floatActionButton;
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
            View view = inflater.Inflate(Resource.Layout.ProfileFregmen, container, false);
            floatActionButton = view.FindViewById<FloatingActionButton>(Resource.Id.fabRequest);
            Input_Search = view.FindViewById<EditText>(Resource.Id.inputSearch);
            Bundle bundle = Arguments;
            if (bundle != null)
            {
                UID = bundle.GetString("user_id");
            }

            myusers = new List<Users>();
            mf_id = new List<int>();
            mf_status = new List<string>();
            myusers = mNewusers;
            mf_id = ReqIDs;
            mf_status = ReqSenderStatus;
            userid = Int32.Parse(UID);

            peopeAdapter = new PeopeAdapter(Activity, myusers, mf_id, mf_status, UID);


            // Get our RecyclerView layout:
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewpeople);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            // Plug the adapter into the RecyclerView:

            peopeAdapter.ItemClick += peopeAdapter_ItemClick;
            peopeAdapter.ItemCancel += peopeAdapter_ItemCancel;
            mRecyclerView.SetAdapter(peopeAdapter);

           // peopeAdapter.NotifyDataSetChanged();

            floatActionButton.Click += (sender, e) =>
            {
                Intent log = new Intent(Context, typeof(Request_Activity));
                log.PutExtra("user_id", UID);
                StartActivity(log);
            };

            Input_Search.TextChanged += Input_Search_TextChanged;
            return view;
        }

        private void peopeAdapter_ItemCancel(object sender, int e)
        {
            int position = e;
            int r_id = myusers[position].U_id;
            if (client1.DeleteRequest(userid, true, r_id, true) == "Success")
            {
                if (client1.DeleteRequest(r_id, true,userid, true) == "Success")
                {
                    updateRequest();
                    mf_id = ReqIDs;
                    mf_status = ReqSenderStatus;
                    peopeAdapter.updateViews(myusers, mf_id,mf_status);
                    Toast.MakeText(Context, "Luup Request Canceld!", ToastLength.Short).Show();
                }
            }
        }

        private void peopeAdapter_ItemClick(object sender, int e)
        {
            int position = e;

            if (client1.AddRequest(myusers[position].U_id, true, userid, true, "Reciever") == "Request Added")
                {
                    if (client1.AddRequest(userid, true, myusers[position].U_id, true, "Sender") == "Request Added")
                    {
                        Toast.MakeText(Context, "Luup Request Sent to " + myusers[position].Name.ToString(), ToastLength.Short).Show();
                        //  vh.addfreind.Visibility = ViewStates.Invisible;
                        updateRequest();
                        mf_id = ReqIDs;
                        mf_status = ReqSenderStatus;
                        peopeAdapter.updateViews(myusers, mf_id,mf_status);
                    }
                }
        }

        private void Input_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            //get the text from Edit Text            
            //var searchText = Input_Search.Text;
            string searchText = Input_Search.Text.ToLower();
            List<Users> searchusers = new List<Users>();
            foreach (Users use in myusers)
            {
                if (use.Username.ToLower().Contains(searchText))
                {
                    searchusers.Add(use);
                }
            }
            myusers = searchusers; ;
            peopeAdapter.updateViews(myusers, mf_id,mf_status);

        }

        public override void OnResume()
        {
            base.OnResume();
            if (mainActivity.isMyProfileContactsLoaded)
            {
              
                if (myusers.Count.Equals(mNewusers.Count))
                {

                }
                else
                {
                    myusers = mNewusers;
                    peopeAdapter.updateViews(myusers, mf_id,mf_status);
                }
            }
        }

        
    }
}