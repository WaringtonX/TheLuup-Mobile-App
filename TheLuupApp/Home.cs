using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class Home : Fragment
    {
        private static RecyclerView mRecyclerView;
        private static RecyclerView.LayoutManager mLayoutManager;
        private static RetuarantRegionAdapter retuarantRegionAdapter;
        private Service1 client1 = new Service1();
        private int arrayLenng = 0;
        private string userid;
        private bool isDataLoadted = false;
        ProgressBar loadingSpinner;
        private List<string> mregionslist;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState); 

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

             View view = inflater.Inflate(Resource.Layout.region_layout, container, false);

            Bundle bundle = Arguments;
            if (bundle != null)
            {
                userid = bundle.GetString("user_id");
            }

            mregionslist = new List<string>();
            mregionslist = mRegions;

            retuarantRegionAdapter = new RetuarantRegionAdapter(Activity, mregionslist);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.regionrecyclerView);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            // Plug the adapter into the RecyclerView:
            retuarantRegionAdapter.ItemClick += retuarantRegionAdapterr_ItemClick;
            mRecyclerView.SetAdapter(retuarantRegionAdapter);
            return view;

        }
    
        private void retuarantRegionAdapterr_ItemClick(object sender, int e)
        {
            int position = e;
            Intent log = new Intent(Activity, typeof(RestuarantList));
            log.PutExtra("user_id", userid);
            log.PutExtra("regioName",mregionslist[position]);
            Activity.StartActivity(log);
        }

    }

}