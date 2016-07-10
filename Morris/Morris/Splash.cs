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
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using Android.Support.V7.App;

namespace Morris
{
    [Activity(Label = "Morris", MainLauncher = true, Icon = "@drawable/morris_icon4", Theme = "@style/MyTheme")]
    public class Splash : AppCompatActivity
    {
        private Android.Support.V7.Widget.Toolbar mToolbar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Splash);
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.logintoolbar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Morris Event Calendar";
            ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            string userName = pref.GetString("Username", String.Empty);
            string password = pref.GetString("Password", String.Empty);

            if (userName == String.Empty || password == String.Empty)
            {
                //No saved credentials, take user to login screen
                Intent intent = new Intent(this, typeof(LoginRegisterActivity));
                this.StartActivity(intent);
                this.Finish();
            }

            else
            {
                //There are saved credentials
                WebClient login = new WebClient();
                Uri uri = new Uri("http://217.208.71.183/calendarusers/login.php");
                NameValueCollection parameters2 = new NameValueCollection();
                parameters2.Add("username", userName);
                parameters2.Add("password", password);
                login.UploadValuesCompleted += Login_UploadValuesCompleted;
                login.UploadValuesAsync(uri, "POST", parameters2);
            }
        }

        private void Login_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);
            string a = JsonConvert.DeserializeObject<string>(json);
            if (a == "Login successful")
            {
                //Successful take the user to application
                Intent intent = new Intent(this, typeof(Activity1));
                this.StartActivity(intent);
                this.Finish();
            }

            else
            {
                //Unsuccesful, take user to login screen
                ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                ISharedPreferencesEditor edit = pref.Edit();
                edit.Clear();
                edit.Apply();
                Intent intent = new Intent(this, typeof(LoginRegisterActivity));
                this.StartActivity(intent);
                this.Finish();
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }
    }
}