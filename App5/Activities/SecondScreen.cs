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

namespace App5.Activities
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