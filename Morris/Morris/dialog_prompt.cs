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

namespace Morris
{
    public class dialog_prompt : DialogFragment
    {
        Button yes, no;
        string mCreator;
        int mId;
        public event EventHandler eventremoved;

        public dialog_prompt(string creator, int aids)
        {
            mCreator = creator;
            mId = aids;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_prompt, container, false);
            TextView pmessage;
            pmessage = view.FindViewById<TextView>(Resource.Id.textprompt);

            yes = view.FindViewById<Button>(Resource.Id.btnYes);
            no = view.FindViewById<Button>(Resource.Id.btnNo);
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string usernamefromsp = pref.GetString("Username", String.Empty);

            if (mCreator == usernamefromsp)
            {
                yes.Click += (object sender, EventArgs e) =>
                {

                    WebClient client = new WebClient();
                    Uri url = new Uri("http://217.208.71.183/calendarusers/DeleteEvent.php");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("username", usernamefromsp);
                    parameters.Add("eventid", mId.ToString());
                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client.UploadValuesAsync(url, "POST", parameters);
                };

                no.Click += (object sender, EventArgs e) =>
                {
                    this.Dismiss();
                };
            }
            else
            {
                pmessage.Text = "Are You Sure You Want To Leave This Event?";
                yes.Click += (object sender, EventArgs e) =>
                {

                    WebClient client = new WebClient();
                    Uri url = new Uri("http://217.208.71.183/calendarusers/ExitEvent.php");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("eventid", mId.ToString());
                    parameters.Add("username", usernamefromsp);
                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client.UploadValuesAsync(url, "POST", parameters);
                };

                no.Click += (object sender, EventArgs e) =>
                {
                    this.Dismiss();
                };
            }
            

            return view;
        }
        

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(this.Activity, message, ToastLength.Short).Show();
            eventremoved.Invoke(sender,e);
            if (message == "Event Deleted")
            {
                this.Dismiss();
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;//Set the animation
        }
    }
}