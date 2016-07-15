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
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace Morris
{
    [Activity(Label = "Activity2" , Theme = "@style/MyTheme")]
    public class activity_friendeventlist : AppCompatActivity
    {
        public ListView mListview;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        string usernamefromsp;
        private CalendarEventListAdapter mAdapter;
        private List<CalendarEvent> mEvents;
        string friendusername;

        public activity_friendeventlist()
        {

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.fragment_friendeventlist);
            mListview = FindViewById<ListView>(Resource.Id.listView4);

            var swipeContainer = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeContainer);
            swipeContainer.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
            swipeContainer.Refresh += SwipeContainer_Refresh;

            Android.Support.V7.Widget.Toolbar mToolbar;
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarEA2);
            SetSupportActionBar(mToolbar);
            if (Intent.GetStringExtra("friendusername") != null)
            {
                 friendusername = Intent.GetStringExtra("friendusername");
            }
            usernamefromsp = pref.GetString("Username", String.Empty);
            SupportActionBar.Title = "Morris EC" + " (" + friendusername + ")";
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            WebClient client = new WebClient();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEventList.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", friendusername);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
        }

        private void SwipeContainer_Refresh(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEventList.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", friendusername);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
            (sender as SwipeRefreshLayout).Refreshing = false;
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            string json1 = UTF8Encoding.UTF8.GetString(e.Result);
            mEvents = new List<CalendarEvent>();
            mEvents = JsonConvert.DeserializeObject<List<CalendarEvent>>(json1);
            mAdapter = new CalendarEventListAdapter(this, Resource.Layout.row_event, mEvents, FragmentManager, true);
            mListview.Adapter = mAdapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_event, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction();

            switch (item.ItemId)
            {
                case Resource.Id.action_logout:
                    ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                    ISharedPreferencesEditor edit = pref.Edit();
                    edit.Clear();
                    edit.Apply();
                    Intent intent = new Intent(this, typeof(activity_loginregister));
                    this.StartActivity(intent);
                    this.Dispose();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
