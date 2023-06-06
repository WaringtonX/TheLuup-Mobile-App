using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TheLuupApp.adapters;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    [Activity(Label = "RestuarantList", Theme = "@style/AppTheme.NoActionBar", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class RestuarantList : AppCompatActivity
    {
        private static RecyclerView mRecyclerView;
        private static RestAdapter restAdapter;
        private Service1 client1 = new Service1();
        private int arrayLenng = 0;
        private string userid;
        private string regionName;
        private bool isDataLoadted = false;
        ProgressBar loadingSpinner;
        private List<Restuarent> restuarents;
        private List<Restuarent> tempretuarent;
        private Android.Support.V7.Widget.Toolbar mToolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HomeFregment);
            // Create your application here
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.restuar_toobar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Restuarants";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }
            userid = Intent.GetStringExtra("user_id");
            regionName = Intent.GetStringExtra("regioName");

            restuarents = new List<Restuarent>();
            tempretuarent = new List<Restuarent>();
            foreach (Restuarent res in mRestuarent)
            {
                if (res.Region.Equals(regionName))
                {
                    tempretuarent.Add(res);
                }
            }
            restuarents = tempretuarent;

            //// Prepare the data source:
            //   mRestuarent = new List<Restuarent>();
            loadingSpinner = FindViewById<ProgressBar>(Resource.Id.progressBar);

            // Instantiate the adapter and pass in its data source
            restAdapter = new RestAdapter(this, restuarents, userid);

            // Get our RecyclerView layout:
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            // Plug the adapter into the RecyclerView:
            restAdapter.ItemClick += restAdapter_ItemClick;
            mRecyclerView.SetAdapter(restAdapter);
            loadingSpinner.Visibility = ViewStates.Gone;
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

        private void restAdapter_ItemClick(object sender, int e)
        {
            int photoNum = e;
            Intent log = new Intent(this, typeof(MenuActivity));
            log.PutExtra("user_id", userid);
            log.PutExtra("res_id", restuarents[photoNum].R_id.ToString());
            StartActivity(log);
        }
    }
}