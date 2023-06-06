using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using TheLuupApp.adapters;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    public class Menu : Fragment
    {
        private string userid;
        private static RecyclerView mRecyclerView;
        private static RecyclerView.LayoutManager mLayoutManager;
        private Service1 client1 = new Service1();
        public  List<CartCode> mcartCodes;
        private MyMenuAdapter menuAdapter;
        private FloatingActionButton floatActionButton;
        private Runnable uiThread;
        private Thread checkThread;
        private int uid;
        private MainActivity mainActivity = new MainActivity();
        public ProgressDialog mFacultyDialog;
        private Handler handler;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
          
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.MenuFregmen, container, false);

            floatActionButton = view.FindViewById<FloatingActionButton>(Resource.Id.fabCreateMenu);
            //// Prepare the data source:
            mcartCodes = new List<CartCode>();
          
            Bundle bundle = Arguments;
            if (bundle != null)
            {
                userid = bundle.GetString("user_id");
                uid = Int32.Parse(userid);
            }

            mcartCodes = mCartCodes;
            menuAdapter = new MyMenuAdapter(Context, mcartCodes, userid);

            // Get our RecyclerView layout:
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewMyMenues);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            // Plug the adapter into the RecyclerView:
            menuAdapter.ItemClick += menuAdapter_ItemClick;
            mRecyclerView.SetAdapter(menuAdapter);

            floatActionButton.Click += (sender, e) =>
            {
                Intent log = new Intent(Context, typeof(CreateMenu));
                log.PutExtra("user_id", userid);
                StartActivity(log);
            };
           
            return view;
        }

        private void menuAdapter_ItemClick(object sender, int e)
        {
            int position = e;
            string cartcode = mcartCodes[position].Cartcode;
            int card_id = mcartCodes[position].Ccid;
            mFacultyDialog = new ProgressDialog(Activity);
            mFacultyDialog.SetMessage("Removing...");
            mFacultyDialog.Indeterminate = true;
            mFacultyDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            mFacultyDialog.SetCancelable(false);
            mFacultyDialog.Show();
            // restuarants


            Action action = () =>
            {

                DeleteMenItem(card_id, cartcode);
            };

            handler = new MyHandlerMenu(this);
            handler.Post(action);

        }


        public async void DeleteMenItem(int card_id, string cartcode)
        {
            Message msg = new Message();
            msg = handler.ObtainMessage();
            if (client1.DeleteCartAll(cartcode) == "Success")
            {
                if (client1.DeleteCartCode(card_id, true) == "Success")
                {
                    //Toast.MakeText(Activity, "Menu Successfully Deleted!", ToastLength.Short).Show();
                    UpdateMyMenue();
                    mcartCodes = mCartCodes;
                    menuAdapter.updateViews(mcartCodes);
                    msg.Arg1 = 0;
                    handler.SendMessage(msg);
                }
            }
        }

        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Toast.MakeText(Activity, "Back from Create Activity!", ToastLength.Short).Show();
        }
        public override void OnResume()
        {
            base.OnResume();
            mcartCodes = mCartCodes;
            menuAdapter.updateViews(mcartCodes);
           // menuAdapter.NotifyDataSetChanged();
            if (mainActivity.isMyMenueLoaded)
            {
              
                /* if (mcartCodes.Count.Equals(mCartCodes.Count))
                 {

                 }else
                 {

                 }*/

            }

        }    
    }


    public class MyHandlerMenu : Handler
    {
        private Menu menuFragment;

        public MyHandlerMenu(Menu menuFragment)
        {
            this.menuFragment = menuFragment;
        }

        public override void HandleMessage(Message msg)
        {
            switch (msg.Arg1)
            {
                case 0:
                    //true
                    menuFragment.mFacultyDialog.Dismiss();
                    break;     
                default:
                    break;
            }
            base.HandleMessage(msg);
        }
    }
}