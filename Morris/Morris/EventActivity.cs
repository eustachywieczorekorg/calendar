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
using Android.Support.V4.Widget;

namespace Morris
{
    public class EventActivity : Android.Support.V4.App.Fragment
    {
        public ListView mListview;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        string usernamefromsp;
        private CalendarEventListAdapter mAdapter;
        private List<CalendarEvent> mEvents;
        public event EventHandler eventremoved;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.EventActivity, container, false);
            HasOptionsMenu = true;
            mListview = view.FindViewById<ListView>(Resource.Id.listView1);
            var swipeContainer = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeContainer);
            swipeContainer.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
            swipeContainer.Refresh += SwipeContainer_Refresh;
            usernamefromsp = pref.GetString("Username", String.Empty);
            WebClient client = new WebClient();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEventList.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", usernamefromsp);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
            return view;
        }

        private void SwipeContainer_Refresh(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEventList.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", usernamefromsp);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
            (sender as SwipeRefreshLayout).Refreshing = false;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.actionbar_event, menu);
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            FragmentTransaction transaction = this.Activity.FragmentManager.BeginTransaction();
            string json1 = UTF8Encoding.UTF8.GetString(e.Result);
            mEvents = new List<CalendarEvent>();
            mEvents = JsonConvert.DeserializeObject<List<CalendarEvent>>(json1);
            mAdapter = new CalendarEventListAdapter(this.Activity, Resource.Layout.row_event, mEvents, this.Activity.FragmentManager);
            mAdapter.eventremoved += MAdapter_eventremoved;
            mAdapter.btncommentspressed += MAdapter_btncommentspressed;
            mAdapter.btninvitefriendspressed += MAdapter_btninvitefriendspressed;
            mAdapter.eventnamepressed += MAdapter_eventnamepressed;
            mListview.Adapter = mAdapter;
        }

        private void MAdapter_eventnamepressed(object sender, CalendarEvent e)
        {
            CreateEventFragment createeventfrag = new CreateEventFragment(e);
            Android.Support.V4.App.FragmentTransaction trans = this.Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.eventactivityframelayout, createeventfrag, "createeventfrag").AddToBackStack(null);
            trans.Commit();
        }

        private void MAdapter_btninvitefriendspressed(object sender, int e)
        {
            Invitetoeventdialog invitefrag = new Invitetoeventdialog(e);
            Android.Support.V4.App.FragmentTransaction trans = this.Activity.SupportFragmentManager.BeginTransaction().Add(Resource.Id.eventactivityframelayout, invitefrag, "invitefrag").AddToBackStack(null);
            trans.Commit();
        }
        private void MAdapter_btncommentspressed(object sender, int e)
        {
            fragment_comments commentsfragment = new fragment_comments(e);
            Android.Support.V4.App.FragmentTransaction trans = this.Activity.SupportFragmentManager.BeginTransaction().Add(Resource.Id.eventactivityframelayout, commentsfragment, "commentsfrag").AddToBackStack(null);
            trans.Commit();
        }

        public void UpdateEventList(object sender, EventArgs e)
        {
            usernamefromsp = pref.GetString("Username", String.Empty);
            WebClient client = new WebClient();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEventList.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", usernamefromsp);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
        }

        private void MAdapter_eventremoved(object sender, EventArgs e)
        {
            eventremoved.Invoke(this, new EventArgs());
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            Android.App.FragmentTransaction transaction = this.Activity.FragmentManager.BeginTransaction();

            switch (item.ItemId)
            {
                case Resource.Id.action_logout:
                    ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                    ISharedPreferencesEditor edit = pref.Edit();
                    edit.Clear();
                    edit.Apply();
                    Intent intent = new Intent(this.Activity, typeof(LoginRegisterActivity));
                    this.StartActivity(intent);
                    this.Dispose();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
    }
}