using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Square.Picasso;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    [Activity(Label = "menu_Item_details", Theme = "@style/TransTheme.NoActionBar", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class Menu_Item_details : AppCompatActivity
    {
        private string user_id;
        private string res_id;
        private string menuid;
        private Service1 client1 = new Service1();
        private Menues cMenues = new Menues();
        private ImageView menucover;
        private TextView toolbarname;
        private TextView productDetaiils;
        private TextView productname;
        private TextView producPrice;
        private EditText AQuantity;
        private Android.Support.Design.Widget.FloatingActionButton floatingActionButton;

        private Android.Support.V7.Widget.Toolbar mToolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu_tem_details_layout);
        
            //ToolBar
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbars);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "";
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.baseline_keyboard_backspace_24);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }

            toolbarname = FindViewById<TextView>(Resource.Id.product_details_title);
            menucover = FindViewById<ImageView>(Resource.Id.backdrop);
            productname = FindViewById<TextView>(Resource.Id.foodnames);
            productDetaiils = FindViewById<TextView>(Resource.Id.foodDescription);
            producPrice = FindViewById<TextView>(Resource.Id.foodprices);
            AQuantity = FindViewById<EditText>(Resource.Id.AQuantity_text);
            floatingActionButton = FindViewById<Android.Support.Design.Widget.FloatingActionButton>(Resource.Id.fabadd);
            DateTime date = DateTime.Now;

            user_id = Intent.GetStringExtra("user_id");
            menuid = Intent.GetStringExtra("menuid");
            res_id = Intent.GetStringExtra("resid");
            int mu_id = Int32.Parse(menuid);
            int r_Id = Int32.Parse(res_id);
            int us_id = Int32.Parse(user_id);
            //cMenues = client1.getMenue(mu_id, true);

            foreach (Menues menues in mMenues)
            {
                if (mu_id.Equals(menues.Mu_id))
                {
                    cMenues = menues;
                }
            }

            AQuantity.Text = "1";

            if (menuid != null)
            {

                string foodimage = "http://theluup.co.za/images/foodimages/" + cMenues.Foodimage;
                productname.Text = cMenues.Foodname;
                toolbarname.Text = cMenues.Foodname;

                Picasso.With(this)
                      .Load(foodimage)
                      .Fit()
                      .Into(menucover);

                productDetaiils.Text = cMenues.Fooddescription;
                producPrice.Text ="" + cMenues.Price;

            }

            if (client1.AddViewsData(us_id, true, r_Id, true, mu_id, true, 1, true, date,true) == "ViewsData Added")
            {
                
            }

            floatingActionButton.Click += (sender, e) =>
            {
                string unit = AQuantity.Text;

                Intent log = new Intent(this, typeof(SelectMenu));
                log.PutExtra("user_id", user_id);
                log.PutExtra("from", "AddMenu");
                log.PutExtra("menuid", menuid);
                log.PutExtra("unit", unit);
                log.PutExtra("resid", res_id);
                StartActivity(log);
            };

            // Create your application here
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

    }
}