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
using Android.Graphics;
using System.Net;
using System.Collections.Specialized;

namespace Morris
{
    class FriendsListAdapter : BaseAdapter<Friend>
    {
        public event EventHandler OnFriendRemoved;
        WebClient client4;
        private Context mContext;
        private int mLayout;
        NameValueCollection parameters2;
        private List<Friend> mFriends;
        public string usernamefromsp;


        public FriendsListAdapter(Context context, int layout, List<Friend> friends)
        {
            mContext = context;
            mLayout = layout;
            mFriends = friends;
        }

        public override Friend this[int position]
        {
            get { return mFriends[position]; }
        }

        public override int Count
        {
            get { return mFriends.Count; }
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

            TextView friendusername;
            friendusername = row.FindViewById<TextView>(Resource.Id.txtName);
            friendusername.Text = mFriends[position].UserName;
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            usernamefromsp = pref.GetString("Username", String.Empty);
            Button btnRemoveFriend = row.FindViewById<Button>(Resource.Id.btnremovefriend);

            if(btnRemoveFriend.HasOnClickListeners == false)
            {
                btnRemoveFriend.Click += (object sender, EventArgs e) =>
                {
                    parameters2 = new NameValueCollection();
                    parameters2.Add("username", usernamefromsp);
                    parameters2.Add("friendusername", mFriends[position].UserName);
                    client4 = new WebClient();
                    Uri urldecline = new Uri("http://217.208.71.183/calendarusers/DeclineFriend.php");
                    client4.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client4.UploadValuesAsync(urldecline, "POST", parameters2);
                };
            }

            return row;
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
                string message = Encoding.UTF8.GetString(e.Result);
                Toast.MakeText(this.mContext, message, ToastLength.Short).Show();
                OnFriendRemoved(this, new EventArgs());
                client4.UploadValuesCompleted -= Client_UploadValuesCompleted;
                parameters2.Clear();
        }
    }

    class FriendRequestListAdapter : BaseAdapter<FriendRequest>
    {
        public event EventHandler update;
        NameValueCollection parameters1;
        private Context mContext;
        private int mLayout;
        WebClient client6;
        private List<FriendRequest> mFriendRequests;
        WebClient client5;
        public string usernamefromsp;
        NameValueCollection parameters4;


        public FriendRequestListAdapter(Context context, int layout, List<FriendRequest> friendRequests)
        {
            mContext = context;
            mLayout = layout;
            mFriendRequests = friendRequests;
        }

        public override FriendRequest this[int position]
        {
            get { return mFriendRequests[position]; }
        }

        public override int Count
        {
            get { return mFriendRequests.Count; }
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

            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            usernamefromsp = pref.GetString("Username", String.Empty);
            TextView usernametext = row.FindViewById<TextView>(Resource.Id.txtName);
            usernametext.Text = mFriendRequests[position].UserName;
            Button btnAccept, btnDecline;
            btnAccept = row.FindViewById<Button>(Resource.Id.btnAccept);
            if(btnAccept.HasOnClickListeners == false)
            {
                btnAccept.Click += (object sender, EventArgs e) =>
                {
                    parameters4 = new NameValueCollection();
                    parameters4.Add("username", usernamefromsp);
                    parameters4.Add("friendusername", mFriendRequests[position].UserName);
                    client5 = new WebClient();
                    Uri urlaccept = new Uri("http://217.208.71.183/calendarusers/AcceptFriend.php");
                    client5.UploadValuesCompleted += Client5_UploadValuesCompleted;
                    client5.UploadValuesAsync(urlaccept, "POST", parameters4);
                };
            }

            btnDecline = row.FindViewById<Button>(Resource.Id.btnDecline);

            if(btnDecline.HasOnClickListeners == false)
            {
                btnDecline.Click += (object sender, EventArgs e) =>
                {
                    parameters1 = new NameValueCollection();
                    parameters1.Add("username", usernamefromsp);
                    parameters1.Add("friendusername", mFriendRequests[position].UserName);
                    client6 = new WebClient();
                    Uri urldecline = new Uri("http://217.208.71.183/calendarusers/DeclineFriend.php");
                    client6.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client6.UploadValuesAsync(urldecline, "POST", parameters1);
                };

            }


            return row;
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
                string message = Encoding.UTF8.GetString(e.Result);
                Toast.MakeText(this.mContext, message, ToastLength.Short).Show();
                update(this, new EventArgs());
                client6.UploadValuesCompleted -= Client_UploadValuesCompleted;
                parameters1.Clear();
        }

