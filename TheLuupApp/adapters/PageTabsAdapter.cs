using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp.adapters
{
    class PageTabsAdapter : FragmentPagerAdapter
    {

        private List<Fragment> _fragments;
        private List<Category> mCategories;
        private FragmentManager _fragmentManager;

        public PageTabsAdapter(FragmentManager manager, List<Category> categories) : base(manager)
        {
            mCategories = categories;
            _fragments = new List<Fragment>();
            foreach(Category ct in categories)
            {
                _fragments.Add(new Fragment());
            }
        }

        public override int Count => _fragments == null ? 0 : _fragments.Count;

        public override Fragment GetItem(int position)
        {
            return _fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(mCategories[position].Name);
        }

        public override int GetItemPosition(Java.Lang.Object objectValue)
        {
            return PositionNone;
        }

        public Fragment GetFragment(int position)
        {
            return _fragments[position];
        }
       /* public bool AddTab(TitleFragment fragment)
        {
            if (Fragments == null || fragment == null) return false;

            Fragments.Add(fragment);
            NotifyDataSetChanged();
            return true;
        } */
    }
}