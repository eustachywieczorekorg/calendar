using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using Android.Views.InputMethods;
using Android.Support.V7.App;

namespace Morris
{
    [Activity(Label = "Login And Register", Theme = "@style/MyTheme")]
    public class LoginRegisterActivity : AppCompatActivity
    {
        private Button nBtnSignUp;
        private Button nBtnSignIn;
        private Android.Support.V7.Widget.Toolbar mToolbar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.logintoolbar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Welcome to Morris EC, visit www.digestivetech.com to browse our other products and current projects";

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
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }
    }
}

