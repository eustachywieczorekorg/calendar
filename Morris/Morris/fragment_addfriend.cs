using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Net;
using System.Collections.Specialized;

namespace Morris
{

    class fragment_addfriend : Android.Support.V4.App.Fragment
    {
         Button mButtonCreateFriend;
         EditText txtFriendUserName;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        string usernamefromsp;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.fragment_addfriend, container, false);
            usernamefromsp = pref.GetString("Username", String.Empty);
            HasOptionsMenu = true;
            ((activity_main)this.Activity).SupportActionBar.Title = "Add Friends" + " (" + usernamefromsp + ")";

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
            friendparameters.Add("username", usernamefromsp);
            friendparameters.Add("friendone", txtFriendUserName.Text);
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
                    this.Activity.SupportFragmentManager.PopBackStack();
                }
            });
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