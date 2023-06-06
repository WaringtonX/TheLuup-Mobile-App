using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using TheLuupApp.theluuprefrence1;

namespace TheLuupApp
{
    [Activity(Label = "Create account", Theme = "@style/AppTheme.NoActionBar")]
    public class signup : Activity
    {
        private EditText name, lastname,email,password,age,username;
        private TextView checkUsername,checkfill;
        private ProgressDialog mFacultyDialog;
        private LinearLayout rootView;
        private Button btnSignuo;
        private Service1 client1 = new Service1();
        private Spinner spinner;
        private string[] genertypes = { "Male", "Female" };
        private string genders = "Male";
        public static List<Users> users;
        private List<string> unames = new List<string>();
        public bool doesuerexist = false;
        public bool islengthcorrect = false;
        private Runnable uiThread;
        private Thread checkThread;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signup);
            rootView = FindViewById<LinearLayout>(Resource.Id.snack2);
            // Create your application here

            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }

            name = FindViewById<EditText>(Resource.Id.first_name);
            lastname = FindViewById<EditText>(Resource.Id.last_name);
            email = FindViewById<EditText>(Resource.Id.student_ID);
            password = FindViewById<EditText>(Resource.Id.student_number);
            btnSignuo = FindViewById<Button>(Resource.Id.btnCreate);
            age = FindViewById<EditText>(Resource.Id.f_age);
            spinner = FindViewById<Spinner>(Resource.Id.genderspinner1);
            username = FindViewById<EditText>(Resource.Id.u_username);
            checkUsername = FindViewById<TextView>(Resource.Id.check_username);
            checkfill = FindViewById<TextView>(Resource.Id.check_fill);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            /*var adapter = ArrayAdapter.CreateFromResource(
                    this, restuarentsNames, Android.Resource.Layout.SimpleSpinnerItem);*/
            var adapter = new ArrayAdapter<string>(this, Resource.Layout.spinner_item_selected, genertypes);
            adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
            spinner.Adapter = adapter;
            users = new List<Users>();

            LoadZUsers();

            username.TextChanged += username_TextChanged;

            uiThread = new Runnable(new Action(delegate {
                while (true)
                {
                  
                    Thread.Sleep(1000);

                }
            }));
            checkThread = new Thread(uiThread);
            checkThread.Start();

            btnSignuo.Click += (sender, e) =>
            {
                string umail = email.Text.ToString();
                string upassword = password.Text.ToString();
                string fname = name.Text.ToString();
                string lname = lastname.Text.ToString();
                string ages = age.Text.ToString();
                int u_age = 0;
                if (ages.Equals(""))
                {
                    //int u_age = Int32.Parse(ages);
                }else
                {
                    u_age = Int32.Parse(ages);
                }

                string uname = username.Text.ToString();
                if (uname.Equals("")||umail.Equals("")||upassword.Equals("")||fname.Equals("")||lname.Equals("")||ages.Equals("")||ages.Equals(""))
                {
                    checkfill.Visibility = ViewStates.Visible;
                
                }
                else
                {
                   if (doesuerexist)
                    {

                    }
                    else
                    {
                        if (islengthcorrect)
                        {
                            checkfill.Visibility = ViewStates.Gone;
                            Signup_User(fname, lname, umail, upassword, u_age, uname);
                        }

                    } 
                }
               
            };
        }

        private void LoadZUsers() {
            users.AddRange(client1.getCustomerUsers());
            List<string> mynames = new List<string>();
            foreach (Users u in users)
            {
                mynames.Add(u.Username.ToLower());

            }
            unames = mynames;
        }
        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            string currentText = username.Text.ToLower(); ;
            if (unames.Contains(currentText))
            {
                checkUsername.Visibility = ViewStates.Visible;
                checkUsername.Text = "username alredy exist please try a diffferent one";
                doesuerexist = true;
            }
            else
            {
                checkUsername.Visibility = ViewStates.Gone;
                doesuerexist = false;
            }
            if (!doesuerexist)
            {
                int sizeOfString = currentText.Length;
                if(sizeOfString < 6)
                {
                    checkUsername.Visibility = ViewStates.Visible;
                    checkUsername.Text = "username must contain atleats 6 characters";
                    islengthcorrect = false;
                }
                else
                {
                    checkUsername.Visibility = ViewStates.Gone;
                    islengthcorrect = true;
                }
            }
           
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            genders = genertypes[e.Position];       
        }


        private async void Signup_User(string fname, string lname, string umail, string upassword,int age,string uname)
        {
            string genderd = genders;
            mFacultyDialog = new ProgressDialog(this);
            mFacultyDialog.SetTitle("Creating account...");
            mFacultyDialog.SetMessage("please wait...");
            mFacultyDialog.Show();

            var thread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                if (client1.AddUser(umail, upassword, fname,lname, age, true, genderd, "default.png","Customer","Offline", uname) == "User Added")
                {
                    Finish();
                    Intent log = new Intent(this, typeof(Login));
                    StartActivity(log);

                    Snackbar snackBar = Snackbar.Make(rootView, "Account Successfully Created!", Snackbar.LengthLong);
                    snackBar.SetAction("Ok", (v) =>
                    {

                    });
                    snackBar.Show();

                    mFacultyDialog.Dismiss();
                }
                else
                {
                    Snackbar snackBar = Snackbar.Make(rootView, "There Was a Problem with Creating an Account!", Snackbar.LengthLong);
                    snackBar.SetAction("Ok", (v) =>
                    {

                    });
                    snackBar.Show();
                    //Toast.MakeText(ApplicationContext, "Username or Password is Incorrect!", ToastLength.Long).Show();
                }
            }));
            thread.Start();

            while (thread.ThreadState == System.Threading.ThreadState.Running)
            {
                await System.Threading.Tasks.Task.Delay(1500);
            }

            mFacultyDialog.Dismiss();
        }
    }
}