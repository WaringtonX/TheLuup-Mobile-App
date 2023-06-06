using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Refractored.Controls;
using Square.Picasso;
using TheLuupApp.theluuprefrence1;
using static TheLuupApp.MainActivity;

namespace TheLuupApp
{
    [Activity(Label = "CreateMenu", Theme = "@style/AppTheme.NoActionBar", NoHistory = true)]
    public class CreateMenu : AppCompatActivity
    {
        private Service1 client1 = new Service1();
        private string UID;
        private string from;
        private FloatingActionButton floatActionButton;
        private TextInputLayout textInputLayout;
        private TextInputEditText NoteInputEditText;
        private TextInputEditText textInputEditText;
        private TextView text_view;
        private Android.Support.V7.Widget.Toolbar mToolbar;
        private static Random random = new Random();
        private Spinner spinner;
        private List<Restuarent> restuarents = new List<Restuarent>();
        private List<string> restuarentsNames = new List<string>();
        private string Res_ID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_createmenu);
            // Create your application here

            textInputLayout = FindViewById<TextInputLayout>(Resource.Id.nameTextinput);
            textInputEditText = FindViewById<TextInputEditText>(Resource.Id.MenuName);
            NoteInputEditText = FindViewById<TextInputEditText>(Resource.Id.MenuNote);
            text_view = FindViewById<TextView>(Resource.Id.text_view_time);
            floatActionButton = FindViewById<FloatingActionButton>(Resource.Id.fabcreteM);
            spinner = FindViewById<Spinner>(Resource.Id.spinner1);
            //ToolBar
            mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.cmenu_toobar);
            SetSupportActionBar(mToolbar);
            SupportActionBar.Title = "Create Menu";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }

            UID = Intent.GetStringExtra("user_id");

            LoadRestuarents();
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            /*var adapter = ArrayAdapter.CreateFromResource(
                    this, restuarentsNames, Android.Resource.Layout.SimpleSpinnerItem);*/
            var adapter = new ArrayAdapter<string>(this,Resource.Layout.spinner_item_selected, restuarentsNames);
            adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
            spinner.Adapter = adapter;

               textInputEditText.TextChanged += textname_TextChange;
               floatActionButton.Click += (sender, e) =>
                {
                    string carttCode = RandomString();
                    string name = textInputEditText.Text;
                    string note = NoteInputEditText.Text;
                    DateTime date = DateTime.UtcNow;
                    if ((name != null)&&(note != null)&&(Res_ID != null))
                    {
                        if (client1.AddCartCode(UID, Res_ID, carttCode, name,note,date,true) == "CartCode Added")
                        {
                            UpdateMyMenue();
                            Toast.MakeText(this, "Menu Successfuly Created!", ToastLength.Short).Show();
                            Finish();
                        }
                    }else
                    {
                        Toast.MakeText(this,"Please fill out and select all the required information !", ToastLength.Long).Show();
                    }
                };
          
           
        }

        private void textname_TextChange(object sender, TextChangedEventArgs e)
        {
            string textname = textInputEditText.Text;
            int texttyped = textname.Length;
            text_view.Text = texttyped + "/32";
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }


        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }

        public void LoadRestuarents()
        {
            restuarents.AddRange(client1.getResturants());
            foreach(Restuarent restuarent in restuarents)
            {
                restuarentsNames.Add(restuarent.Name);
            }
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            Res_ID = ""+ restuarents[e.Position].R_id;
            string toast = string.Format("The Language is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast + " Restuarent ID = " + Res_ID, ToastLength.Long).Show();
        }

        private static string RandomString()
        {
            int length = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}