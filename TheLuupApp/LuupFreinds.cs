using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
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
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    [Activity(Label = "LuupFreinds", Theme = "@style/AppTheme.NoActionBar", NoHistory = true)]
    public class LuupFreinds : AppCompatActivity
    {
        private RecyclerView mRecyclerView;
        private List<LFreinds> mFrei;
        private FreindsAdapter freindsAdapter;
        private Service1 client1 = new Service1();
        private string UID;
        private Android.Support.V7.Widget.Toolbar mToolbar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FreindsFregmen);
            // Create your application here
            //ToolBar
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.friends_toobar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Friends";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }

            mFrei = new List<LFreinds>();

            
            UID = Intent.GetStringExtra("user_id");

            LoadData();

            freindsAdapter = new FreindsAdapter(ApplicationContext, mFrei, UID,this);


            // Get our RecyclerView layout:
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewFreinds);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            // Plug the adapter into the RecyclerView:
            freindsAdapter.ItemClick += freindsAdapter_ItemClick;
            mRecyclerView.SetAdapter(freindsAdapter);

            //freindsAdapter.NotifyDataSetChanged();
        }

        private void freindsAdapter_ItemClick(object sender, int e)
        {
            int position = e;

            string uf_id =""+ mFrei[position].Uf_id;
            Intent intent = new Intent(this, typeof(Chat));
            intent.PutExtra("Type", "OneToOne");
            intent.PutExtra("user_id", uf_id);
            intent.PutExtra("muser_id", UID);
           // intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
            Finish();
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

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }

        private void LoadData()
        {
            int userid = Int32.Parse(UID);
            mFrei = mFreinds;
        }
    }
}