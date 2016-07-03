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

namespace Morris
{
    public class EventActivity : Android.Support.V4.App.Fragment
    {
        public ListView mListview;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        string usernamefromsp;
        private CalendarEventListAdapter mAdapter;
        private List<CalendarEvent> mEvents;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.EventActivity, container, false);
            HasOptionsMenu = true;
            mListview = view.FindViewById<ListView>(Resource.Id.listView1);

            usernamefromsp = pref.GetString("Username", String.Empty);
            WebClient client = new WebClient();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEventList.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", usernamefromsp);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.actionbar_event, menu);
            return;
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            FragmentTransaction transaction = this.Activity.FragmentManager.BeginTransaction();
            string json1 = UTF8Encoding.UTF8.GetString(e.Result);
            mEvents = new List<CalendarEvent>();
            mEvents = JsonConvert.DeserializeObject<List<CalendarEvent>>(json1);
            mAdapter = new CalendarEventListAdapter(this.Activity, Resource.Layout.row_event, mEvents, this.Activity.FragmentManager);
            mListview.Adapter = mAdapter;
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