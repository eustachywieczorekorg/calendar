using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Android.Support.V7.App;

namespace Morris
{
    public class Invitetoeventdialog : Android.Support.V4.App.Fragment
    {
        public int Eventid;
        public string usernamefromsp;
        List<Friend> mFriends;
        InviteToEventAdapter mAdapter;
        ListView mListView;
        Button mButton;

        public Invitetoeventdialog(int eventid)
        {
            Eventid = eventid;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Invitetoevent, container, false);
            ((Activity1)this.Activity).SupportActionBar.Hide();
            mListView = view.FindViewById<ListView>(Resource.Id.invitetoeventlistview);

            mButton = view.FindViewById<Button>(Resource.Id.btnInvite);
            mButton.Click += MButton_Click;

            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            usernamefromsp = pref.GetString("Username", String.Empty);

            WebClient client = new WebClient();
            NameValueCollection parameters = new NameValueCollection();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadUninvitedFriends.php");
            parameters.Add("username", usernamefromsp);
            parameters.Add("id", Eventid.ToString());

            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
            return view;
      
        }

        private void MButton_Click(object sender, EventArgs e)
        {
            mAdapter.onfriendinvited.Invoke(this, e);
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);
            mFriends = new List<Friend>();
            mFriends = JsonConvert.DeserializeObject<List<Friend>>(json);
            mAdapter = new InviteToEventAdapter(this.Activity, Resource.Layout.row_invitetoevent, mFriends, Eventid);
            mAdapter.onfriendinvited += MAdapter_onfriendinvited;
            mListView.Adapter = mAdapter;
         }

        private void MAdapter_onfriendinvited(object sender, EventArgs e)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            usernamefromsp = pref.GetString("Username", String.Empty);

            WebClient client1 = new WebClient();
            NameValueCollection parameters = new NameValueCollection();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadUninvitedFriends.php");
            parameters.Add("username", usernamefromsp);
            parameters.Add("id", Eventid.ToString());

            client1.UploadValuesCompleted += Client_UploadValuesCompleted;
            client1.UploadValuesAsync(url, "POST", parameters);
        }
        public override void OnDestroy()
        {

            ((Activity1)this.Activity).SupportActionBar.Show();
            base.OnDestroy();
        }
    }
}
