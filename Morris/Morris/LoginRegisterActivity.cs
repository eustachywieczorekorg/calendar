using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using Android.Views.InputMethods;


namespace Morris
{
    [Activity(Label = "Login And Register")]
    public class LoginRegisterActivity : Activity
    {
        private Button nBtnSignUp;
        private Button nBtnSignIn;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            nBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            nBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);

            nBtnSignUp.Click += (object sender, EventArgs args) =>
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                dialog_Signup signUpDialog = new dialog_Signup();
                signUpDialog.Show(transaction, "dialog fragment");
            };

            nBtnSignIn.Click += (object sender, EventArgs args) =>
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                dialog_Signin signInDialog = new dialog_Signin();
                signInDialog.Show(transaction, "dialog fragment");
            };

        }
    }
}

