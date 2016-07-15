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
    public class activity_main : Android.Support.V7.App.AppCompatActivity
    {
        ViewPager _viewPager;
        activity_friends fa;
        activity_calendar ca;
        fragment_eventlist ea;
        JavaList<Android.Support.V4.App.Fragment> fragments;
        public EventHandler updateall;

        public activity_main()
        {
            fa = new activity_friends();
            ca = new activity_calendar();
            ea = new fragment_eventlist();
        }
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_viewpager);

            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string usernamefromsp = pref.GetString("Username", String.Empty);

            updateall += update;
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
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public void update(object sender, EventArgs e)
        {
            ca.UpdateCalendar(this, new EventArgs());
            ea.UpdateEventList(this, new EventArgs());
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