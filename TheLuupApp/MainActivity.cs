using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Java.Lang;
using Android.Views;
using Android.Widget;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp
{
    [Activity(Label = "@string/app_name", Theme ="@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        public static List<LFreinds> mFreindsLoad;
        public static List<GroupChat> myChats;
        public static List<LFreinds> mFreinds;

        public static List<Users> musers;
        public static List<Users> mNewusers;


        public static List<LRequest> mRequests;
        public static List<int> ReqIDs = new List<int>();
        public static List<string> ReqSenderStatus = new List<string>();
        public static List<Restuarent> mRestuarent;
        public static List<string> mRegions = new List<string>();

        public static List<CartCode> mCartCodes;
        private List<string> duplicate = new List<string>();

        public List<LFreinds> muserFreinds;
        public List<int> mf_id;
        public List<int> mf_idDuplicate;

        public static List<LuppAdvertisemnet> mAdvertisemnets;
        public static List<LuppAdvertisemnet> mAdvertisemnetsTop;
        public static List<LuppAdvertisemnet> mAdvertisemnetsOther;

        // public static List<CartCode> mcartCodes;
        //public List<int> mMyFreinds.;

        public static List<Menues> mMenues;
        public static List<Category> mCategories;
        public static List<CatMenu> mCatMenues;
        public static List<List<Menues>> mAZrrayOfArrays;
        public static List<List<LChats>> mAllChats;
        public static List<List<GroupChat>> mallSingleGrpupChats;

        // public static List<List<LChats>> mAllChats;

        private Home mHome;
        private Menu mMenu;
        private Advertisement mAdvertisement;
        private UserChats FuserChats;
        private Profile Fprofile;

        private TextView uname;
        private static Service1 client1 = new Service1();
        private Handler mHandler = new Handler();

        private string user_id;
        private static int id;

        private Android.Support.V7.Widget.Toolbar toolbar;
        private BottomNavigationView bottomNavigation;

        private Runnable uiThread;
        private Thread checkThread;
        public  ProgressDialog mFacultyDialog;
        private Handler handler;

        //SharePreferences
        private ISharedPreferences prefs;
        private ISharedPreferencesEditor editor;
        private static string LUUP_USER_PRIVATE_USERID_DETAILS_PERSONAL = "LUUP_USER_PRIVATE_USERID_DETAILS_PERSONAL";

        public  bool isMyMenueLoaded = false;
        public bool isChatsLoaded = false;
        public bool isMyProfileContactsLoaded = false;
        public bool isAdvertisementLoaded = false;       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);          
            SetSupportActionBar(toolbar);
            
           
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            if(Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop){
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.baseline_sort_24);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);


            prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            editor = prefs.Edit();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            mHome = new Home();
            mMenu = new Menu();
            mCatMenues = new List<CatMenu>();
            mAdvertisement = new Advertisement();
            FuserChats = new UserChats();
            Fprofile = new Profile();

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            user_id = prefs.GetString(LUUP_USER_PRIVATE_USERID_DETAILS_PERSONAL, null);
            if (user_id != null)
            {
                // Load the first fragment on creation
                // LoadFragment(Resource.Id.menu_home);

                // Prepare the data source:
                mRestuarent = new List<Restuarent>();

                //user chats data 
                mFreinds = new List<LFreinds>();
                mFreindsLoad = new List<LFreinds>();
                myChats = new List<GroupChat>();

                // profile or freids
                musers = new List<Users>();
                mNewusers = new List<Users>();

                //request
                mRequests = new List<LRequest>();

                //My Menu
                mCartCodes = new List<CartCode>();

                //menu
                mMenues = new List<Menues>();
                mCategories = new List<Category>();
                mAZrrayOfArrays = new List<List<Menues>>();

                muserFreinds = new List<LFreinds>();
                mf_id = new List<int>();
                mf_idDuplicate = new List<int>();

                //Adverts
                mAdvertisemnets = new List<LuppAdvertisemnet>();
                mAdvertisemnetsTop = new List<LuppAdvertisemnet>();
                mAdvertisemnetsOther = new List<LuppAdvertisemnet>();

                mAllChats = new List<List<LChats>>();
                mallSingleGrpupChats = new List<List<GroupChat>>();

                //my menu
                // mcartCodes = new List<CartCode>();

                Bundle bundle = new Bundle();
                bundle.PutString("user_id", user_id);

                mMenu.Arguments = bundle;
                mHome.Arguments = bundle;
                FuserChats.Arguments = bundle;
                Fprofile.Arguments = bundle;

                View headerView = navigationView.GetHeaderView(0);
                uname = headerView.FindViewById<TextView>(Resource.Id.textView);
                id = Int32.Parse(user_id);

                Users u = new Users();
                u = client1.getUser(id, true);

                uname.Text = u.Name + " " + u.Surname;
             
                SupportActionBar.Title = "Restaurants";

                mFacultyDialog = new ProgressDialog(this);
                mFacultyDialog.SetMessage("Loading data...");
                mFacultyDialog.Indeterminate = true;
                mFacultyDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                mFacultyDialog.SetCancelable(false);
                mFacultyDialog.Show();
                // restuarants

                
                Action action = () =>
                {
                    LoadAllUSerData(id);
                   // LoadMyMenue();
                   // LoadMyUserChats();
                   // LoadMyProfileRequest();
                  //  LoadAdvertisements();
                };

                handler = new MyHandler(this);
                handler.Post(action);


                /* uiThread = new Runnable(new Action(delegate {
                     while (true)
                     {
                         LoadAllUSerData2(id);
                         System.Threading.Thread.Sleep(1000);

                     }
                 }));
                 checkThread = new Thread(uiThread);
                 checkThread.Start();*/

            }
            else
            {
                Finish();
                Intent log = new Intent(this, typeof(Login));
                StartActivity(log);
            }

        }

        public void LoadTheRest()
        {
            uiThread = new Runnable(new Action(delegate {
                while (true)
                {
                    LoadMyMenue();
                    LoadMyUserChats();
                    LoadMyProfileRequest();
                    LoadAdvertisements();
                    System.Threading.Thread.Sleep(1000);

                }
            }));
            checkThread = new Thread(uiThread);
            checkThread.Start();
            
        }

        public void ShowFragment()
        {
            setFragment(mHome);
        }


        public static void updateRequest()
        {
            //frind Request
            List<LRequest> mynewrequest = new List<LRequest>();
            List<int> myreqid = new List<int>(); List<string> mysenderreqid = new List<string>();
            mynewrequest.AddRange(client1.getRequests(id, true));
            mRequests = mynewrequest;
            foreach (LRequest res in mRequests)
            {
                myreqid.Add(res.Ur_id);
                mysenderreqid.Add(res.Lr_status);
            }
            ReqIDs = myreqid;
            ReqSenderStatus = mysenderreqid;
           
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.bnav_restaurant:
                    setFragment(mHome);
                   /// toolbar.Visibility = ViewStates.Visible;
                    SupportActionBar.Title = "Region";
                    break;
                case Resource.Id.bnav_menu:
                    setFragment(mMenu); 
                   // toolbar.Visibility = ViewStates.Visible;
                    SupportActionBar.Title = "Menu";                    
                    break;
                case Resource.Id.bnav_chats:
                    setFragment(FuserChats);
                    SupportActionBar.Title = "Chats";
                   // toolbar.Visibility = ViewStates.Gone;
                    break;
                case Resource.Id.bnav_Contacts:
                    setFragment(Fprofile);
                    SupportActionBar.Title = "Contacts";
                    //toolbar.Visibility = ViewStates.Gone;
                    break;
                case Resource.Id.bnav_Ads:
                    setFragment(mAdvertisement);
                    //toolbar.Visibility = ViewStates.Visible;
                    SupportActionBar.Title = "Explore TheLuup"; 
                    break;
              
            }

        }

        private async void LoadMyMenue()
        {
            Message msg = new Message();
            msg = handler.ObtainMessage();
            List<CartCode> mycode = new List<CartCode>();
            //My Menu
            mycode.AddRange(client1.getCartCode(id, true));
            mCartCodes = mycode;

            msg.Arg1 = 1;
            handler.SendMessage(msg);
        }

        private async void LoadMyProfileRequest()
        {
            Message msg = new Message();
            msg = handler.ObtainMessage();

            // profile or freids
            List<LFreinds> freinds = new List<LFreinds>();
            List<Users> mynewuser = new List<Users>();
            List<LRequest> myrequests = new List<LRequest>();
            freinds.AddRange(client1.getFreinds(id, true));
            muserFreinds = freinds;
            foreach (LFreinds mf in muserFreinds)
            {

                if (mf_id.Contains(mf.Uf_id))
                {

                }
                else
                {
                    mf_id.Add(mf.Uf_id);
                }
            }
            foreach (Users rs in musers)
            {

                if (rs.Type == "Customer")
                {
                    if (rs.U_id.Equals(id))
                    {

                    }
                    else
                    {
                        if (mf_id.Contains(rs.U_id))
                        {

                        }
                        else
                        {
                            mynewuser.Add(rs);
                        }
                    }

                }
            }
            mNewusers = mynewuser;

            //frind Request
            myrequests.AddRange(client1.getRequests(id, true));
            mRequests = myrequests;
            List<int> myreqid = new List<int>();
            List<string> mysenderreqid = new List<string>();
            foreach (LRequest res in mRequests)
            {
                myreqid.Add(res.Ur_id);
                mysenderreqid.Add(res.Lr_status);
            }
            ReqIDs = myreqid;
            ReqSenderStatus = mysenderreqid;
           
            msg.Arg1 = 3;
            handler.SendMessage(msg);
        }

        private async void LoadAdvertisements()
        {
            Message msg = new Message();
            msg = handler.ObtainMessage();

            List<LuppAdvertisemnet> advertisements = new List<LuppAdvertisemnet>();
            List<LuppAdvertisemnet> adtop = new List<LuppAdvertisemnet>();
            List<LuppAdvertisemnet> adother = new List<LuppAdvertisemnet>();
            advertisements.AddRange(client1.GetLuppAdvertisemnets());
            mAdvertisemnets = advertisements;
            foreach (LuppAdvertisemnet advert in mAdvertisemnets)
            {
                int rid = advert.R_id;
                Management man = client1.getManagement(rid, true);
                int u_id = man.U_id;
                AssignAdsPriority a = client1.GetPriorityRange(u_id, true);
                int cad = a.Cad_id;
                AdsPriorityRange apr = client1.GetUserAdsPriorityRange(cad, true);

                if (apr.PriorityTier == 1)
                {
                    adtop.Add(advert);
                    mAdvertisemnetsTop = adtop;
                }
                else
                {
                    adother.Add(advert);
                    mAdvertisemnetsOther = adother;
                }
            }


            msg.Arg1 = 4;
            handler.SendMessage(msg);
        }

        private async void LoadMyUserChats()
        {
            Message msg = new Message();
            msg = handler.ObtainMessage();
            List<LFreinds> tempFreinds = new List<LFreinds>();
            List<GroupChat> tempmyChats = new List<GroupChat>();
            List<List<LChats>> temAllChats = new List<List<LChats>>();
            List<List<GroupChat>> temAllSingleGroup = new List<List<GroupChat>>();
            List<LFreinds> tempFreindsLoad = new List<LFreinds>();
            List<GroupChat> tempmyChatsForduplicates = new List<GroupChat>();
            List<string> GroupChatRepeat = new List<string>();

            tempFreinds.AddRange(client1.getFreinds(id, true));
            tempmyChats.AddRange(client1.getGroup(id, true));

            mFreinds = tempFreinds;

            foreach (GroupChat group in tempmyChats)
            {
                string groupchat = group.Gcode;
                if (GroupChatRepeat.Contains(groupchat))
                {

                }
                else
                {
                    GroupChatRepeat.Add(groupchat);
                    tempmyChatsForduplicates.Add(group);
                }

            }

            myChats = tempmyChatsForduplicates;

            foreach (LFreinds f in mFreinds)
            {
                int fid = f.Uf_id;
                string chatcode = f.Chatcode;
;
                    List<LChats> chats = new List<LChats>();
                    chats.AddRange(client1.getChat(chatcode));
                    temAllChats.Add(chats);

                    if (chats.Count > 0)
                    {
                        tempFreindsLoad.Add(f);
                    }

            }

            foreach (GroupChat group in myChats)
            {
                string groupchat = group.Gcode;
                if (groupchat != null)
                {
                    List<GroupChat> gpchat = new List<GroupChat>();
                    gpchat.AddRange(client1.getSingleGroup(groupchat));
                    temAllSingleGroup.Add(gpchat);

                    List<LChats> chats = new List<LChats>();
                    chats.AddRange(client1.getChat(groupchat));
                    temAllChats.Add(chats);
                }
            }

            mFreindsLoad = tempFreindsLoad;
            mAllChats = temAllChats;
            mallSingleGrpupChats = temAllSingleGroup;
            msg.Arg1 = 2;
            handler.SendMessage(msg);
        }

        private async void LoadAllUSerData(int id)
        {
                Message msg = new Message();
                msg = handler.ObtainMessage();

                List<string> duplicateregion = new List<string>();
                
                mRestuarent.AddRange(client1.getResturants());
                musers.AddRange(client1.getCustomerUsers());

                mCategories.AddRange(client1.getCategories());
                mCatMenues.AddRange(client1.getCatMenus());
                mMenues.AddRange(client1.getMaxMenue());

                foreach(Restuarent res in mRestuarent)
                {
                     string region = res.Region;
                     if (duplicateregion.Contains(region))
                     {
                        
                     }else
                     {
                        duplicateregion.Add(region);
                        mRegions.Add(region);
                     }
                }

                msg.Arg1 = 0;
                handler.SendMessage(msg);
        }


        public static void UpdateUserChats()
        {
            //UserChats
            List<LFreinds> tempFreinds = new List<LFreinds>();
            List<GroupChat> tempmyChats = new List<GroupChat>();
            List<List<LChats>> temAllChats = new List<List<LChats>>();
            List<List<GroupChat>> temAllSingleGroup = new List<List<GroupChat>>();
            List<LFreinds> tempFreindsLoad = new List<LFreinds>();
            List<GroupChat> tempmyChatsForduplicates = new List<GroupChat>();
            List<string> GroupChatRepeat = new List<string>();

            tempFreinds.AddRange(client1.getFreinds(id, true));
            tempmyChats.AddRange(client1.getGroup(id, true));

            mFreinds = tempFreinds;

            foreach (GroupChat group in tempmyChats)
            {
                string groupchat = group.Gcode;
                if (GroupChatRepeat.Contains(groupchat))
                {

                }
                else
                {
                    GroupChatRepeat.Add(groupchat);
                    tempmyChatsForduplicates.Add(group);
                }

            }

            myChats = tempmyChatsForduplicates;

            foreach (LFreinds f in mFreinds)
            {
                int fid = f.Uf_id;
                string chatcode = f.Chatcode;

                List<LChats> chats = new List<LChats>();
                chats.AddRange(client1.getChat(chatcode));
                temAllChats.Add(chats);

                if (chats.Count > 0)
                {
                    tempFreindsLoad.Add(f);
                }

            }

            mFreindsLoad = tempFreindsLoad;

            foreach (GroupChat group in myChats)
            {
                string groupchat = group.Gcode;
                if (groupchat != null)
                {
                    List<GroupChat> gpchat = new List<GroupChat>();
                    gpchat.AddRange(client1.getSingleGroup(groupchat));
                    temAllSingleGroup.Add(gpchat);

                    List<LChats> chats = new List<LChats>();
                    chats.AddRange(client1.getChat(groupchat));
                    temAllChats.Add(chats);
                }
            }
   
            mAllChats = temAllChats;
            mallSingleGrpupChats = temAllSingleGroup;
        }

        public static void UpdateMyMenue()
        {
            //UserChats
            //My Menu
            List<CartCode> temparray = new List<CartCode>();
            temparray.AddRange(client1.getCartCode(id, true));
            mCartCodes = temparray;
        }
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        private void setFragment(Fragment fragment)
        {
            //Clear Frament Back
         //   ClearFragmentBackStack();

            // Create a new fragment and a transaction.
            FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();

            // Replace the fragment that is in the View fragment_container (if applicable).
            fragmentTx.Replace(Resource.Id.admin_content, fragment);

            // Add the transaction to the back stack.
           // fragmentTx.AddToBackStack(null);

            // Commit the transaction.
            fragmentTx.Commit();

        }

        public void ClearFragmentBackStack()
        {
            FragmentManager fm = this.FragmentManager;
            for (int i = 0; i < fm.BackStackEntryCount; ++i)
            {
                fm.PopBackStack();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                editor.Clear();
                editor.Commit(); // commit changes
                Finish();
                Intent log = new Intent(this, typeof(Login));
                StartActivity(log);
            }

            return base.OnOptionsItemSelected(item);
        }


        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_settings)
            {
                // Handle the camera action
                SupportActionBar.Title = "Settings";

            }
          
         
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

    }

    public class MyHandler : Handler
    {
        private MainActivity mainActivity;

        public MyHandler(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public override void HandleMessage(Message msg)
        {
            switch (msg.Arg1)
            {
                case 0:
                    //true
                    mainActivity.ShowFragment();
                    mainActivity.mFacultyDialog.Dismiss();
                    mainActivity.LoadTheRest();
                    break;
                case 1:
                    //true
                    mainActivity.isMyMenueLoaded = true;
                    break;
                case 2:
                    //true
                    mainActivity.isChatsLoaded = true;
                    break;
                case 3:
                    //true
                    mainActivity.isMyProfileContactsLoaded = true;
                    break;
                case 4:
                    //true
                    mainActivity.isAdvertisementLoaded = true;
                    break;
                default:
                    break;
            }
            base.HandleMessage(msg);
        }
    }
}

