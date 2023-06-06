using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using Refractored.Controls;

namespace TheLuupApp
{
    [Activity(Label = "TheLuup", MainLauncher = true)]
    public class Splash_Activity : Activity
    {
        private ImageView mImageView;
        private static int SPLASH_TIME_OUT = 2000;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_splash);
            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor("#000000"));
            }
            System.Threading.ThreadPool.QueueUserWorkItem(o => LoadActivity());
            mImageView = FindViewById<ImageView>(Resource.Id.splashimage);
            Animation animation = AnimationUtils.LoadAnimation(this, Resource.Animation.bounce_new);
            mImageView.StartAnimation(animation);

            // Create your application here

            /* Handler h = new Handler();
             Action myAction = () =>
             {
                 // your code that you want to delay here
                 Finish();
                 System.Threading.Thread.Sleep(8000);
                 Intent log = new Intent(this, typeof(MainActivity));
                 StartActivity(log);
             };

             h.PostDelayed(myAction, SPLASH_TIME_OUT); */
        }

        public void LoadActivity()
        {
            System.Threading.Thread.Sleep(2000); // Simulate a long pause    
            RunOnUiThread(() => StartActivity(typeof(MainActivity)));
            Finish();
        }
    }
}