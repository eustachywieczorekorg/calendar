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
using Android.Views.InputMethods;

namespace Morris
{
    
    public class dialog_Signin : DialogFragment
    {
        
        public EditText nTxtUsername;
        private EditText nTxtPassword;
        private Button nBtnSignIn;
        public string echo;
        

        public object Jsonconvert { get; private set; }
        public object CurrentFocus { get; private set; }

        //public event EventHandler<OnSignInEventArgs> nOnSignInComplete;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           
            base.OnCreateView(inflater, container, savedInstanceState);
            
            var view = inflater.Inflate(Resource.Layout.dialog_sign_in, container, false);
            nTxtUsername = view.FindViewById<EditText>(Resource.Id.txtUserName1);
            nTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            nBtnSignIn = view.FindViewById<Button>(Resource.Id.btnSignIn);
            nBtnSignIn.Click += NBtnSignIn_Click;

            return view;
        }

        

        private void NBtnSignIn_Click(object sender, EventArgs e)
        {
            WebClient login = new WebClient();
            Uri uri = new Uri("http://217.208.71.183/calendarusers/login.php");
            NameValueCollection parameters2 = new NameValueCollection();
            parameters2.Add("username", nTxtUsername.Text);
            parameters2.Add("password", nTxtPassword.Text);
            
            login.UploadValuesCompleted += Client_UploadLoginCompleted;
            login.UploadValuesAsync(uri, "POST", parameters2);
            
        }

        private void Client_UploadLoginCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
                    ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
                    ISharedPreferencesEditor edit = pref.Edit();
                    string json = Encoding.UTF8.GetString(e.Result);
                    echo = JsonConvert.DeserializeObject<string>(json);

                    Toast.MakeText(Activity, echo, ToastLength.Short).Show();

                    if (echo == "Login successful")
                    {

                        edit.PutString("Password", nTxtPassword.Text.Trim());
                        edit.PutString("Username", nTxtUsername.Text.Trim());
                        edit.Apply();


                        this.Dismiss();
                        Intent intent = new Intent(this.Activity, typeof(Activity1));
                        this.Activity.StartActivity(intent);
                        this.Activity.Finish();
                    }
        }

        

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;//Set the animation

        }
    }
}