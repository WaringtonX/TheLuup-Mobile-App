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
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Square.Picasso;
using TheLuupApp.theluuprefrence1;
using static Android.Resource;

namespace TheLuupApp.adapters
{
    class MenuAdapter : RecyclerView.Adapter
    {
        private List<Menues> mMenues;
        private Context mContext;
        private Service1 client1 = new Service1();
        private string mUser_ID;
        private Android.Views.Animations.Animation addmenu;

        public MenuAdapter(Context context,List<Menues> Menues,string User_ID)
        {
            mContext = context;
            mMenues = Menues;
            mUser_ID = User_ID;
            addmenu = AnimationUtils.LoadAnimation(mContext, Resource.Animation.bounce);
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MenuViewHolder vh = holder as MenuViewHolder;

            vh.foodname.Text = mMenues[position].Foodname.ToString();
            vh.foodPrice.Text = "R" + mMenues[position].Price;
            vh.foodimage.Visibility = ViewStates.Gone;
          //  string foodimage = "http://theluup.co.za/images/foodimages/" + mMenues[position].Foodimage;
          /* Picasso.With(mContext)
                       .Load(foodimage)
                       .Fit()
                       .Into(vh.foodimage);

           /*  vh.addcard.Click += (sender, e) =>
               {
                   string unit = vh.fpoodunit.Text;
                   string muiid = mMenues[position].Mu_id.ToString();
                   string resuiid = mMenues[position].R_id.ToString();



                   Intent log = new Intent(mContext, typeof(menu_Item_details));
                   log.PutExtra("user_id", mUser_ID);
                   log.PutExtra("menuid", muiid);
                   log.PutExtra("resid", resuiid);
                   mContext.StartActivity(log);

                   Intent log = new Intent(mContext, typeof(SelectMenu));
                   log.PutExtra("user_id", mUser_ID);
                   log.PutExtra("from", "AddMenu");
                   log.PutExtra("menuid", muiid);
                   log.PutExtra("unit", unit);
                   log.PutExtra("resid", resuiid);
                   mContext.StartActivity(log);

               }; */

            vh.ItemView.Click += (sender, e) =>
            {
                string muiid = mMenues[position].Mu_id.ToString();
                string resuiid = mMenues[position].R_id.ToString();

              //  Android.Views.Animations.Animation animation = AnimationUtils.LoadAnimation(mContext, Resource.Animation.bounce_new);
               // vh.ItemView.StartAnimation(animation);
                Intent log = new Intent(mContext, typeof(Menu_Item_details));
                log.PutExtra("user_id", mUser_ID);
                log.PutExtra("menuid", muiid);
                log.PutExtra("resid", resuiid);
                mContext.StartActivity(log);
            };
        }

        public override int ItemCount
        {
            get { return mMenues.Count; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.menu_listicons, parent, false);
            MenuViewHolder vh = new MenuViewHolder(itemView);
            return vh;
        }

        public void updateMenu(List<Menues> menues)
        {
            mMenues = new List<Menues>();
            mMenues.AddRange(menues);
            NotifyDataSetChanged();
        }
    }

    public class MenuViewHolder : RecyclerView.ViewHolder
    {

        public ImageView foodimage { get; private set; }
        public TextView foodname { get; private set; }
        public TextView foodPrice { get; private set; }

        public MenuViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            foodimage = itemView.FindViewById<ImageView>(Resource.Id.DrImage);
            foodname = itemView.FindViewById<TextView>(Resource.Id.DrName);
            foodPrice = itemView.FindViewById<TextView>(Resource.Id.DrItem);
        }
    }
}