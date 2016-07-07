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
            fa.openfriendeventactivity += Fa_openfriendeventactivity;

            Android.Support.V7.Widget.Toolbar mToolbar;
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Morris EC" + "("+ usernamefromsp + ")";


            _viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            _viewPager.Adapter = new LayoutFragmentadapter(SupportFragmentManager, getfragments());
            _viewPager.SetCurrentItem(1, false);
        }

        private void Fa_openfriendeventactivity(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(EventActivity));
            StartActivity(intent);
        }

        public void update(object sender, EventArgs e)
        {
            int i = _viewPager.CurrentItem;
            _viewPager.Adapter = new LayoutFragmentadapter(SupportFragmentManager, getfragments());
            _viewPager.SetCurrentItem(i, false);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }
        private JavaList<Android.Support.V4.App.Fragment> getfragments()
        {
            JavaList<Android.Support.V4.App.Fragment> fragments;
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