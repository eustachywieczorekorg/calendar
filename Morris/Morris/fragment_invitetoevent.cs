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
    public class fragment_invitetoevent : Android.Support.V4.App.Fragment
    {
        public int Eventid;
        public string EventName;
        public string usernamefromsp;
        List<Friend> mFriends;
        InviteToEventAdapter mAdapter;
        ListView mListView;
        Button mButton;

        public fragment_invitetoevent(int eventid, string ename)
        {
            Eventid = eventid;
            EventName = ename;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_invitetoevent, container, false);
            HasOptionsMenu = true;
            ((activity_main)this.Activity).SupportActionBar.Title = "Send Invites: " + EventName;
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
            mAdapter.onfriendinvited.Invoke(sender, e);
            ((activity_main)this.Activity).SupportFragmentManager.PopBackStack();
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
            ((activity_main)this.Activity).SupportActionBar.Title = "Morris" + " (" + usernamefromsp + ")";
            base.OnDestroy();
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
