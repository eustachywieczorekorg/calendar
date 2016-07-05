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
using Java.Util;
using Android.Text.Format;

namespace Morris
{
    class Friend
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    class FriendRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }


    class CalendarEvent
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public int Week { get; set; }
        public int Category { get; set; }
        public string Creator { get; set; }
    }

    class InvitedFriend
    {
        public int Id { get; set; }
        public int Eventid { get; set; }
        public string Usernamer { get; set; }
        public string Friendusername { get; set; }
    }
}