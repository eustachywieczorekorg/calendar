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

namespace Morris
{
    public class dialog_prompt : DialogFragment
    {
        Button yes, no;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_prompt, container, false);

            yes = view.FindViewById<Button>(Resource.Id.btnYes);
            no = view.FindViewById<Button>(Resource.Id.btnNo);

            yes.Click += (object sender, EventArgs e) =>
            {

                /*WebClient client = new WebClient();
                NameValueCollection parameters = new NameValueCollection();
                Uri url = new Uri("http://217.208.71.183/calendarusers/DeleteEvent.php");
                parameters.Add("eventid", mEvents[position].Id.ToString());
                client.UploadValuesCompleted += Client_UploadValuesCompleted;*/
            };

            no.Click += (object sender, EventArgs e) =>
            {

            };
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;//Set the animation
        }
    }
}