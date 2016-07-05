using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;

namespace Morris
{ 
    public class CalendarActivity : Android.Support.V4.App.Fragment, CalendarView.IOnDateChangeListener, DatePicker.IOnDateChangedListener
    {

        ListView mListView;
         Uri url = new Uri("http://217.208.71.183/calendarusers/LoadEvents.php");
        string usernamefromsp;
        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        public DatePicker mDatePicker;
        public event EventHandler updateevent;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.CalendarActivity, container, false);
            HasOptionsMenu = true;
            mDatePicker = view.FindViewById<DatePicker>(Resource.Id.datePicker1); 
            mDatePicker.CalendarView.SetOnDateChangeListener(this);

            mListView = view.FindViewById<ListView>(Resource.Id.EventsListView);
            NameValueCollection parameters = new NameValueCollection();
            usernamefromsp = pref.GetString("Username", String.Empty);
            parameters.Add("username", usernamefromsp);
            parameters.Add("selecteddate", mDatePicker.Year + "-" + (mDatePicker.Month + 1) + "-" + mDatePicker.DayOfMonth);
            WebClient client = new WebClient();
            client.UploadValuesCompleted += Client1_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);

            return view;
        }
        
        private void CalendarView_DateChange(object sender, CalendarView.DateChangeEventArgs e)
        {
            WebClient client = new WebClient();
            NameValueCollection parameters = new NameValueCollection();
            usernamefromsp = pref.GetString("Username", String.Empty);
            parameters.Add("username", usernamefromsp);
            parameters.Add("selecteddate", mDatePicker.Year + "-" + (mDatePicker.Month + 1) + "-" + mDatePicker.DayOfMonth);
            client.UploadValuesCompleted += Client1_UploadValuesCompleted;
            client.UploadValuesAsync(url, "POST", parameters);
        }

        private void MDatePicker_update(object sender, EventArgs e)
        {
            WebClient client1 = new WebClient();
            NameValueCollection parameters1 = new NameValueCollection();
            usernamefromsp = pref.GetString("Username", String.Empty);
            parameters1.Add("username", usernamefromsp);
            parameters1.Add("selecteddate", mDatePicker.Year + "-" + (mDatePicker.Month + 1) + "-" + mDatePicker.DayOfMonth);
            client1.UploadValuesCompleted += Client1_UploadValuesCompleted;
            client1.UploadValuesAsync(url, "POST", parameters1);
            updateevent.Invoke(sender, e);
            
        }

        private void Client1_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string json1 = System.Text.Encoding.UTF8.GetString(e.Result);
            List<CalendarEvent> mEvents = new List<CalendarEvent>();
            mEvents = JsonConvert.DeserializeObject<List<CalendarEvent>>(json1);
            CalendarEventListAdapter mAdapter;
            mAdapter = new CalendarEventListAdapter(this.Activity, Resource.Layout.row_event, mEvents, this.Activity.FragmentManager);
            mAdapter.eventremoved += MDatePicker_update;
            mListView.Adapter = mAdapter;
        }

        
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.actionbar_calendar, menu);
            
            return;
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
                    Intent intent = new Intent(this.Activity, typeof(LoginRegisterActivity));
                    this.StartActivity(intent);
                    this.Dispose();
                    return true;

                case Resource.Id.addevent:
                    DateTime mDate2 = mDatePicker.DateTime;
                    CreateEventDialog createeventdialog = new CreateEventDialog(mDate2);
                    createeventdialog.Show(transaction, "dialog fragment");
                    createeventdialog.eventcreated += MDatePicker_update;
                    return true;

                case Resource.Id.eventinvites:
                    dialog_eventinvites eventinvitedialog = new dialog_eventinvites();
                    eventinvitedialog.Show(transaction, "dialog fragment");
                    eventinvitedialog.eventad += MDatePicker_update;
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
}

