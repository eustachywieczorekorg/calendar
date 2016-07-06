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
        TimePicker starttp, endtp;
        Spinner mSpinner;
        public TextView startdate, enddate;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        DateTime selecteddate, mEnddate, ddate;
        EditText week;
        int category, myId;
        string EventName,EventDescription, Location;
        string TimeStart, TimeEnd;
        string Creator;
        bool editing;

        public CreateEventDialog(DateTime getdate)
        {
            editing = false;
            selecteddate = getdate;
        }
        public CreateEventDialog(int id, string ename, string edescription, DateTime estartdate,DateTime eenddate, string elocation, string timestart, string timeend, int ecategory, string creator)
        {
            myId = id;
            editing = true;
            selecteddate = estartdate;
            mEnddate = eenddate;
            EventName = ename;
            EventDescription = edescription;
            Location = elocation;
            category = ecategory;
            TimeStart = timestart;
            TimeEnd = timeend;
            Creator = creator;
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

            startdate = view.FindViewById<TextView>(Resource.Id.startdate);
            enddate = view.FindViewById<TextView>(Resource.Id.enddate);
            location = view.FindViewById<EditText>(Resource.Id.theLocation);
            eventName = view.FindViewById<EditText>(Resource.Id.eventName);
            eventDescription = view.FindViewById<EditText>(Resource.Id.eventDescription);
            createeventbtn = view.FindViewById<Button>(Resource.Id.createEventBtn);
            starttp = view.FindViewById<TimePicker>(Resource.Id.fromTimePicker);
            endtp = view.FindViewById<TimePicker>(Resource.Id.toTimePicker);
            week = view.FindViewById<EditText>(Resource.Id.theweek);

            starttp.SetIs24HourView(Java.Lang.Boolean.True);
            endtp.SetIs24HourView(Java.Lang.Boolean.True);
            starttp.Focusable = true;
            endtp.Focusable = true;
            starttp.Activated = true;
            endtp.Activated = true;

            ddate = selecteddate;
            startdate.Text = selecteddate.Year + "-" + selecteddate.Date.Month + "-" + selecteddate.Date.Day.ToString();

            ImageButton addday = view.FindViewById<ImageButton>(Resource.Id.btnaddday);
            addday.Click += Addday_Click;

            if (editing)
            {
                eventName.Text = EventName;
                eventDescription.Text = EventDescription;
                location.Text = Location;
                int starthour, startminute, endhour, endminute;
                int.TryParse(TimeStart.Substring(0, 2), out starthour);
                int.TryParse(TimeStart.Substring(3, 2), out startminute);
                int.TryParse(TimeStart.Substring(0, 2), out endhour);
                int.TryParse(TimeStart.Substring(3, 2), out endminute);
                starttp.Hour = starthour;
                starttp.Minute = startminute;
                endtp.Hour = endhour;
                endtp.Minute = endminute;
                enddate.Text = mEnddate.Year + "-" + mEnddate.Date.Month + "-" + mEnddate.Date.Day.ToString();
                createeventbtn.Text = "Update Event";
                mSpinner.SetSelection(category);

                createeventbtn.Click += (object sender, EventArgs e) =>
                {
                    if (eventName.Text != EventName)
                    {
                        WebClient client = new WebClient();
                        Uri url = new Uri("http://217.208.71.183/calendarusers/UpdateEventNameReq.php");
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url, "POST", parameters);
                    }
                    if (eventDescription.Text != EventDescription)
                    {
                        WebClient client = new WebClient();
                        Uri url = new Uri("http://217.208.71.183/calendarusers/UpdateEventDescriptionReq.php");
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url, "POST", parameters);
                    }
                    if (location.Text != Location)
                    {
                        WebClient client = new WebClient();
                        Uri url = new Uri("http://217.208.71.183/calendarusers/UpdateLocationReq.php");
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url, "POST", parameters);
                    }
                    if (mSpinner.Id != category)
                    {
                        WebClient client = new WebClient();
                        Uri url = new Uri("http://217.208.71.183/calendarusers/UpdateCategoryReq.php");
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url, "POST", parameters);
                    }
                    if (starttp.Hour.ToString() != TimeStart.Substring(0, 2) || starttp.Minute.ToString() != TimeStart.Substring(3, 2))
                    {
                        WebClient client = new WebClient();
                        Uri url = new Uri("http://217.208.71.183/calendarusers/UpdateFromTimeReq.php");
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url, "POST", parameters);
                    }
                    if (endtp.Hour.ToString() != TimeStart.Substring(0, 2) || endtp.Minute.ToString() != TimeStart.Substring(3, 2))
                    {
                        WebClient client = new WebClient();
                        Uri url = new Uri("http://217.208.71.183/calendarusers/UpdateToTimeReq.php");
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url, "POST", parameters);
                    }
                };
            }

            else
            {

                enddate.Text = selecteddate.Year + "-" + selecteddate.Date.Month + "-" + selecteddate.Date.Day.ToString();
                createeventbtn.Click += (object sender, EventArgs e) =>
                {
                    WebClient client = new WebClient();
                    Uri url = new Uri("http://217.208.71.183/calendarusers/CreateEvent.php");
                    NameValueCollection parameters = new NameValueCollection();
                    string usernamefromsp = pref.GetString("Username", String.Empty);

                    Time starttime = new Time((int)starttp.CurrentHour, (int)starttp.CurrentMinute,0);
                    Time endtime = new Time((int)endtp.CurrentHour, (int)endtp.CurrentMinute, 0);

                    parameters.Add("eventname", eventName.Text);
                    parameters.Add("eventdescription", eventDescription.Text);
                    parameters.Add("startdate", selecteddate.Year + "-" + selecteddate.Month + "-" + selecteddate.Day);
                    parameters.Add("starttp", starttime.ToString());
                    parameters.Add("enddate", ddate.Year + "-" + ddate.Month + "-" + ddate.Day);
                    parameters.Add("endtp", endtime.ToString());
                    parameters.Add("category", category.ToString());
                    parameters.Add("username", usernamefromsp);
                    parameters.Add("location", location.Text);
                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client.UploadValuesAsync(url, "POST", parameters);
                };
            }

            return view;
        }

        private void Addday_Click(object sender, EventArgs e)
        {
            TimeSpan asd = new TimeSpan(1, 0, 0, 0);
            ddate.Add(asd);
            enddate.Text = ddate.Year + "-" + ddate.Date.Month + "-" + ddate.Date.Day.ToString();
        }

        private void Client_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
            string response = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(this.Activity, response, ToastLength.Short).Show();
            if (response == "Event updated / Event update req sent")
            {
                eventcreated(this, new EventArgs());
                this.Dismiss();
            }
        }

        private void MSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            category = (int)e.Id;
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string response = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(this.Activity, response, ToastLength.Short).Show();
            if (response == "Event created")
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