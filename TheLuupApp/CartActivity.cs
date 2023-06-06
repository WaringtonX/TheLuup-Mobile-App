using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TheLuupApp.adapters;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp
{
    [Activity(Label = "CartActivity", Theme="@style/AppTheme.NoActionBar", NoHistory = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class CartActivity : Activity
    {
        private TextView total;
        private static Service1 client1 = new Service1();
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private CartAdapter chartAdaper;

        private List<Cart> mCarts;
        private string user_id;
        private string cartcode;
        private int uid;
        private Decimal totals;
        public int removeedItemPosition = 0;
        List<Decimal> mTotdecimals;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.cart_layout);
            // Create your application here
            total = FindViewById<TextView>(Resource.Id.totalamount);

            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }

            mCarts = new List<Cart>();
            mTotdecimals = new List<Decimal>();
            user_id = Intent.GetStringExtra("user_id");
            cartcode = Intent.GetStringExtra("user_cartcode");
            uid = Int32.Parse(user_id);

            LoadData();
          //  total.Text = "R" + totals;

            chartAdaper = new CartAdapter(this, mCarts, mTotdecimals, total, cartcode);


            // Get our RecyclerView layout:
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.cartrecyclerView);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            // Plug the adapter into the RecyclerView:
            mRecyclerView.SetAdapter(chartAdaper);
        }

        public void LoadData()
        {
            mCarts.AddRange(client1.getCart(cartcode));
            
            foreach (Cart c in mCarts)
            {
                Menues m = client1.getMenue(c.Muid, true);
                Decimal temp = m.Price * c.Unit;
                mTotdecimals.Add(temp);
                totals += temp;
            } 

        }

          
    }
}