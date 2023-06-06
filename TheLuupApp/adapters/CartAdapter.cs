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
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp.adapters
{
    class CartAdapter : RecyclerView.Adapter
    {
        private List<Cart> mCart;
        private Context mContext;
        private Service1 client1 = new Service1();
        private Android.Views.Animations.Animation deletemenu;
        private TextView mtotal;
        private Decimal totals = 0;
        List<Decimal> mTotdecimals;
        private string mCartcode;

        public CartAdapter(Context context, List<Cart> cart,List<Decimal> Totdecimals, TextView total, string cartcode)
        {
            mContext = context;
            mCart = cart;
            mtotal = total;
            mCartcode = cartcode;
            mTotdecimals = Totdecimals;
            deletemenu = AnimationUtils.LoadAnimation(mContext, Resource.Animation.bounce);
            setTotal();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CartViewHolder vh = holder as CartViewHolder;
            string muid = mCart[position].Muid.ToString();
            int cart_id = mCart[position].Cid;
            int id = Int32.Parse(muid);

            Menues m = client1.getMenue(id, true);

            vh.foodname.Text = m.Foodname;
            vh.foodes.Text = m.Fooddescription;
            vh.foodprcice.Text = "" + m.Price;
            vh.foodqauntity.Text = mCart[position].Unit.ToString();

            
            vh.deletecard.Click += (sender, e) =>
            {
                vh.deletecard.StartAnimation(deletemenu);
                if (client1.DeleteCart(cart_id,true) == "Success")
                {
                    mCart = new List<Cart>();
                    mCart.AddRange(client1.getCart(mCartcode));
                    NotifyDataSetChanged();
                    getTotal(position);
                    Toast.MakeText(mContext, m.Foodname + " removed!", ToastLength.Short).Show();
                }

            };
        }

        public override int ItemCount
        {
            get { return mCart.Count; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                 Inflate(Resource.Layout.cart_listitrem, parent, false);
            CartViewHolder vh = new CartViewHolder(itemView);
            return vh;
        }

        public void RemoveItem(int position)
        {
             mCart.RemoveAt(position);
             NotifyDataSetChanged();
             NotifyItemChanged(position);
             NotifyItemRangeChanged(position, mCart.Count);
             getTotal(position);      

        }

        public void getTotal(int position)
        {
            mTotdecimals.RemoveAt(position);
            totals = 0;
            mtotal.Text = "";
            for (int i = 0; i < mTotdecimals.Count;i++)
            {
                totals += mTotdecimals[i];
            }

            mtotal.Text = "R" + totals;
        }

        public void setTotal()
        {
            totals = 0;
            mtotal.Text = "";

            for (int i = 0; i < mTotdecimals.Count; i++)
            {
                totals += mTotdecimals[i];
            }
            mtotal.Text = "R" + totals;
        }
    }

    public class CartViewHolder : RecyclerView.ViewHolder
    {

        public FloatingActionButton deletecard { get; private set; }
        public TextView foodname { get; private set; }
        public TextView foodprcice { get; private set; }
        public TextView foodes { get; private set; }
        public TextView foodqauntity { get; private set; }

        public CartViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            deletecard = itemView.FindViewById<FloatingActionButton>(Resource.Id.deletfoods);
            foodname = itemView.FindViewById<TextView>(Resource.Id.foodnames);
            foodes = itemView.FindViewById<TextView>(Resource.Id.foodDescription);
            foodqauntity = itemView.FindViewById<TextView>(Resource.Id.foodQuantity);
            foodprcice = itemView.FindViewById<TextView>(Resource.Id.foodprices);

       
        }
    }
}