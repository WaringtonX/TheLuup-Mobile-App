using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp.adapters
{
    class SelectMenuAdaoter : RecyclerView.Adapter
    {
        private List<CartCode> mCardCode;
        private Context mContext;
        private Service1 client1 = new Service1();
        private string mFrom;
        private string mMenuid;
        private string mUnit;
        private string mGroupdcode;
        private string mresID;
        public event EventHandler<int> ItemClick;

        public SelectMenuAdaoter(Context Context,List<CartCode> cardCode,string From, string Menuid, string Unit, string Groupdcode,string resID)
        {
            mCardCode = cardCode;
            mContext = Context;
            mFrom = From;
            mMenuid = Menuid;
            mUnit = Unit;
            mGroupdcode = Groupdcode;
            mresID = resID;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
                SelectViewHolder vh = holder as SelectViewHolder;

                vh.Name.Text = mCardCode[position].Cartname.ToString();      
             
                vh.deletecartcode.Visibility = ViewStates.Invisible;
               
        }

        public override int ItemCount
        {
            get { return mCardCode.Count; }
        }

        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                    Inflate(Resource.Layout.layout_menu_listitem, parent, false);
            SelectViewHolder vh = new SelectViewHolder(itemView, OnClick);
            return vh;
        }

    }

    public class SelectViewHolder : RecyclerView.ViewHolder
    {
        public ImageButton deletecartcode { get; private set; }
        public TextView Name { get; private set; }


        public SelectViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            deletecartcode = itemView.FindViewById<ImageButton>(Resource.Id.DeleteMenu);
            Name = itemView.FindViewById<TextView>(Resource.Id.MyMenuName);
            itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}