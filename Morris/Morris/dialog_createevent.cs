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
        DateTime selecteddate, mEnddate;
        public DateTime ddate;
        EditText week;
        int category, myId;
        string EventName,EventDescription, Location;
        string TimeStart, TimeEnd;
        string Creator;
        bool editing;
        Uri url, url2, url3, url4, url5, url6, url7, url8;
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
            starttp.Focusable = true;
            endtp.Focusable = true;
            starttp.Activated = true;
            endtp.Activated = true;
            
            startdate.Text = selecteddate.Year + "-" + selecteddate.Date.Month + "-" + selecteddate.Date.Day.ToString();

            Button addday = view.FindViewById<Button>(Resource.Id.btnadd);
            addday.Clickable = true;
            addday.Click += Addday_Click;

            if (editing)
            {
                eventName.Text = EventName;
                eventDescription.Text = EventDescription;
                location.Text = Location;
                Java.Lang.Integer shour, smin, ehour, emin;
                int starthour, startminute, endhour, endminute;
                
                int.TryParse(TimeStart.Substring(0, 2), out starthour);
                int.TryParse(TimeStart.Substring(3, 2), out startminute);
                int.TryParse(TimeEnd.Substring(0, 2), out endhour);
                int.TryParse(TimeEnd.Substring(3, 2), out endminute);
                shour = new Java.Lang.Integer(starthour);
                smin = new Java.Lang.Integer(startminute);
                ehour= new Java.Lang.Integer(endhour);
                emin = new Java.Lang.Integer(endminute);

                starttp.CurrentHour = shour;
                starttp.CurrentMinute = smin;
                endtp.CurrentHour = ehour;
                endtp.CurrentMinute = emin;
                starttp.SetIs24HourView(Java.Lang.Boolean.True);
                endtp.SetIs24HourView(Java.Lang.Boolean.True);
                enddate.Text = mEnddate.Year + "-" + mEnddate.Date.Month + "-" + mEnddate.Date.Day.ToString();
                createeventbtn.Text = "Update Event";
                mSpinner.SetSelection(category);


                createeventbtn.Click += (object sender, EventArgs e) =>
                {
                    string usernamefromsp = pref.GetString("Username", String.Empty);
                    if (Creator == usernamefromsp)
                    {
                        url = new Uri("http://217.208.71.183/calendarusers/UpdateEventName.php");
                        url2 = new Uri("http://217.208.71.183/calendarusers/UpdateEventDescription.php");
                        url3 = new Uri("http://217.208.71.183/calendarusers/UpdateLocation.php");
                        url4 = new Uri("http://217.208.71.183/calendarusers/UpdateCategory.php");
                        url5 = new Uri("http://217.208.71.183/calendarusers/UpdateToTime.php");
                        url6 = new Uri("http://217.208.71.183/calendarusers/UpdateEndDate.php");
                        url7 = new Uri("http://217.208.71.183/calendarusers/UpdateFromTime.php");
                        url8 = new Uri("http://217.208.71.183/calendarusers/UpdateStartDate.php");
                    }
                    else
                    {
                        url = new Uri("http://217.208.71.183/calendarusers/UpdateEventNameReq.php");
                        url2 = new Uri("http://217.208.71.183/calendarusers/UpdateEventDescriptionReq.php");
                        url3 = new Uri("http://217.208.71.183/calendarusers/UpdateLocationReq.php");
                        url4 = new Uri("http://217.208.71.183/calendarusers/UpdateCategoryReq.php");
                        url5 = new Uri("http://217.208.71.183/calendarusers/UpdateToTimeReq.php");
                        url6 = new Uri("http://217.208.71.183/calendarusers/UpdateEndDateReq.php");
                        url7 = new Uri("http://217.208.71.183/calendarusers/UpdateFromTimeReq.php");
                        url8 = new Uri("http://217.208.71.183/calendarusers/UpdateStartDateReq.php");
                    }
                    if (eventName.Text != EventName)
                    {
                        WebClient client = new WebClient();
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url, "POST", parameters);
                        this.Dismiss();
                        Console.WriteLine("eventname changed");
                    }
                    if (eventDescription.Text != EventDescription)
                    {
                        WebClient client = new WebClient();
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url2, "POST", parameters);
                        this.Dismiss();
                        Console.WriteLine("eventdesc changed");
                    }
                    if (location.Text != Location)
                    {
                        WebClient client = new WebClient();
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url3, "POST", parameters);
                        this.Dismiss();
                        Console.WriteLine("location changed");
                    }
                    if (mSpinner.Id != category)
                    {
                        WebClient client = new WebClient();
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url4, "POST", parameters);
                        this.Dismiss();
                        Console.WriteLine("category changed");
                    }
                    if (startdate.Text != selecteddate.Year + "-" + selecteddate.Date.Month + "-" + selecteddate.Date.Day.ToString())
                    {
                        WebClient client = new WebClient();
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url5, "POST", parameters);
                        this.Dismiss();
                        Console.WriteLine("start date changed");
                    }
                        if (starttp.CurrentHour.ToString() != TimeStart.Substring(0, 2) || starttp.CurrentMinute.ToString() != TimeStart.Substring(3, 2))
                    {
                        WebClient client = new WebClient();
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url6, "POST", parameters);
                        this.Dismiss();
                        Console.WriteLine("start time changed");
                    }
                    if (enddate.Text != mEnddate.Year + "-" + mEnddate.Date.Month + "-" + mEnddate.Date.Day.ToString())
                    {
                        WebClient client = new WebClient();
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url7, "POST", parameters);
                        this.Dismiss();
                        Console.WriteLine("end date changed");
                    }
                        if (endtp.CurrentHour.ToString() != TimeStart.Substring(0, 2) || endtp.CurrentMinute.ToString() != TimeStart.Substring(3, 2))
                    {
                        WebClient client = new WebClient();
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("eventid", myId.ToString());
                        //parameters.Add("", );
                        //parameters.Add("", );
                        client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                        client.UploadValuesAsync(url8, "POST", parameters);
                    }
                };
            }

            else
            {

                starttp.SetIs24HourView(Java.Lang.Boolean.True);
                endtp.SetIs24HourView(Java.Lang.Boolean.True);
                enddate.Text = selecteddate.Year + "-" + selecteddate.Date.Month + "-" + selecteddate.Date.Day.ToString();
                ddate = selecteddate;
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
            ddate = ddate.Add(asd);
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