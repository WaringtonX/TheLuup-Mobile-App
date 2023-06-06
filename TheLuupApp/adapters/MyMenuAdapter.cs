using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TheLuupApp.MainActivity;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp.adapters
{
    class MyMenuAdapter : RecyclerView.Adapter
    {
        private List<CartCode> mCardCode;
        private Context mContext;
        private Service1 client1 = new Service1();
        private string mUser_id;
        public event EventHandler<int> ItemClick;
        public MyMenuAdapter(Context Context,List<CartCode> cardCode,string user_id)
        {
            mCardCode = cardCode;
            mContext = Context;
            mUser_id = user_id;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyMenuViewHolder vh = holder as MyMenuViewHolder;

                vh.Name.Text = mCardCode[position].Cartname;
                vh.Note.Text = mCardCode[position].Cartnote;

               //Restuarent
                int r_id = mCardCode[position].Rid;
                foreach (Restuarent res in mRestuarent)
                {
                   if (r_id.Equals(res.R_id))
                   {
                      Restuarent r = res;
                      vh.RestName.Text = r.Name;
                    }
                }

            //DateTime
                DateTime date = mCardCode[position].DateTime;
                vh.Date.Text = date.ToLongDateString()  + " " + date.ToShortTimeString();

                string  u_id = mCardCode[position].Uid.ToString();
                string cartcode = mCardCode[position].Cartcode;
                int userId = Int32.Parse(mUser_id);
              
                vh.ItemView.Click += (sender, e) =>
                {
                  //  Animation animation = AnimationUtils.LoadAnimation(mContext, Resource.Animation.bounce_new);
                   // vh.ItemView.StartAnimation(animation);
                    Intent log = new Intent(mContext, typeof(CartActivity));
                    log.PutExtra("user_id", u_id);
                    log.PutExtra("user_cartcode", cartcode);
                    mContext.StartActivity(log);
                };


        }

        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }

        public override int ItemCount
        {
            get { return mCardCode.Count; }
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                    Inflate(Resource.Layout.layout_menu_listitem, parent, false);
            MyMenuViewHolder vh = new MyMenuViewHolder(itemView,OnClick);
            return vh;
        }

        public void updateViews(List<CartCode> CardCode)
        {
            // updateRequest();;
            mCardCode = new List<CartCode>();
            mCardCode.AddRange(CardCode);
            NotifyDataSetChanged();
        }

    }

    public class MyMenuViewHolder : RecyclerView.ViewHolder
    {
        public FloatingActionButton deletecartcode { get; private set; }
        public TextView Name { get; private set; }
        public TextView Note { get; private set; }
        public TextView RestName { get; private set; }
        public TextView Date { get; private set; }
        private CardView card;

        public MyMenuViewHolder(View itemview, Action<int> listener) : base(itemview)
        {
            // Locate and cache view references:
            deletecartcode = itemview.FindViewById<FloatingActionButton>(Resource.Id.DeleteMenu);
            Name = itemview.FindViewById<TextView>(Resource.Id.MyMenuName);
            Note = itemview.FindViewById<TextView>(Resource.Id.bookrestNote);
            RestName = itemview.FindViewById<TextView>(Resource.Id.bookrestname);
            Date = itemview.FindViewById<TextView>(Resource.Id.bookresdate);
            card = itemview.FindViewById<CardView>(Resource.Id.cardviewaut);
            deletecartcode.Click += (sender, e) => listener(base.Position);
        }
    }
}