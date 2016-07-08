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
        List<string> mStrings;
        ArrayAdapter<string> mAdapter;
        ListView mlistview;

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

            send.Click += (object sender, EventArgs e) => 
            {
                WebClient client1 = new WebClient();
                Uri url1 = new Uri("http://217.208.71.183/calendarusers/Comment.php");
                NameValueCollection parameters1 = new NameValueCollection();
                parameters1.Add("eventid", EventId.ToString());
                parameters1.Add("message", message.Text);
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
            mStrings = new List<string>();
            mStrings = JsonConvert.DeserializeObject<List<string>>(json1);
            if(mStrings != null)
            {
                mAdapter = new ArrayAdapter<string>(this.Activity, Android.Resource.Layout.SimpleListItem1, mStrings);
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
}