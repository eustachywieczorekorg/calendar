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

    public class fragment_eventinvites : Android.Support.V4.App.Fragment
    {
        List<CalendarEvent> mEventlist;
        private ListView mListView;
        private CalendarEventInviteListAdapter mAdapter;
        public Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEventInvites.php");
        public string usernamefromsp;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_eventinvites, container, false);
            HasOptionsMenu = true;
            ((activity_main)this.Activity).SupportActionBar.Title = "Eventinvites" + " (" + usernamefromsp + ")";

            mListView = view.FindViewById<ListView>(Resource.Id.listView1);
            usernamefromsp = pref.GetString("Username", String.Empty);
            WebClient client = new WebClient();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", usernamefromsp);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);

            return view;
        }
        

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string json1 = System.Text.Encoding.UTF8.GetString(e.Result);
            mEventlist = new List<CalendarEvent>();
            mEventlist = JsonConvert.DeserializeObject<List<CalendarEvent>>(json1);
            mAdapter = new CalendarEventInviteListAdapter(Activity, Resource.Layout.row_eventinvite, mEventlist);
            mAdapter.eventad += MAdapter_update;
            mListView.Adapter = mAdapter;
        }
        

        public override void OnDestroy()
        {
            ((activity_main)this.Activity).SupportActionBar.Title = "Morris" + " (" + usernamefromsp + ")";
            ((activity_main)this.Activity).updateall.Invoke(this, new EventArgs());
            base.OnDestroy();
        }

        private void MAdapter_update(object sender, EventArgs e)
        {
            usernamefromsp = pref.GetString("Username", String.Empty);
            WebClient client = new WebClient();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", usernamefromsp);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.actionbar_event, menu);
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
                    Intent intent = new Intent(this.Activity, typeof(activity_loginregister));
                    this.StartActivity(intent);
                    this.Dispose();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

        }

    }
}