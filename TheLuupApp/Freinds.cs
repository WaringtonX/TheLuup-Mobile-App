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
using TheLuupApp.theluuprefrence;

namespace TheLuupApp
{
    public class Freinds : Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private List<LFreinds> mFreinds;
        private FreindsAdapter freindsAdapter;
        private Service1 client1 = new Service1();
        private string UID;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.FreindsFregmen, container, false);

            mFreinds = new List<LFreinds>();

            Bundle bundle = Arguments;
            if (bundle != null)
            {
                UID = bundle.GetString("user_id");
            }

            LoadData();

            freindsAdapter = new FreindsAdapter(Context, mFreinds, UID);


            // Get our RecyclerView layout:
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewFreinds);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            // Plug the adapter into the RecyclerView:
            mRecyclerView.SetAdapter(freindsAdapter);

            freindsAdapter.NotifyDataSetChanged();
            return view;
        }

        private void LoadData()
        {
            int userid = Int32.Parse(UID);
            mFreinds.AddRange(client1.getFreinds(userid, true));
        }
    }
}