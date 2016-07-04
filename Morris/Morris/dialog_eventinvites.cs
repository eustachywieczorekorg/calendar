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

    public class dialog_eventinvites : DialogFragment
    {
        List<CalendarEvent> mEventlist;
        private ListView mListView;
        private CalendarEventInviteListAdapter mAdapter;
        public Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEventInvites.php");
        public string usernamefromsp;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        public event EventHandler eventad;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_eventinvites, container, false);
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
            mAdapter.update += MAdapter_update;
            mListView.Adapter = mAdapter;
        }

        private void MAdapter_update(object sender, EventArgs e)
        {
            usernamefromsp = pref.GetString("Username", String.Empty);
            WebClient client = new WebClient();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", usernamefromsp);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
            eventad.Invoke(sender, e);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;//Set the animation
        }
    }
}