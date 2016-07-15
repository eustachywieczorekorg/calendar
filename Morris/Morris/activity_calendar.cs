using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;
using Android.Support.V4.Widget;

namespace Morris
{ 
    public class activity_calendar : Android.Support.V4.App.Fragment, CalendarView.IOnDateChangeListener, DatePicker.IOnDateChangedListener
    {
        ListView mListView;
        Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEvents.php");
        string usernamefromsp;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        public DatePicker mDatePicker;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_calendar, container, false);

            HasOptionsMenu = true;
            mDatePicker = view.FindViewById<DatePicker>(Resource.Id.datePicker1); 
            mDatePicker.CalendarView.SetOnDateChangeListener(this);
            mListView = view.FindViewById<ListView>(Resource.Id.EventsListView);
            var swipeContainer = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeContainer);
            swipeContainer.SetColorSchemeResources(Android.Resource.Color.HoloBlueLight, Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloOrangeLight, Android.Resource.Color.HoloRedLight);
            swipeContainer.Refresh += SwipeContainer_Refresh;
            NameValueCollection parameters = new NameValueCollection();
            usernamefromsp = pref.GetString("Username", String.Empty);
            parameters.Add("username", usernamefromsp);
            parameters.Add("selecteddate", mDatePicker.Year + "-" + (mDatePicker.Month + 1) + "-" + mDatePicker.DayOfMonth);
            WebClient client = new WebClient();
            client.UploadValuesCompleted += Client1_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);

            return view;
        }

        private void SwipeContainer_Refresh(object sender, EventArgs e)
        {
            NameValueCollection parameters = new NameValueCollection();
            usernamefromsp = pref.GetString("Username", String.Empty);
            parameters.Add("username", usernamefromsp);
            parameters.Add("selecteddate", mDatePicker.Year + "-" + (mDatePicker.Month + 1) + "-" + mDatePicker.DayOfMonth);
            WebClient client = new WebClient();
            client.UploadValuesCompleted += Client1_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
            (sender as SwipeRefreshLayout).Refreshing = false;
        }

        public void UpdateCalendar(object sender, EventArgs e)
        {
            NameValueCollection parameters = new NameValueCollection();
            usernamefromsp = pref.GetString("Username", String.Empty);
            parameters.Add("username", usernamefromsp);
            parameters.Add("selecteddate", mDatePicker.Year + "-" + (mDatePicker.Month + 1) + "-" + mDatePicker.DayOfMonth);
            WebClient client = new WebClient();
            client.UploadValuesCompleted += Client1_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
        }

        private void Client1_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string json1 = System.Text.Encoding.UTF8.GetString(e.Result);
            List<CalendarEvent> mEvents = new List<CalendarEvent>();
            mEvents = JsonConvert.DeserializeObject<List<CalendarEvent>>(json1);
            CalendarEventListAdapter mAdapter;
            mAdapter = new CalendarEventListAdapter(this.Activity, Resource.Layout.row_event, mEvents, this.Activity.FragmentManager);
            mAdapter.eventremoved += MAdapter_eventremoved;
            mAdapter.btncommentspressed += MAdapter_btncommentspressed1;
            mAdapter.btninvitefriendspressed += MAdapter_btninvitefriendspressed1;
            mAdapter.eventnamepressed += MAdapter_eventnamepressed;
            mListView.Adapter = mAdapter;
        }

        private void MAdapter_eventremoved(object sender, EventArgs e)
        {
            ((activity_main)this.Activity).updateall.Invoke(this, new EventArgs());
        }

        private void MAdapter_eventnamepressed(object sender, CalendarEvent e)
        {
            fragment_createevent createeventfrag = new fragment_createevent(e);
            Android.Support.V4.App.FragmentTransaction trans = this.Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.calendarframelayout, createeventfrag, "createeventfrag").AddToBackStack(null);
            trans.Commit();
        }

        private void MAdapter_btninvitefriendspressed1(object sender, commentsfrageventargs e)
        {
            fragment_invitetoevent invitefrag = new fragment_invitetoevent(e.EventId, e.EventName);
            Android.Support.V4.App.FragmentTransaction trans = this.Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.calendarframelayout, invitefrag, "invitefrag").AddToBackStack(null);
            trans.Commit();
        }

        private void MAdapter_btncommentspressed1(object sender, commentsfrageventargs e)
        {
            fragment_comments commentsfragment = new fragment_comments(e.EventId, e.EventName);
            Android.Support.V4.App.FragmentTransaction trans = this.Activity.SupportFragmentManager.BeginTransaction().Add(Resource.Id.calendarframelayout, commentsfragment, "commentsfrag").AddToBackStack(null);
            trans.Commit();
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.actionbar_calendar, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
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
                case Resource.Id.addevent:
                        DateTime mDate2 = mDatePicker.DateTime;
                        fragment_createevent ced = new fragment_createevent(mDate2);
                        ced.eventcreated += UpdateCalendar;
                        Android.Support.V4.App.FragmentTransaction trans;
                        trans = this.Activity.SupportFragmentManager.BeginTransaction().Add(Resource.Id.calendarframelayout, ced, "swag").AddToBackStack(null);
                        trans.Commit();
                    return true;
                case Resource.Id.eventinvites:
                    fragment_eventinvites eventinvitefrag = new fragment_eventinvites();
                    Android.Support.V4.App.FragmentTransaction transaction1 = this.Activity.SupportFragmentManager.BeginTransaction().Add(Resource.Id.calendarframelayout,eventinvitefrag,"eventinvitefrag").AddToBackStack(null);
                    transaction1.Commit();
                    return true;
                default:
                return base.OnOptionsItemSelected(item);
            }   
        }

        public void OnSelectedDayChange(CalendarView view, int year, int month, int dayOfMonth)
        {
            WebClient client = new WebClient();
            NameValueCollection parameters = new NameValueCollection();
            usernamefromsp = pref.GetString("Username", String.Empty);
            parameters.Add("username", usernamefromsp);
            parameters.Add("selecteddate", year + "-" + (month+1) + "-" + dayOfMonth);
            client.UploadValuesCompleted += Client1_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
            mDatePicker.Init(year, month, dayOfMonth, this);
        }

        public void OnDateChanged(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            WebClient client = new WebClient();
            NameValueCollection parameters = new NameValueCollection();
            usernamefromsp = pref.GetString("Username", String.Empty);
            parameters.Add("username", usernamefromsp);
            parameters.Add("selecteddate", year + "-" + (monthOfYear+1) + "-" + dayOfMonth);
            Console.WriteLine("monthofyear" + monthOfYear);
            client.UploadValuesCompleted += Client1_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
        }
    }
    public class commentsfrageventargs : EventArgs
    {
        public int EventId { get; set; }
        public string EventName { get; set; }

        public commentsfrageventargs(int id, string name)
        {
            EventId = id;
            EventName = name;
        }
    }
}

