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
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TheLuupApp.adapters;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp
{
    [Activity(Label = "SelectMenu", NoHistory = true)]
    public class SelectMenu : Activity
    {
        private static RecyclerView mRecyclerView;
        private static RecyclerView.LayoutManager mLayoutManager;
        private Service1 client1 = new Service1();
        public static List<CartCode> mcartCodes;
        private SelectMenuAdaoter selectMenuAdaoter;
        private string user_id;
        private string res_id;
        private string from;
        private string menuid;
        private string unit;
        private string groupdCode;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_selectmenu);
            // Create your application here

            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }

            user_id = Intent.GetStringExtra("user_id");
            from = Intent.GetStringExtra("from");
            menuid = Intent.GetStringExtra("menuid");
            unit = Intent.GetStringExtra("unit");
            res_id = Intent.GetStringExtra("resid");
            groupdCode = Intent.GetStringExtra("groupdCode");

            mcartCodes = new List<CartCode>();

            LoadData();


            selectMenuAdaoter = new SelectMenuAdaoter(this, mcartCodes,from,menuid,unit,groupdCode, res_id);

            // Get our RecyclerView layout:
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewMySelect);
            mRecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            // Plug the adapter into the RecyclerView:
            selectMenuAdaoter.ItemClick += selectMenuAdaoter_ItemClick;
            mRecyclerView.SetAdapter(selectMenuAdaoter);
        }

        private void selectMenuAdaoter_ItemClick(object sender, int e)
        {
            int position = e;
            string u_id = mcartCodes[position].Uid.ToString();
            string cartcode = mcartCodes[position].Cartcode;

            if (from.Equals("AddMenu"))
            {
                int cres_id = Int32.Parse(res_id);
                int cunit = Int32.Parse(unit);
                int menu_id = Int32.Parse(menuid);
                int user_id = Int32.Parse(u_id);
                DateTime date = DateTime.UtcNow;

                if (client1.AddCart(cartcode, user_id, true, menu_id, true, cunit, true) == "Cart Added")
                {
                    if (client1.AddCountData(user_id, true, cres_id, true, menu_id, true, 1, true, date, true) == "CountData Added")
                    {
                        Toast.MakeText(this, "Added to Cart!", ToastLength.Short).Show();
                        this.Finish();

                    }

                }
            }
            else if (from.Equals("CharSend"))
            {
                int user_id = Int32.Parse(u_id);
                DateTime date = DateTime.UtcNow;
                if (client1.AddChat(groupdCode, user_id, true, user_id, true, cartcode, "Attachment", date,true) == "Chat Added") {
                    Toast.MakeText(this, "Attached!", ToastLength.Short).Show();
                    this.Finish();
                }
            }

        }

        private void LoadData()
        {
            int uid = Int32.Parse(user_id);
            mcartCodes.AddRange(client1.getCartCode(uid, true));
            Toast.MakeText(this, groupdCode + " = Size !", ToastLength.Short).Show();
        }
    }
}