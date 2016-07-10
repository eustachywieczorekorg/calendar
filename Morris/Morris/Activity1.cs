using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;

namespace Morris
{
    [Activity(Label = "Morris EC", Theme ="@style/MyTheme")]
    public class Activity1 : Android.Support.V7.App.AppCompatActivity
    {
        ViewPager _viewPager;
        FriendsActivity fa;
        CalendarActivity ca;
        EventActivity ea;
        JavaList<Android.Support.V4.App.Fragment> fragments;

        public Activity1()
        {
            fa = new FriendsActivity();
            ca = new CalendarActivity();
            ea = new EventActivity();
        }
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewPager);

            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string usernamefromsp = pref.GetString("Username", String.Empty);

            ca.updateevent += update;
            ea.eventremoved += update;

            Android.Support.V7.Widget.Toolbar mToolbar;
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Morris" + " ("+ usernamefromsp + ")";
            


            _viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            _viewPager.Adapter = new LayoutFragmentadapter(SupportFragmentManager, getfragments());
            _viewPager.SetCurrentItem(1, false);
        }
        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                SupportFragmentManager.PopBackStack();
                ca.closed = true;
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public void update(object sender, EventArgs e)
        {
            ca.UpdateCalendar(sender, e);
            ea.UpdateEventList(sender, e);
        }
        
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }

        private JavaList<Android.Support.V4.App.Fragment> getfragments()
        {
            fragments = new JavaList<Android.Support.V4.App.Fragment>();
            fragments.Add(fa);
            fragments.Add(ca);
            fragments.Add(ea);
            return fragments;
        }
    }
    
    public class LayoutFragmentadapter : FragmentPagerAdapter
    {
        JavaList<Android.Support.V4.App.Fragment> mJavalist;

        public LayoutFragmentadapter(Android.Support.V4.App.FragmentManager fm, JavaList<Android.Support.V4.App.Fragment> mjavalist) : base(fm)
        {
            mJavalist = mjavalist;
        }
        public override int Count
        {
            get
            {
                return 3;
            }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return mJavalist[position];
        }
    }
        
    
}