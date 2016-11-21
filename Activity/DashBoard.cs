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
using Android.Support.V7.App;
using Firebase.Auth;
using static Android.Views.View;
using Android.Gms.Tasks;
using Android.Support.Design.Widget;

namespace XamarinFirebaseAuth
{
    [Activity(Label = "DashBoard",Theme ="@style/AppTheme")]
    public class DashBoard : AppCompatActivity,IOnClickListener,IOnCompleteListener
    {
        TextView txtWelcome;
        EditText input_new_password;
        Button btnChangePass, btnLogout;
        RelativeLayout activity_dashboard;

        FirebaseAuth auth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DashBoard);

            //Init Firebase
            auth = FirebaseAuth.GetInstance(MainActivity.app);

            //View
            txtWelcome = FindViewById<TextView>(Resource.Id.dashboard_welcome);
            input_new_password = FindViewById<EditText>(Resource.Id.dashboard_newpassword);
            btnChangePass = FindViewById<Button>(Resource.Id.dashboard_btn_change_pass);
            btnLogout = FindViewById<Button>(Resource.Id.dashboard_btn_logout);
            activity_dashboard = FindViewById<RelativeLayout>(Resource.Id.activity_dashboard);

            btnChangePass.SetOnClickListener(this);
            btnLogout.SetOnClickListener(this);

            //Check session
            if (auth.CurrentUser != null)
                txtWelcome.Text = "Welcome , " + auth.CurrentUser.Email;
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.dashboard_btn_change_pass)
                ChangePassword(input_new_password.Text);
            else if (v.Id == Resource.Id.dashboard_btn_logout)
                LogoutUser();
        }

        private void LogoutUser()
        {
            auth.SignOut();
            if(auth.CurrentUser == null)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                Finish();
            }
        }

        private void ChangePassword(string newPassword)
        {
            FirebaseUser user = auth.CurrentUser;
            user.UpdatePassword(newPassword)
                .AddOnCompleteListener(this);
        }

        public void OnComplete(Task task)
        {
           if(task.IsSuccessful == true)
            {
                Snackbar snackBar = Snackbar.Make(activity_dashboard, "Password has been changed", Snackbar.LengthShort);
                snackBar.Show();
            }
        }
    }
}