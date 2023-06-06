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
using Refractored.Controls;
using Square.Picasso;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.Request;

namespace TheLuupApp.adapters
{
    class RequestAdapter : RecyclerView.Adapter
    {
        private List<LRequest> nMRequests;
        private Context mContext;
        private Service1 client1 = new Service1();
        private string mUserId;
        private static Random random = new Random();

        public RequestAdapter(Context Context, List<LRequest> requests, string UserId)
        {
            nMRequests = requests;
            mContext = Context;
            mUserId = UserId;
        }



        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RequestViewHolder vh = holder as RequestViewHolder;
            Users u = new Users();
            int user_id = Int32.Parse(mUserId);
            u = client1.getUser(nMRequests[position].Ur_id, true);
            vh.Name.Text = u.Username.ToString();
            vh.fullnames.Text = u.Name.ToString() + " " + u.Surname.ToString();


            Random r = new Random();
            int red = r.Next(255 - 0 + 1) + 0;
            int green = r.Next(255 - 0 + 1) + 0;
            int blue = r.Next(255 - 0 + 1) + 0;
            vh.card.SetCardBackgroundColor(Color.Rgb(red, green, blue));
            string name = u.Name;
            string firstword = name.Substring(0, 1);
            vh.carttittle.Text = firstword;

            vh.btnAccept.Click += (sender, e) =>
            {
                string chatCode = RandomString();
               if (client1.AddFreind(chatCode, user_id, true, u.U_id,true) == "Freind Added")
                {
                    if (client1.AddFreind(chatCode, u.U_id, true, user_id, true) == "Freind Added")
                    {
                        if (client1.DeleteRequest(user_id, true, u.U_id, true) == "Success")
                        {
                            Toast.MakeText(mContext, "Luup Request Accepted!", ToastLength.Short).Show();
                            nMRequests.RemoveAt(position);
                            NotifyDataSetChanged();
                        }
                    }
                }
            };

            vh.btndecline.Click += (sender, e) =>
            {
                if (client1.DeleteRequest(user_id, true, u.U_id, true) == "Success")
                {
                    Toast.MakeText(mContext, "Luup Request Declined!", ToastLength.Short).Show();
                    nMRequests.RemoveAt(position);
                    NotifyDataSetChanged();
                }
            };
        }

        public override int ItemCount
        {
            get { return nMRequests.Count; }
        }

        private static string RandomString()
        {
            int length = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                  Inflate(Resource.Layout.request_listitem, parent, false);
            RequestViewHolder vh = new RequestViewHolder(itemView);
            return vh;
        }

        public void updateViews(List<LRequest> requests)
        {
            // updateRequest();
            nMRequests = new List<LRequest>();
            nMRequests.AddRange(requests);
            NotifyDataSetChanged();
        }
    }

   
    public class RequestViewHolder : RecyclerView.ViewHolder
    {
        public CardView card { get; private set; }
        public TextView carttittle { get; private set; }
        public TextView Name { get; private set; }
        public TextView fullnames { get; private set; }
        public Button btnAccept { get; private set; }
        public Button btndecline { get; private set; }

        public RequestViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            card = itemView.FindViewById<CardView>(Resource.Id.card_request);
            carttittle = itemView.FindViewById<TextView>(Resource.Id.cardtittlereq);
            Name = itemView.FindViewById<TextView>(Resource.Id.RlikerName);
            fullnames = itemView.FindViewById<TextView>(Resource.Id.Runamesurname);
            btnAccept = itemView.FindViewById<Button>(Resource.Id.btnacceptrequest);
            btndecline = itemView.FindViewById<Button>(Resource.Id.btndeclinerequest);
        }
    }
}