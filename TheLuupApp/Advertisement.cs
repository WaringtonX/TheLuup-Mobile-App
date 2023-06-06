using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
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
    public class Advertisement : Fragment
    {
        private string userid;
        private static RecyclerView mRecyclerViewTop;
        private static RecyclerView mRecyclerViewOther;
        private Service1 client1 = new Service1();
        public List<LuppAdvertisemnet> AdvertisemnetsTop;
        public List<LuppAdvertisemnet> AdvertisemnetsOther;
        private AdvertAdapter otherAdapter;
        private AdvertAdapterHorizontal TopAdapter;
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
            View view = inflater.Inflate(Resource.Layout.layout_advertisment, container, false);

            //// Prepare the data source:
            AdvertisemnetsTop = new List<LuppAdvertisemnet>();
            AdvertisemnetsOther = new List<LuppAdvertisemnet>();

            Bundle bundle = Arguments;
            if (bundle != null)
            {
                userid = bundle.GetString("user_id");
            }

            // Instantiate the adapter and pass in its data source
           
            AdvertisemnetsTop = mAdvertisemnetsTop;
            AdvertisemnetsOther = mAdvertisemnetsOther;

            TopAdapter = new AdvertAdapterHorizontal(Context, AdvertisemnetsTop);
            otherAdapter = new AdvertAdapter(Context, AdvertisemnetsOther);

            // Get our RecyclerView layout: Top
            LinearLayoutManager layoutManager = new LinearLayoutManager(Context, RecyclerView.Horizontal, false);
            mRecyclerViewTop = view.FindViewById<RecyclerView>(Resource.Id.ToprecyclerView);
            mRecyclerViewTop.SetLayoutManager(layoutManager);
            // Plug the adapter into the RecyclerView:
            mRecyclerViewTop.SetAdapter(TopAdapter);

            // Get our RecyclerView layout: Other
            LinearLayoutManager layoutManager1 = new LinearLayoutManager(Context, RecyclerView.Vertical, false);
            mRecyclerViewOther = view.FindViewById<RecyclerView>(Resource.Id.OthererecyclerView);
            mRecyclerViewOther.SetLayoutManager(layoutManager1);
            // Plug the adapter into the RecyclerView:
            mRecyclerViewOther.NestedScrollingEnabled = false;
            mRecyclerViewOther.SetAdapter(otherAdapter);
            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            if (mainActivity.isAdvertisementLoaded)
            {
           
              
                if (AdvertisemnetsTop.Count.Equals(mAdvertisemnetsTop.Count))
                {

                }else
                {
                    AdvertisemnetsTop = mAdvertisemnetsTop;
                    TopAdapter.updateViewsTop(AdvertisemnetsTop);
                }

                if (AdvertisemnetsOther.Count.Equals(mAdvertisemnetsOther.Count))
                {

                }
                else
                {
                    AdvertisemnetsOther = mAdvertisemnetsOther;
                    otherAdapter.updateViewsNorm(AdvertisemnetsTop);

                }
              
            }
        }

    }
}