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
    public class dialog_comments : DialogFragment
    {
        int EventId;
        List<Comment> mComments;
        CommentsAdapter mAdapter;
        ListView mlistview;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        
        public dialog_comments(int id)
        {
            EventId = id;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.dialog_comments, container, false);
            mlistview = view.FindViewById<ListView>(Resource.Id.commentslistview);
            EditText message = view.FindViewById<EditText>(Resource.Id.txtcomment);
            Button send = view.FindViewById<Button>(Resource.Id.comment);
            string usernamefromsp = pref.GetString("Username", String.Empty);

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

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
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