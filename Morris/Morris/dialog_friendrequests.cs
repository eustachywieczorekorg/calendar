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

namespace Morris
{
    public class FriendRequestDialog : DialogFragment
    {

        public ListView mListView;
        private FriendRequestListAdapter mAdapter;
        private List<FriendRequest> mFriendRequests;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        
        Uri url = new Uri("http://217.208.71.183/calendarusers/LoadFriendRequests.php");
        NameValueCollection parameters = new NameValueCollection();
        public event EventHandler updatefriends;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_friendrequests, container, false);
            mListView = view.FindViewById<ListView>(Resource.Id.FriendRequestlistView);

            string usernamefromsp = pref.GetString("Username", String.Empty);
            parameters.Add("username", usernamefromsp);
            WebClient client = new WebClient();
            client.UploadValuesCompleted += Client_UploadValuesCompleted12;
            client.UploadValuesAsync(url, "POST", parameters);
            

            return view;
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            updatefriends(this, new EventArgs());

        }

        public void Client_UploadValuesCompleted12(object sender, UploadValuesCompletedEventArgs e)
        {

            string json = Encoding.UTF8.GetString(e.Result);
            mFriendRequests = new List<FriendRequest>();
            mFriendRequests = JsonConvert.DeserializeObject<List<FriendRequest>>(json);
            mAdapter = new FriendRequestListAdapter(this.Activity, Resource.Layout.row_friendrequest, mFriendRequests);
            mAdapter.update += MAdapter_update;
            mListView.Adapter = mAdapter;
            
           
        }

        private void MAdapter_update(object sender, EventArgs e)
        {
            WebClient client1 = new WebClient();
            client1.UploadValuesCompleted += Client_UploadValuesCompleted12;
            client1.UploadValuesAsync(url, "POST", parameters);
        }
        

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}
