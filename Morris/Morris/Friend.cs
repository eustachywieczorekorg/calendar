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
        public int otshare {  get; set; }
        public int toshare { get; set; }
        public int friend_one { get; set; }
                            
    }

    class FriendRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
    
    public class CalendarEvent
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
        public int Week { get; set; }
        public string Creator { get; set; }
    }

    class InvitedFriend
    {
        public int Id { get; set; }
        public int Eventid { get; set; }
        public string Usernamer { get; set; }
        public string Friendusername { get; set; }
    }

    class Comment
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public DateTime SendDate { get; set; }
    }
}