        private void Client5_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(this.mContext, message, ToastLength.Short).Show();
            update(this, new EventArgs());
            client5.UploadValuesCompleted -= Client5_UploadValuesCompleted;
            parameters4.Clear();
        }


    }

    class CalendarEventListAdapter : BaseAdapter<CalendarEvent>
    {

        private Context mContext;
        private int mLayout;
        private List<CalendarEvent> mEvents;
        public FragmentManager mFragmentManager;
        public string usernamefromsp;
        public LinearLayout mLinearLayout;
        bool bgset = false;

        public CalendarEventListAdapter(Context context, int layout, List<CalendarEvent> events, FragmentManager fragmentmanager)
        {
            mContext = context;
            mLayout = layout;
            mEvents = events;
            mFragmentManager = fragmentmanager;
        }

        public override CalendarEvent this[int position]
        {
            get { return mEvents[position]; }
        }

        public override int Count
        {
            get { return mEvents.Count; }
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
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            usernamefromsp = pref.GetString("Username", String.Empty);

            ImageButton btndelete = row.FindViewById<ImageButton>(Resource.Id.button1);
            if (btndelete.HasOnClickListeners == false)
            {
                btndelete.Click += (object sender, EventArgs e) =>
                {
                    FragmentTransaction transaction = mFragmentManager.BeginTransaction();
                    dialog_prompt dialogprompt = new dialog_prompt();
                    dialogprompt.Show(transaction, "dialog fragment");
                };
            }
            TextView EventName = row.FindViewById<TextView>(Resource.Id.roweventname);
            EventName.Text = mEvents[position].EventName;
            if(EventName.HasOnClickListeners == false)
            {
                EventName.Click += (object sender, EventArgs e) =>
                {
                    FragmentTransaction transaction = mFragmentManager.BeginTransaction();
                    CreateEventDialog changeeventdialog = new CreateEventDialog(mEvents[position].Id, mEvents[position].EventName, mEvents[position].EventDescription, mEvents[position].Date, mEvents[position].Location, mEvents[position].TimeFrom, mEvents[position].TimeTo, mEvents[position].Category);
                    changeeventdialog.Show(transaction, "dialog fragment");
                };
            }
            TextView EventDescription = row.FindViewById<TextView>(Resource.Id.roweventdescription);
            EventDescription.Text = mEvents[position].EventDescription;

            TextView Location = row.FindViewById<TextView>(Resource.Id.textLocation);
            Location.Text = mEvents[position].Location;

            TextView TimeFrom = row.FindViewById<TextView>(Resource.Id.timefrom);
            TimeFrom.Text = mEvents[position].TimeFrom + " ";

            TextView TimeTo = row.FindViewById<TextView>(Resource.Id.timeto);
            TimeTo.Text = " " + mEvents[position].TimeTo;

            mLinearLayout = row.FindViewById<LinearLayout>(Resource.Id.linearlayout123);

            TextView Date = row.FindViewById<TextView>(Resource.Id.rowdate);
            Date.Text = mEvents[position].Date.Year + "-" + mEvents[position].Date.Month + "-" + mEvents[position].Date.Day.ToString();
           

            TextView Week = row.FindViewById<TextView>(Resource.Id.theweek);
            Week.Text ="W." + mEvents[position].Week.ToString();

            List<Color> mColors = new List<Color>();
            mColors.Add(new Color(170, 170, 170));
            mColors.Add(new Color(210, 84, 00));
            mColors.Add(new Color(232, 76, 61));
            mColors.Add(new Color(53, 152, 219));
            mColors.Add(new Color(154, 101, 66));
            mColors.Add(new Color(241, 196, 15));
            mColors.Add(new Color(27, 188, 155));
            mColors.Add(new Color(39, 174, 97));
            mColors.Add(new Color(143, 68, 173));

            Color bgcolor = new Color(mColors[mEvents[position].Category]);
            mLinearLayout.SetBackgroundColor(bgcolor);

            ImageButton btnInviteFriend = row.FindViewById<ImageButton>(Resource.Id.buttonInviteFriend);
            btnInviteFriend.SetBackgroundColor(bgcolor);
            string kr8er = mEvents[position].Creator;

                if(kr8er == usernamefromsp)
                {
                btnInviteFriend.SetBackgroundResource(Resource.Drawable.plus);
                btnInviteFriend.Clickable = true;
                if (btnInviteFriend.HasOnClickListeners == false){

                        btnInviteFriend.Click += (object sender, EventArgs e) =>
                        {
                            FragmentTransaction transaction = mFragmentManager.BeginTransaction();
                            Invitetoeventdialog invitetoeventdialog = new Invitetoeventdialog(mEvents[position].Id);
                            invitetoeventdialog.Show(transaction, "dialog fragment");
                        };
                    };
                }
            else
            {
                btnInviteFriend.Clickable = false;
            };
            
           

            return row;
        }
    }

    class InviteToEventAdapter : BaseAdapter<Friend>
    {
        public event EventHandler onfriendinvited;
        private Context mContext;
        private int mLayout;
        private List<Friend> mFriends;
        public string usernamefromsp;
        public int Eventid;
        private NameValueCollection parameters2;
        private WebClient client5;
        TextView txtUsername;
        private ImageButton btnInvite;
        string message;
        Uri url = new Uri("http://217.208.71.183/calendarusers/InviteToEvent.php");

        ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

        public InviteToEventAdapter(Context context, int layout, List<Friend> friends, int eventid)
        {
            mContext = context;
            mLayout = layout;
            mFriends = friends;
            Eventid = eventid;

        }

        public override Friend this[int position]
        {
            get { return mFriends[position]; }
        }

        public override int Count
        {
            get { return mFriends.Count; }
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
                row = LayoutInflater.From(mContext).Inflate(mLayout, null, false);
            }

            
            usernamefromsp = pref.GetString("Username", String.Empty);

            txtUsername = row.FindViewById<TextView>(Resource.Id.friendusername);
            txtUsername.Text = mFriends[position].UserName;
            
            btnInvite = row.FindViewById<ImageButton>(Resource.Id.btninvite);
            if (btnInvite.HasOnClickListeners == false)
            {
                btnInvite.Click += (object sender, EventArgs e) =>
                {
                    parameters2 = new NameValueCollection();
                    parameters2.Add("username", usernamefromsp);
                    parameters2.Add("friendusername", mFriends[position].UserName);
                    parameters2.Add("eventid", Eventid.ToString());
                    client5 = new WebClient();
                    client5.UploadValuesCompleted += Client5_UploadValuesCompleted;
                    client5.UploadValuesAsync(url, "POST", parameters2);
                };
            }

            return row;
        }

        private void Client5_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
                 message = Encoding.UTF8.GetString(e.Result);
                 Toast.MakeText(mContext, message, ToastLength.Short).Show();
                 client5.UploadValuesCompleted -= Client5_UploadValuesCompleted;
                 parameters2.Clear();
                 onfriendinvited.Invoke(sender,new EventArgs());
        }

    }

    class CalendarEventInviteListAdapter : BaseAdapter<CalendarEvent>
    {
        public event EventHandler update;
        private Context mContext;
        private int mLayout;
        private List<CalendarEvent> mEvents;
        public string usernamefromsp;
        WebClient client2;
        WebClient client1;
        NameValueCollection parameters1;
        NameValueCollection parameters2;
        public CalendarEventInviteListAdapter(Context context, int layout, List<CalendarEvent> events)
        {
            mContext = context;
            mLayout = layout;
            mEvents = events;

        }

        public override CalendarEvent this[int position]
        {
            get { return mEvents[position]; }
        }

        public override int Count
        {
            get { return mEvents.Count; }
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

            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            usernamefromsp = pref.GetString("Username", String.Empty);
            TextView EventName = row.FindViewById<TextView>(Resource.Id.roweventinvitename);
            EventName.Text = mEvents[position].EventName;
            TextView EventDescription = row.FindViewById<TextView>(Resource.Id.roweventinvitedescription);
            EventDescription.Text = mEvents[position].EventDescription;
            TextView Location = row.FindViewById<TextView>(Resource.Id.txtLocation);
            Location.Text = mEvents[position].Location;
            TextView TimeFrom = row.FindViewById<TextView>(Resource.Id.timefrom1);
            TimeFrom.Text = mEvents[position].TimeFrom;
            TextView TimeTo = row.FindViewById<TextView>(Resource.Id.timeto1);
            TimeTo.Text = mEvents[position].TimeTo;
            TextView Date = row.FindViewById<TextView>(Resource.Id.rowdate1);
            Date.Text = mEvents[position].Date.Year + "-" + mEvents[position].Date.Month + "-" + mEvents[position].Date.Day.ToString();
            TextView week = row.FindViewById<TextView>(Resource.Id.theweek1);
            week.Text = mEvents[position].Week.ToString();

            LinearLayout mLinearLayout = row.FindViewById<LinearLayout>(Resource.Id.linearlayout111);
            List<Color> mColors = new List<Color>();
            mColors.Add(new Color(170, 170, 170));
            mColors.Add(new Color(210, 84, 00));
            mColors.Add(new Color(232, 76, 61));
            mColors.Add(new Color(53, 152, 219));
            mColors.Add(new Color(154, 101, 66));
            mColors.Add(new Color(241, 196, 15));
            mColors.Add(new Color(27, 188, 155));
            mColors.Add(new Color(39, 174, 97));
            mColors.Add(new Color(143, 68, 173));

            Color bgcolor = new Color(mColors[mEvents[position].Category]);
            mLinearLayout.SetBackgroundColor(bgcolor);

            ImageButton btnAccept = row.FindViewById<ImageButton>(Resource.Id.buttonAccept1);
            if (btnAccept.HasOnClickListeners == false)
            {
                btnAccept.Click += (object sender, EventArgs e) =>
                {
                    client1 = new WebClient();
                    parameters1 = new NameValueCollection();
                    Uri url = new Uri("http://217.208.71.183/calendarusers/AcceptEventInvite.php");
                    parameters1.Add("eventid", mEvents[position].Id.ToString());
                    parameters1.Add("username", usernamefromsp);
                    Console.WriteLine(position);
                    client1.UploadValuesCompleted += Client1_UploadValuesCompleted;
                    client1.UploadValuesAsync(url, "POST", parameters1);
                };
            }

            ImageButton btnDecline = row.FindViewById<ImageButton>(Resource.Id.buttonDecline1);
            if (btnDecline.HasOnClickListeners == false)
            {
                btnDecline.Click += (object sender, EventArgs e) =>
                {
                    client2 = new WebClient();
                    parameters2 = new NameValueCollection();
                    Uri url = new Uri("http://217.208.71.183/calendarusers/DeclineEventInvite.php");
                    parameters2.Add("eventid", mEvents[position].Id.ToString());
                    parameters2.Add("username", usernamefromsp);
                    client2.UploadValuesCompleted += Client2_UploadValuesCompleted;
                    client2.UploadValuesAsync(url, "POST", parameters2);
                };
            }
            return row;

        }

        private void Client2_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
                string json1 = Encoding.UTF8.GetString(e.Result);
                Toast.MakeText(mContext, json1, ToastLength.Short).Show();
                update(sender, new EventArgs());
                client2.UploadValuesCompleted -= Client2_UploadValuesCompleted;
                parameters2.Clear(); 
        }

        private void Client1_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
                string json1 = Encoding.UTF8.GetString(e.Result);
                Toast.MakeText(mContext, json1, ToastLength.Short).Show();
                update(sender, new EventArgs());
                client1.UploadValuesCompleted -= Client1_UploadValuesCompleted;
                parameters1.Clear();
        }
       
    }
}