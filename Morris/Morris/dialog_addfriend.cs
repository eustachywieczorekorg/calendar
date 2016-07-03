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

    class dialog_addfriend : DialogFragment
    {
        private Button mButtonCreateFriend;
        private EditText txtFriendUserName;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_addfriend, container, false);

            mButtonCreateFriend = view.FindViewById<Button>(Resource.Id.btnCreateContact);
            txtFriendUserName = view.FindViewById<EditText>(Resource.Id.txtUserName);

            mButtonCreateFriend.Click += mButtonCreateFriend_Click;
            return view;
           
        }

        void mButtonCreateFriend_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("http://217.208.71.183/calendarusers/AddFriend.php");
            NameValueCollection friendparameters = new NameValueCollection();
            friendparameters.Add("friendone", txtFriendUserName.Text);
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string getusername = pref.GetString("Username", String.Empty);
            friendparameters.Add("username", getusername);
            client.UploadValuesCompleted += Client_UploadValuesCompleted1; 
            client.UploadValuesAsync(uri, "POST", friendparameters);   
        }

        private void Client_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
        Activity.RunOnUiThread(() =>
        {
            string CreateFriendEcho = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(Activity, CreateFriendEcho, ToastLength.Short).Show();
            if (CreateFriendEcho == "Friendrequest sent")
            {
                this.Dismiss();
            }
        });

        }

        
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}