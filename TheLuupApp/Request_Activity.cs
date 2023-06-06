using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TheLuupApp.adapters;
using TheLuupApp.theluuprefrence1;
using Android.Graphics;

namespace TheLuupApp
{
    [Activity(Label = "Request_Activity", Theme = "@style/AppTheme.NoActionBar", NoHistory = true)]
    public class Request_Activity : AppCompatActivity
    {
       
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RequestAdapter requestAdapter;
        private Service1 client1 = new Service1();
        private string UID;
        private int userid;
        private Android.Support.V7.Widget.Toolbar toolbar;
        private List<string> SenderStatus = new List<string>();
        private static List<LRequest> lRequests;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RequestFragment);
            // Create your application here           
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarRequest);
            SetSupportActionBar(toolbar);
           //--------- SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.baseline_people_24);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = "Luup Request";


            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }


            UID = Intent.GetStringExtra("user_id");
            userid = Int32.Parse(UID);

            lRequests = new List<LRequest>();
            LoadData();

            requestAdapter = new RequestAdapter(this, lRequests, UID);

            // Get our RecyclerView layout:
            mRecyclerView =FindViewById<RecyclerView>(Resource.Id.recyclerViewRequest);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            // Plug the adapter into the RecyclerView:
            mRecyclerView.SetAdapter(requestAdapter);

           // requestAdapter.NotifyDataSetChanged();
           
        }

        private void LoadData()
        { 
            List<LRequest> requests = new List<LRequest>();
            List<LRequest> tempreq = new List<LRequest>();
            requests.AddRange(client1.getRequests(userid, true));
            foreach (LRequest res in requests)
            {
                if (res.Lr_status.Equals("Reciever"))
                {
                    tempreq.Add(res);
                }
            }
            lRequests = tempreq;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }


        protected override void OnResume()
        {
            base.OnResume();

            List<LRequest> rquests = new List<LRequest>();
            List<LRequest> tempreq = new List<LRequest>();
            tempreq.AddRange(client1.getRequests(userid, true));
            foreach (LRequest res in tempreq)
            {
                if (res.Lr_status.Equals("Reciever"))
                {
                    rquests.Add(res);
                }
            }

            if (rquests.Count > lRequests.Count)
            {
                requestAdapter.updateViews(rquests);
            }

        }
    }
}