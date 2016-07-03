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
using Java.Sql;

namespace Morris
{

    public class CreateEventDialog : DialogFragment
    {
        public event EventHandler eventcreated;
        EditText eventName;
        EditText eventDescription;
        EditText location;
        Button createeventbtn;
        TimePicker fromtp;
        TimePicker totp;
        Spinner mSpinner;
        TextView date;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        DateTime selecteddate;
        EditText week;
        int category;
        string EventName,EventDescription, Location;
        bool editing;

        public CreateEventDialog(DateTime getdate)
        {
            editing = false;
            selecteddate = getdate;
        }
        public CreateEventDialog(int id, string ename, string edescription, DateTime edate, string elocation, string timefrom, string timeto, int ecategory)
        {
            editing = true;
            selecteddate = edate;
            EventName = ename;
            EventDescription = edescription;
            Location = elocation;
            category = ecategory;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_addevent, container, false);

           

            mSpinner = view.FindViewById<Spinner>(Resource.Id.spinner);
            if(mSpinner.HasOnClickListeners == false)
            {
                mSpinner.ItemSelected += MSpinner_ItemSelected;
            }

            var adapter = ArrayAdapter.CreateFromResource(this.Activity, Resource.Array.category_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            mSpinner.Adapter = adapter;

            date = view.FindViewById<TextView>(Resource.Id.theDate);
            location = view.FindViewById<EditText>(Resource.Id.theLocation);
            eventName = view.FindViewById<EditText>(Resource.Id.eventName);
            eventDescription = view.FindViewById<EditText>(Resource.Id.eventDescription);
            createeventbtn = view.FindViewById<Button>(Resource.Id.createEventBtn);

            fromtp = view.FindViewById<TimePicker>(Resource.Id.fromTimePicker);
            totp = view.FindViewById<TimePicker>(Resource.Id.toTimePicker);
            week = view.FindViewById<EditText>(Resource.Id.theweek);
            totp.SetIs24HourView(Java.Lang.Boolean.True);
            fromtp.SetIs24HourView(Java.Lang.Boolean.True);
            fromtp.Focusable = true;
            totp.Focusable = true;
            fromtp.Activated = true;
            totp.Activated = true;


            date.Text = selecteddate.Year + "-" + selecteddate.Date.Month + "-" + selecteddate.Date.Day.ToString();

            if (editing)
            {
                location.Text = Location;
                eventName.Text = EventName;
                eventDescription.Text = EventDescription;
                createeventbtn.Text = "Update Event";
                mSpinner.SetSelection(category);

                createeventbtn.Click += (object sender, EventArgs e) =>
                {
                    if (eventName.Text != EventName)
                    {

                    }
                    if (eventDescription.Text != EventDescription)
                    {

                    }
                    if (location.Text != Location)
                    {

                    }
                   
                    if (eventName.Text != EventName)
                    {

                    }
                };
            }
            else
            {
                createeventbtn.Click += (object sender, EventArgs e) =>
                {
                    WebClient client = new WebClient();
                    Uri url = new Uri("http://217.208.71.183/calendarusers/CreateEvent.php");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("eventname", eventName.Text);
                    parameters.Add("eventdescription", eventDescription.Text);
                    parameters.Add("date", selecteddate.Year + "-" + selecteddate.Month + "-" + selecteddate.Day);
                    parameters.Add("category", category.ToString());

                    if (fromtp.CurrentHour.ToString().Length == 1 && fromtp.CurrentMinute.ToString().Length == 1)
                    {
                        parameters.Add("fromtp", "0" + fromtp.CurrentHour + ":0" + fromtp.CurrentMinute);
                    }
                    else if (fromtp.CurrentHour.ToString().Length == 2 && fromtp.CurrentMinute.ToString().Length == 1)
                    {
                        parameters.Add("fromtp", fromtp.CurrentHour + ":0" + fromtp.CurrentMinute);
                    }
                    else if (fromtp.CurrentHour.ToString().Length == 1 && fromtp.CurrentMinute.ToString().Length == 2)
                    {
                        parameters.Add("fromtp", "0" + fromtp.CurrentHour + ":" + fromtp.CurrentMinute);
                    }
                    else
                    {
                        parameters.Add("fromtp", fromtp.CurrentHour + ":" + fromtp.CurrentMinute);
                    }


                    if (totp.CurrentHour.ToString().Length == 1 && totp.CurrentMinute.ToString().Length == 1)
                    {
                        parameters.Add("totp", "0" + totp.CurrentHour + ":0" + totp.CurrentMinute);
                    }
                    else if (totp.CurrentHour.ToString().Length == 2 && totp.CurrentMinute.ToString().Length == 1)
                    {
                        parameters.Add("totp", totp.CurrentHour + ":0" + totp.CurrentMinute);
                    }
                    else if (totp.CurrentHour.ToString().Length == 1 && totp.CurrentMinute.ToString().Length == 2)
                    {
                        parameters.Add("totp", "0" + totp.CurrentHour + ":" + totp.CurrentMinute);
                    }
                    else
                    {
                        parameters.Add("totp", totp.CurrentHour + ":" + totp.CurrentMinute);
                    }

                    string usernamefromsp = pref.GetString("Username", String.Empty);
                    parameters.Add("username", usernamefromsp);
                    parameters.Add("location", location.Text);
                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client.UploadValuesAsync(url, "POST", parameters);
                };
            }

            return view;
        }

        private void MSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            category = (int)e.Id;
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string response = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(this.Activity, response, ToastLength.Short).Show();
            if(response=="Event created")
            {
                eventcreated(this, new EventArgs());
                this.Dismiss();
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