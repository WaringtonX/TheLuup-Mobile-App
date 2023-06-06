using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Widget;
using Java.Lang;

namespace TheLuupApp.adapters
{
    class TabsPagerAdapter : FragmentPagerAdapter
    {
        private string usrid;
        public TabsPagerAdapter(FragmentManager fm,string uid) : base(fm)
        {
            this.usrid = uid;
        }

        public override int Count
        {
            get { return 3; }
        }

        public override Fragment GetItem(int position)
        {
            Bundle bundle = new Bundle();
            bundle.PutString("user_id", usrid);

            switch (position)
            {
                case 0:
                    UserChats userChats = new UserChats();
                    userChats.Arguments = bundle;
                    return null;
                case 1:
                    Profile profile = new Profile();
                    profile.Arguments = bundle;
                    return null;
                case 2:
                    Request request = new Request();
                    request.Arguments = bundle;
                    return request;
                default:
                    return null;

            }
        }

       
  
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            var titles = CharSequence.ArrayFromStringArray(new[] {
               "CHATS",
               "PEOPLE",
               "REQUEST",
           });
            return titles[position];
        }
    }
}