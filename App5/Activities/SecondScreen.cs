using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace AutoLock.Activities
{
    [Activity(Label = "SecondScreen")]
    public class SecondScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SecondScreen);
            Button btn = (Button)FindViewById(Resource.Id.ExitScreen);
            btn.Click += Btn_Click;

            // Create your application here
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            this.Finish();
        }
    }
}