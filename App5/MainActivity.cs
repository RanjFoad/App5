using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using App5.Activities;
using System.Windows;
using Android.App.Admin;
using System.Resources;
using Android.Content.Res;
using Android.Util;
using Java.Util;

namespace App5
{
    [Activity(Label = "App5", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        private SeekBar sbSetPeriod;
        private int int_MinPeriod, int_MaxPeriod;
        private TextView txt_Progress;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetInterfaceLocal("ku", this.BaseContext);



            int_MaxPeriod = Resources.GetInteger(Resource.Integer.maxPeriod);
            int_MinPeriod = Resources.GetInteger(Resource.Integer.minPeriod);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            txt_Progress = FindViewById<TextView>(Resource.Id.textView1);
            sbSetPeriod = FindViewById<SeekBar>(Resource.Id.seekBar1);
            sbSetPeriod.ProgressChanged += SbSetPeriod_ProgressChanged;
            
            sbSetPeriod.Max = int_MaxPeriod- int_MinPeriod;

            //DevicePolicyManager devicePolicyManager = (DevicePolicyManager)GetSystemService(Context.DevicePolicyService);
            //ComponentName demoDeviceAdmin = new ComponentName(this, Java.Lang.Class.FromType(typeof(DeviceAdmin)));
            //Intent intent = new Intent(DevicePolicyManager.ActionAddDeviceAdmin);
            //intent.PutExtra(DevicePolicyManager.ExtraDeviceAdmin, demoDeviceAdmin);
            //intent.PutExtra(DevicePolicyManager.ExtraAddExplanation, "Device administrator");
            //StartActivity(intent);
            // Get our button from the layout resource,
            // and attach an event to it


            button.Click += delegate
            {
                //PopupMenu menu = new PopupMenu(this, button);
                //menu.MenuItemClick += Menu_MenuItemClick;
                //menu.Inflate(Resource.Menu.MainMenu);
                //menu.Show();
                //devicePolicyManager.LockNow();

                //SetContentView(Resource.Layout.Main);


                //var builder = new AlertDialog.Builder(this);
                //builder.SetMessage("Increased");
                //builder.SetPositiveButton("OK", (s, e) => { /* do something on OK click */ });

                //builder.Create().Show();
                Intent int1 = new Intent(this, typeof(SecondScreen));
                StartActivity(int1);

            };


            //private void Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
            //{
            //    int SelectedItem = e.Item.ItemId;
            //    switch (SelectedItem)
            //        {
            //        case Resource.Id.new_game:

            //            break;
            //        case Resource.Id.exit:
            //            this.Finish();
            //            break;
            //    }


            //}

        }

        private void SbSetPeriod_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            string strMinute, strMinutes;
            strMinute = Resources.GetString(Resource.String.minute);
            strMinutes = Resources.GetString(Resource.String.minutes);
            int int_Pos = sbSetPeriod.Progress + int_MinPeriod;
            string strMinutesLable = int_Pos == 1 ? strMinute : strMinute;
            txt_Progress.Text = int_Pos.ToString()+ " " + strMinutes;
        }

        private void SetInterfaceLocal(string LocaleName,Context ApplyContext)
        {
            Resources res = ApplyContext.Resources;
            Locale lclKurdish = new Locale(LocaleName);
            Locale.Default = lclKurdish;
            Configuration conf = res.Configuration;
            conf.Locale = lclKurdish;
            ApplyContext.Resources.UpdateConfiguration(conf, ApplyContext.Resources.DisplayMetrics);
        }
    }
}

