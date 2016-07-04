using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using System.Net;
using System.Collections.Specialized;

namespace Morris
{
    [Activity(Label = "Morris EC", Theme ="@style/MyTheme")]
    public class Activity1 : Android.Support.V7.App.AppCompatActivity
    {
        private Android.Support.V7.Widget.Toolbar mToolbar;
        ViewPager _viewPager;
        JavaList<Android.Support.V4.App.Fragment> fragments;
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

            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Morris EC" + "("+ usernamefromsp + ")";


            _viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            _viewPager.Adapter = new LayoutFragmentadapter(SupportFragmentManager, getfragments());
            _viewPager.SetCurrentItem(1, false);
        }
        public void update(object sender, EventArgs e)
        {
            _viewPager.Adapter = new LayoutFragmentadapter(SupportFragmentManager, getfragments());
            _viewPager.SetCurrentItem(1, false);
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