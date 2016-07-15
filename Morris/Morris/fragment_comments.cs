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
    public class fragment_comments : Android.Support.V4.App.Fragment
    {
        int EventId;
        List<Comment> mComments;
        CommentsAdapter mAdapter;
        ListView mlistview;
        string EventName;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        string usernamefromsp;


        public fragment_comments(int id, string ename)
        {
            EventId = id;
            EventName = ename;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_comments, container, false);
            HasOptionsMenu = true;
            ((activity_main)this.Activity).SupportActionBar.Title = "Comments: " + EventName;
            Android.Support.V7.Widget.Toolbar mToolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarcomments);
            mlistview = view.FindViewById<ListView>(Resource.Id.commentslistview);
            EditText message = view.FindViewById<EditText>(Resource.Id.txtcomment);
            Button send = view.FindViewById<Button>(Resource.Id.comment);
            mlistview.StackFromBottom = true;

            usernamefromsp = pref.GetString("Username", String.Empty);

            send.Click += (object sender, EventArgs e) => 
            {
                WebClient client1 = new WebClient();
                Uri url1 = new Uri("http://217.208.71.183/calendarusers/Comment.php");
                NameValueCollection parameters1 = new NameValueCollection();
                parameters1.Add("eventid", EventId.ToString());
                parameters1.Add("message", message.Text);
                parameters1.Add("username", usernamefromsp);
                client1.UploadValuesCompleted += Client1_UploadValuesCompleted;
                client1.UploadValuesAsync(url1, "POST", parameters1);
                message.Text = "";
            };

            WebClient client = new WebClient();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadComments.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("eventid", EventId.ToString());
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);


            return view;
        }
        

        private void Client1_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            WebClient client = new WebClient();
            Uri url = new Uri("http://217.208.71.183/calendarusers/LoadComments.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("eventid", EventId.ToString());
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
        }

        public override void OnDestroy()
        {
            ((activity_main)this.Activity).SupportActionBar.Title = "Morris" + " (" + usernamefromsp + ")";
            base.OnDestroy();
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string json1 = Encoding.UTF8.GetString(e.Result);
            mComments = new List<Comment>();
            mComments = JsonConvert.DeserializeObject<List<Comment>>(json1);
            if(mComments != null)
            {
                mAdapter = new CommentsAdapter(this.Activity, Resource.Layout.row_comment, mComments);
                mlistview.Adapter = mAdapter;
            }
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

    class CommentsAdapter : BaseAdapter<Comment>
    {
        List<Comment> mComments;
        Context mContext;
        int mLayout;

        public CommentsAdapter(Context context, int layout, List<Comment> comments)
        {
            mContext = context;
            mComments = comments;
            mLayout = layout;

        }
        public override Comment this[int position]
        {
            get
            {
                return mComments[position];
            }
        }

        public override int Count
        {
            get
            {
                return mComments.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(mLayout, parent, false);
            }

            TextView mMessage = row.FindViewById<TextView>(Resource.Id.txtMessage);
            TextView mDate = row.FindViewById<TextView>(Resource.Id.txtDate);
            TextView mUser = row.FindViewById<TextView>(Resource.Id.txtUser);

            mMessage.Text = mComments[position].Message;
            mDate.Text = mComments[position].SendDate.TimeOfDay.ToString();
            mUser.Text = mComments[position].Username;

            return row;
        }
    }

}