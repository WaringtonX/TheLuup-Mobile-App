using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp
{
    [Activity(Label = "TheLuup", Theme = "@style/AppTheme.NoActionBar")]
    public class Login : AppCompatActivity
    {
        private Service1 client1 = new Service1();
        private TextView create;
        private EditText email,password;
        private Button btnLogin;
        private string u_id;
        private string User_ID;
        private ProgressDialog mFacultyDialog;
        private LinearLayout rootView;
        private static string LUUP_USER_PRIVATE_USERID_DETAILS_PERSONAL= "LUUP_USER_PRIVATE_USERID_DETAILS_PERSONAL";
        private ISharedPreferences prefs;
        private ISharedPreferencesEditor editor;
        private TextView checkusename;
        private bool iswrongpasusername = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            rootView = FindViewById<LinearLayout>(Resource.Id.snackview);
            // Create your application here
            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }
            prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            editor = prefs.Edit();

            create = FindViewById<TextView>(Resource.Id.createaccount);
            email = FindViewById<EditText>(Resource.Id.Aemail_text);
            password = FindViewById<EditText>(Resource.Id.Apassword_text);
            btnLogin = FindViewById<Button>(Resource.Id.Abtlognadmin);
            checkusename = FindViewById<TextView>(Resource.Id.check_usepass);

            create.Click += (sender, e) =>
            {
                Intent log = new Intent(this, typeof(signup));
                StartActivity(log);
            };

            btnLogin.Click  += (sender, e) =>
            {
                string username = email.Text;
                string upassword = password.Text;

                Logins(username, upassword);
                if (iswrongpasusername)
                {
                    checkusename.Visibility = ViewStates.Visible;
                    checkusename.Text = "Username or password is incorrect!";
                }
            };
        }

        private void checkstate() {
            RunOnUiThread(() => {
                checkusename.Visibility = ViewStates.Visible;
                checkusename.Text = "Username or password is incorrect!";
            });
        }

        private void checkstateuser()
        {
            RunOnUiThread(() => {
                checkusename.Visibility = ViewStates.Visible;
                checkusename.Text = "Only Customers can access the app!";
            });
        }

        protected override void OnStart()
        {
            base.OnStart();
           /* ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            User_ID = prefs.GetString(LUUP_USER_PRIVATE_USERID_DETAILS_PERSONAL, null);

            if(User_ID != null)
            {
                Finish();
                Intent log = new Intent(this, typeof(MainActivity));
                log.PutExtra("user_id", User_ID);              
                StartActivity(log);
            }else
            {
                rootView.Visibility = ViewStates.Visible;
            } */
        }

        public async  void Logins(string username, string ps)
        {

            mFacultyDialog = new ProgressDialog(this);
            mFacultyDialog.SetTitle("Sigin in...");
            mFacultyDialog.SetMessage("please wait...");
            mFacultyDialog.Show();

            var thread = new System.Threading.Thread(new ThreadStart(delegate
            {


                if (client1.signinApp(username, ps) == "true")
                {
                        u_id = client1.getUserIDUsername(username);
                        int uid = Int32.Parse(u_id);
                        Users u = client1.getUser(uid, true);

                    if (u.Type == "Customer")
                    {
                        mFacultyDialog.Dismiss();
                       // checkusename.Visibility = ViewStates.Gone;
                        Finish();
                        Intent log = new Intent(this, typeof(MainActivity));
                        log.PutExtra("user_id", u_id);
                        editor.PutString(LUUP_USER_PRIVATE_USERID_DETAILS_PERSONAL, u_id);
                        editor.Apply();
                        StartActivity(log);

                    }
                    else
                    {
                        mFacultyDialog.Dismiss();
                        checkstateuser();
                       //checkusename.Text = "Only Customers can access the app!";

                    }



                }
                else
                {
                     iswrongpasusername = true;   
                     mFacultyDialog.Dismiss();
                     checkstate();
                    //Toast.MakeText(ApplicationContext, "Username or Password is Incorrect!", ToastLength.Long).Show();
                } 
         
            }));
            thread.Start();

            while (thread.ThreadState == ThreadState.Running)
            {
                await Task.Delay(1500);
            }

            mFacultyDialog.Dismiss();
        }

       
    }
}