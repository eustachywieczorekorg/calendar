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

namespace Morris
{

    public class dialog_Signup : DialogFragment
    {
        private EditText nTxtUserName;
        private EditText nTxtPassword;
        private Button nBtnSignUp;
        public string ToastMessage;

        public object Jsonconvert { get; private set; }

        //public event EventHandler<OnSignUpEventArgs> nOnSignUpComplete;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_sign_up, container, false);

            nTxtUserName = view.FindViewById<EditText>(Resource.Id.txtUsername);
            nTxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            nBtnSignUp = view.FindViewById<Button>(Resource.Id.btnDialogEmail);
            nBtnSignUp.Click += NBtnSignUp_Click;

            return view;
        }
        private void NBtnSignUp_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("http://217.208.71.183/calendarusers/Register.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", nTxtUserName.Text);
            parameters.Add("password", nTxtPassword.Text);
            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(uri, "POST", parameters);
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            
            Activity.RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                ToastMessage = JsonConvert.DeserializeObject<string>(json);
                Toast.MakeText(this.Activity, ToastMessage, ToastLength.Short).Show();
                if (ToastMessage == "Account created, welcome")
                {
                    this.Dismiss();
                }
            });
        }
        
        

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;//Set the animation
        }
    }
}