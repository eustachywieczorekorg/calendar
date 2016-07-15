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
using Android.Graphics.Drawables;

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
        public string usernamefromsp, username;
        public CheckBox cbse;
        public event EventHandler<string> friendusernameclicked;
        public event EventHandler anevent;

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
            username = mFriends[position].UserName;
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            usernamefromsp = pref.GetString("Username", String.Empty);
            Button btnRemoveFriend = row.FindViewById<Button>(Resource.Id.btnremovefriend);

            if (mFriends[position].otshare == 1 && mFriends[position].friend_one == mFriends[position].Id)
            {
                if (friendusername.HasOnClickListeners == false)
                {
                    friendusername.Click += (object sender, EventArgs e) =>
                    {
                        friendusernameclicked.Invoke(this, mFriends[position].UserName);
                    };
                }

            }
            else if (mFriends[position].toshare == 1 && mFriends[position].friend_one != mFriends[position].Id)
            {
                if (friendusername.HasOnClickListeners == false)
                {
                    friendusername.Click += (object sender, EventArgs e) =>
                    {
                        friendusernameclicked.Invoke(this, mFriends[position].UserName);
                    };
                }
            }
            else
            {
                if (friendusername.HasOnClickListeners == false)
                {
                    friendusername.Click += (object sender, EventArgs e) =>
                    {
                        Toast.MakeText(mContext, "User is not sharing events with you", ToastLength.Short).Show();
                    };
                }

            }

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
            cbse = row.FindViewById<CheckBox>(Resource.Id.cbse);

            if(mFriends[position].otshare == 1 && mFriends[position].friend_one != mFriends[position].Id)
            {
                cbse.Checked = true;
            }
            if(mFriends[position].toshare == 1 && mFriends[position].friend_one == mFriends[position].Id)
            {
                cbse.Checked = true;
            }

            if (cbse.HasOnClickListeners == false)
            {
                cbse.Click += (object sender, EventArgs e) =>
                {
                    WebClient client = new WebClient();
                    Uri url = new Uri("http://217.208.71.183/calendarusers/ShareEvents.php");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("username", usernamefromsp);
                    parameters.Add("friendid", mFriends[position].Id.ToString());
                    client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                    client.UploadValuesAsync(url, "POST", parameters);
                };
            }
            
            return row;
        }
        

        private void Client_UploadValuesCompleted2(object sender, UploadValuesCompletedEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(this.mContext, message, ToastLength.Short).Show();
        }

        private void Client_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(this.mContext, message, ToastLength.Short).Show();
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
        NameValueCollection parameters1;
        private Context mContext;
        private int mLayout;
        WebClient client6;
        private List<FriendRequest> mFriendRequests;
        WebClient client5;
        public string usernamefromsp;
        NameValueCollection parameters4;
        public event EventHandler friendaddedordeclined;


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
            ImageButton btnAccept, btnDecline;
            btnAccept = row.FindViewById<ImageButton>(Resource.Id.btnAccept);
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

            btnDecline = row.FindViewById<ImageButton>(Resource.Id.btnDecline);

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
                client6.UploadValuesCompleted -= Client_UploadValuesCompleted;
                friendaddedordeclined.Invoke(this, new EventArgs());
                parameters1.Clear();
        }

        private void Client5_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(this.mContext, message, ToastLength.Short).Show();
            client5.UploadValuesCompleted -= Client5_UploadValuesCompleted;
            friendaddedordeclined.Invoke(this, new EventArgs());
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
        public event EventHandler eventremoved;
        bool onlyviewing;
        public event EventHandler<commentsfrageventargs> btncommentspressed;
        public event EventHandler<commentsfrageventargs> btninvitefriendspressed;
        public event EventHandler<CalendarEvent> eventnamepressed;

        public CalendarEventListAdapter(Context context, int layout, List<CalendarEvent> events, FragmentManager fragmentmanager)
        {
            onlyviewing = false;
            mContext = context;
            mLayout = layout;
            mEvents = events;
            mFragmentManager = fragmentmanager;
        }

        public CalendarEventListAdapter(Context context, int layout, List<CalendarEvent> events, FragmentManager fragmentmanager, bool viewing)
        {
            onlyviewing = true;
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
           
            TextView EventName = row.FindViewById<TextView>(Resource.Id.roweventname);
            EventName.Text = mEvents[position].EventName;
            
            TextView EventDescription = row.FindViewById<TextView>(Resource.Id.roweventdescription);
            EventDescription.Text = mEvents[position].EventDescription;

            TextView Location = row.FindViewById<TextView>(Resource.Id.textLocation);
            Location.Text = mEvents[position].Location;

            TextView StartDate = row.FindViewById<TextView>(Resource.Id.datestart);
            StartDate.Text = mEvents[position].StartDate.Year + "-" + mEvents[position].StartDate.Month + "-" + mEvents[position].StartDate.Day.ToString();

            TextView TimeStart = row.FindViewById<TextView>(Resource.Id.timestart);
            TimeStart.Text = mEvents[position].StartTime;

            TextView EndDate = row.FindViewById<TextView>(Resource.Id.dateend);
            EndDate.Text = mEvents[position].EndDate.Year + "-" + mEvents[position].EndDate.Month + "-" + mEvents[position].EndDate.Day.ToString();

            TextView TimeEnd = row.FindViewById<TextView>(Resource.Id.timeend);
            TimeEnd.Text =  mEvents[position].EndTime;


            mLinearLayout = row.FindViewById<LinearLayout>(Resource.Id.linearlayout123);
            
            TextView Week = row.FindViewById<TextView>(Resource.Id.theweek);
            Week.Text ="W." + mEvents[position].Week.ToString();

            List<Color> mColors = new List<Color>();
            mColors.Add(new Color(237, 74, 54));
            mColors.Add(new Color(26, 198, 181));

            Color bgcolor = new Color(mColors[position%2]);
            mLinearLayout.SetBackgroundColor(bgcolor);

            ImageButton btnInviteFriend = row.FindViewById<ImageButton>(Resource.Id.buttonInviteFriend);
            btnInviteFriend.SetBackgroundColor(bgcolor);
            
            ImageButton BtnChangeReq = row.FindViewById<ImageButton>(Resource.Id.buttonChangeReqs);
            BtnChangeReq.SetBackgroundColor(bgcolor);

            ImageButton btncomments = row.FindViewById<ImageButton>(Resource.Id.buttoncomments);

            ImageButton btnmembers = row.FindViewById<ImageButton>(Resource.Id.btnMembers);

            if (onlyviewing == false)
            {
                if (btncomments.HasOnClickListeners == false)
                {
                    btncomments.Click += (object sender, EventArgs e) =>
                    {
                        commentsfrageventargs asd = new commentsfrageventargs(mEvents[position].Id, mEvents[position].EventName);
                        btncommentspressed.Invoke(this, asd);
                    };
                }
                if (btndelete.HasOnClickListeners == false)
                {
                    btndelete.Click += (object sender, EventArgs e) =>
                    {
                        FragmentTransaction transaction = mFragmentManager.BeginTransaction();
                        dialog_prompt dialogprompt = new dialog_prompt(mEvents[position].Creator, mEvents[position].Id);
                        dialogprompt.eventremoved += Dialogprompt_eventremoved;
                        dialogprompt.Show(transaction, "dialog fragment");
                    };
                }
                if (EventName.HasOnClickListeners == false)
                {
                    EventName.Click += (object sender, EventArgs e) =>
                    {
                        eventnamepressed.Invoke(this, mEvents[position]);
                    };
                }
                if (mEvents[position].Creator == usernamefromsp)
                {
                    btnInviteFriend.SetBackgroundResource(Resource.Drawable.plus);
                    btnInviteFriend.Clickable = true;
                    if (btnInviteFriend.HasOnClickListeners == false)
                    {
                        btnInviteFriend.Click += (object sender, EventArgs e) =>
                        {
                            commentsfrageventargs asd = new commentsfrageventargs(mEvents[position].Id, mEvents[position].EventName);
                            btninvitefriendspressed.Invoke(this, asd);
                        };
                    };

                    BtnChangeReq.SetBackgroundResource(Resource.Drawable.settingskoncept1);
                    BtnChangeReq.Clickable = true;
                    if (BtnChangeReq.HasOnClickListeners == false)
                    {
                        BtnChangeReq.Click += (object sender, EventArgs e) =>
                        {
                            //TODO
                        };
                    }
                }
            }
            else
            {
                BtnChangeReq.Clickable = false;
                btncomments.Clickable = false;
                btnmembers.Clickable = false;
                btndelete.Clickable = false;
                btnInviteFriend.Clickable = false;
                EventName.Clickable = false;
                btndelete.SetBackgroundColor(bgcolor);
                BtnChangeReq.SetBackgroundColor(bgcolor);
                btncomments.SetBackgroundColor(bgcolor);
                btnmembers.SetBackgroundColor(bgcolor);
            }

            
            return row;
        }

        public void Dialogprompt_eventremoved(object sender, EventArgs e)
        {
            eventremoved.Invoke(this, new EventArgs());
        }
    }

    class InviteToEventAdapter : BaseAdapter<Friend>
    {
        public EventHandler onfriendinvited;
        private Context mContext;
        private int mLayout;
        private List<Friend> mFriends;
        public string usernamefromsp;
        public int Eventid;
        TextView txtUsername;
        private CheckBox mCheckBox;
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
            
            mCheckBox = row.FindViewById<CheckBox>(Resource.Id.FriendCheckBox);
            if(mCheckBox.HasOnClickListeners == false)
            {
                mCheckBox.Click += (object sender, EventArgs e) =>
                {
                    if (mCheckBox.Checked == false)
                    {
                        onfriendinvited += (object qwe, EventArgs q) =>
                        {
                            WebClient client5 = new WebClient();
                            NameValueCollection parameters2 = new NameValueCollection();
                            parameters2 = new NameValueCollection();
                            parameters2.Add("username", usernamefromsp);
                            parameters2.Add("friendusername", mFriends[position].UserName);
                            parameters2.Add("eventid", Eventid.ToString());
                            client5 = new WebClient();
                            client5.UploadValuesCompleted += Client5_UploadValuesCompleted;
                            client5.UploadValuesAsync(url, "POST", parameters2);
                        };
                    }
                    else
                    {
                        NameValueCollection parameters2 = new NameValueCollection();
                    }
                };
            }

            return row;
        }

        private void Client5_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string json1 = Encoding.UTF8.GetString(e.Result);
            Toast.MakeText(mContext, json1, ToastLength.Short).Show();
        }
    }

    class CalendarEventInviteListAdapter : BaseAdapter<CalendarEvent>
    {
        private Context mContext;
        private int mLayout;
        private List<CalendarEvent> mEvents;
        public string usernamefromsp;
        WebClient client2;
        WebClient client1;
        NameValueCollection parameters1;
        NameValueCollection parameters2;
        public event EventHandler eventad;

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
            TextView StartTime = row.FindViewById<TextView>(Resource.Id.mstarttime);
            StartTime.Text = mEvents[position].StartTime;
            TextView EndTime = row.FindViewById<TextView>(Resource.Id.mendtime);
            EndTime.Text = mEvents[position].EndTime;
            TextView StartDate = row.FindViewById<TextView>(Resource.Id.mstartdate);
            StartDate.Text = mEvents[position].StartDate.Year + "-" + mEvents[position].StartDate.Month + "-" + mEvents[position].StartDate.Day.ToString();
            TextView EndDate = row.FindViewById<TextView>(Resource.Id.menddate);
            EndDate.Text = mEvents[position].EndDate.Year + "-" + mEvents[position].EndDate.Month + "-" + mEvents[position].EndDate.Day.ToString();
            TextView week = row.FindViewById<TextView>(Resource.Id.theweek1);
            week.Text = "W."+ mEvents[position].Week.ToString();

            LinearLayout mLinearLayout = row.FindViewById<LinearLayout>(Resource.Id.linearlayout111);
            List<Color> mColors = new List<Color>();
            mColors.Add(new Color(237, 74, 54));
            mColors.Add(new Color(26, 198, 181));

            Color bgcolor = new Color(mColors[position%2]);
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
                client2.UploadValuesCompleted -= Client2_UploadValuesCompleted;
                parameters2.Clear();
            eventad.Invoke(sender, e);
        }

        private void Client1_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
                string json1 = Encoding.UTF8.GetString(e.Result);
                Toast.MakeText(mContext, json1, ToastLength.Short).Show();
                client1.UploadValuesCompleted -= Client1_UploadValuesCompleted;
                parameters1.Clear();
            eventad.Invoke(sender, e);
        }
       
    }
}