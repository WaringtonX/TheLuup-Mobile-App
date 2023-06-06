using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using Refractored.Controls;
using Square.Picasso;
using TheLuupApp.theluuprefrence1;
using TheLuupApp.adapters;
using Android.Support.V4.View;
using Android.Util;
using com.refractored;
using static TheLuupApp.MainActivity;
using Android.Views;
using Android.Graphics;
//using com.refractored;

namespace TheLuupApp
{
    [Activity(Label = "MenuActivity", Theme = "@style/AppTheme.NoActionBar", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    public class MenuActivity : AppCompatActivity , IOnTabReselectedListener, ViewPager.IOnPageChangeListener
    {
        private RecyclerView mRecyclerView;                                                                                           
        private RecyclerView.LayoutManager mLayoutManager;
        private Android.Support.V7.Widget.Toolbar mToolbar;
        private string user_id;
        private string r_id;

        private TextView rename;
        private TextView redetails;
        private Service1 client1 = new Service1();
        private CircleImageView restimage;
        private ImageView  rescover;
        public Restuarent cRestuarent;
        private string recoverimage,resiconimage;
        private MenuAdapter menuAdapter;
        private List<Menues> cMenues;
        private ViewPager viewPager;
        private PagerSlidingTabStrip _tabs;
        private PageTabsAdapter pageTabsAdapter;
        private List<Category> Categories;
        private List<CatMenu> catMenus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menuactivity);
            // Create your application here

            //ToolBar
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.mtoolbar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Menu";
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.baseline_keyboard_backspace_24);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }
            rename = FindViewById<TextView>(Resource.Id.rsename);
            redetails = FindViewById<TextView>(Resource.Id.resdetails);
            restimage = FindViewById<CircleImageView>(Resource.Id.resimage);
            rescover = FindViewById<ImageView>(Resource.Id.rescover);
            // Prepare the data source:
            cRestuarent = new Restuarent();
            cMenues = new List<Menues>();
            Categories = new List<Category>();
            catMenus = new List<CatMenu>();

            user_id = Intent.GetStringExtra("user_id");
            r_id = Intent.GetStringExtra("res_id");
           

          
            if(r_id != null)
            {

                int rest_id = Int32.Parse(r_id);

                foreach (Restuarent res in mRestuarent)
                {
                    if (rest_id.Equals(res.R_id))
                    {
                        cRestuarent = res;
                    }
                }

                foreach (Category cat in mCategories)
                {
                    if (rest_id.Equals(cat.Res_id))
                    {
                        Categories.Add(cat);
                    }
                }


                rename.Text = cRestuarent.Name;
                redetails.Text = cRestuarent.Location;
                recoverimage = cRestuarent.Coverimage;
                resiconimage = cRestuarent.Image;

                string resc = "http://theluup.co.za/images/rescover/" + recoverimage;
                string resi = "http://theluup.co.za/images/resimages/" + resiconimage;

                Picasso.With(this)
                           .Load(resc)
                           .Fit()
                           .Into(rescover);

                Picasso.With(this)
                          .Load(resi)
                          .Fit()
                          .Into(restimage);
            }

          //  LoadData(r_id);

            viewPager = FindViewById<ViewPager>(Resource.Id.main_tabs_viewpager);
            _tabs = FindViewById<PagerSlidingTabStrip>(Resource.Id.main_tabs_header);
            pageTabsAdapter = new PageTabsAdapter(SupportFragmentManager, Categories);
            viewPager.Adapter = pageTabsAdapter;
            _tabs.SetViewPager(viewPager);

            int pageMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, Resources.DisplayMetrics);
            viewPager.PageMargin = pageMargin;
            viewPager.CurrentItem = 0;

            _tabs.OnTabReselectedListener = this;
            _tabs.OnPageChangeListener = this;
           // _tabs.SetOnPageChangeListener(this);


            if (viewPager.CurrentItem == 0)
            {
                  catMenus.Clear();
                  if(Categories.Count > 0)
                  {
                    int catid = Categories[0].Ct_id;
                    foreach (CatMenu cm in mCatMenues)
                    {
                        if (catid.Equals(cm.Ct_id))
                        {
                            catMenus.Add(cm);
                        }
                    }

                  
                    foreach (CatMenu cm in catMenus)
                    {
                        foreach (Menues menues in mMenues)
                        {
                            if (cm.Mu_id.Equals(menues.Mu_id))
                            {
                                cMenues.Add(menues);
                            }
                        }
                        
                    }

                    menuAdapter = new MenuAdapter(this, cMenues, user_id);
                    mRecyclerView = FindViewById<RecyclerView>(Resource.Id.menurecyclerView);
                    mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
                    mRecyclerView.SetAdapter(menuAdapter);
                }
                 

            }

            /* LoadData(r_id);
             if (Categories.Count >= 0)`
             {
                 //foreach (Category item in Categories)
                   // tabsAdapter.AddTab(new TitleFragment() { Title = item.Name, MyFragment = new MenuActivity(item.Name) });

             } */


            // menuAdapter = new MenuAdapter(this, mMenues, user_id);

            // Get our RecyclerView layout:
            // mRecyclerView = FindViewById<RecyclerView>(Resource.Id.menurecyclerView);
            // mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            // Plug the adapter into the RecyclerView:
            // mRecyclerView.SetAdapter(menuAdapter);


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

        private void LoadData(string id)
        {
            //int r = Int32.Parse(id);
          //  mMenues.AddRange(client1.getMenues(r, true));
            //Categories.AddRange(client1.getCategory(r,true));
        }

        public void OnPageScrollStateChanged(int state)
        {
            
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            
        }

        public void OnPageSelected(int position)
        {
            Toast.MakeText(this, "Tab reselected: " + position, ToastLength.Short).Show();
            catMenus.Clear();
            cMenues.Clear();

            if (Categories.Count > 0)
            {
                int catid = Categories[position].Ct_id;
                foreach (CatMenu cm in mCatMenues)
                {
                    if (catid.Equals(cm.Ct_id))
                    {
                        catMenus.Add(cm);
                    }
                }


                foreach (CatMenu cm in catMenus)
                {
                    foreach (Menues menues in mMenues)
                    {
                        if (cm.Mu_id.Equals(menues.Mu_id))
                        {
                            cMenues.Add(menues);
                        }
                    }

                }

                RunOnUiThread(() =>
                {
                    menuAdapter.updateMenu(cMenues);
                });
            }
        }

        public void OnTabReselected(int position)
        {
          
         
        }
    }
}