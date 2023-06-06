using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using TheLuupApp.adapters;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    public class Request : Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RequestAdapter requestAdapter;
        private Service1 client1 = new Service1();
        private string UID;
        private int userid;
        private static List<LRequest> lRequests;
      
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.RequestFragment, container, false);
           
         
            Bundle bundle = Arguments;
            if (bundle != null)
            {
                UID = bundle.GetString("user_id");
            }
            // mRequests = new List<LRequest>();

           // LoadData();
            lRequests = new List<LRequest>();
            lRequests = mRequests;
            userid = Int32.Parse(UID);

            requestAdapter = new RequestAdapter(Context, lRequests, UID);

            // Get our RecyclerView layout:
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewRequest);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            // Plug the adapter into the RecyclerView:
            mRecyclerView.SetAdapter(requestAdapter);

            requestAdapter.NotifyDataSetChanged();
            return view;
        }

        /*  private void LoadData()
          {
              int uid = Int32.Parse(UID);
              mRequests.AddRange(client1.getRequests(uid, true));
          }*/

        public override void OnResume()
        {
           
            base.OnResume();
            lRequests = new List<LRequest>();
            lRequests.AddRange(client1.getRequests(userid, true));
            if(lRequests.Count  > mRequests.Count)
            {
                requestAdapter.updateViews(lRequests);
            }
           
        }
    }
    
   
}