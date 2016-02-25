using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Content.Res;
using Java.Util;
using AutoLock.Logics;
using Android.Util;
using Android.App.Admin;
using AutoLock.Activities;

namespace AutoLock
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        private SeekBar sbSetPeriod;
        private int int_MinPeriod, int_MaxPeriod;
        private TextView txt_Progress;
        string Lang;
        private BroadcastReceiver brReciever;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Lang = ReadPresestingString("Lang");
            SetInterfaceLocal(Lang, this.BaseContext);



            int_MaxPeriod = Resources.GetInteger(Resource.Integer.maxPeriod);
            int_MinPeriod = Resources.GetInteger(Resource.Integer.minPeriod);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Button button = FindViewById<Button>(Resource.Id.btnStart);
            button.Click += Start_Button_Click;
            if (isServiceRunning())
            {
                button.Enabled = false;
                //brReciever = new BroadcastReceiver();


            }
            txt_Progress = FindViewById<TextView>(Resource.Id.tvMinutes);
            sbSetPeriod = FindViewById<SeekBar>(Resource.Id.sbSelectMinutes);
            sbSetPeriod.ProgressChanged += SbSetPeriod_ProgressChanged;
            sbSetPeriod.Max = int_MaxPeriod - int_MinPeriod;

            DevicePolicyManager devicePolicyManager = (DevicePolicyManager)GetSystemService(Context.DevicePolicyService);
            ComponentName cnDeviceAdmin = new ComponentName(this, Java.Lang.Class.FromType(typeof(DeviceAdmin)));
            Intent intent = new Intent(DevicePolicyManager.ActionAddDeviceAdmin);
            intent.PutExtra(DevicePolicyManager.ExtraDeviceAdmin, cnDeviceAdmin);
            intent.PutExtra(DevicePolicyManager.ExtraAddExplanation, "To enable autolock functionality.");
            StartActivity(intent);

        }

        private void Start_Button_Click(object sender, EventArgs e)
        {
            try
            {
                if(isServiceRunning())
                {
                    Toast.MakeText(this, "Timer already running.", ToastLength.Long);
                    return;
                }
                DateTime dtNow = DateTime.Now;
                long lngDelay = (sbSetPeriod.Progress + int_MinPeriod) * 60000;
                SavePresestingLong("StartTime",dtNow.ToBinary());
                SavePresestingLong("Duration", lngDelay);
                Intent intService = new Intent(this, typeof(LockService));
                intService.PutExtra("StartTime", dtNow.ToBinary());
                intService.PutExtra("Duration", lngDelay);
                StartService(intService);
                this.Finish();

            }
            catch
            {
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            int SelectedItem = item.ItemId;
            switch (SelectedItem)
            {
                case Resource.Id.IdEnglish:
                    if (Lang == "en") break;
                    SetInterfaceLocal("en", this.BaseContext);
                    SavePresestingString("Lang","en");
                    RestartActivity();
                    break;

                case Resource.Id.IdKurdish:
                    if (Lang == "ku") break;
                    SetInterfaceLocal("ku", this.BaseContext);
                    SavePresestingString("Lang", "ku");
                    RestartActivity();
                    break;

                case Resource.Id.IdArabic:
                    if (Lang == "ar") break;
                    SetInterfaceLocal("ar", this.BaseContext);
                    SavePresestingString("Lang", "ar");
                    RestartActivity();
                    break;
                case Resource.Id.help:
                    Intent intAbout = new Intent(this, typeof(About));
                    StartActivity(intAbout);
                    break;
                case Resource.Id.exit:
                    this.Finish();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            if (Lang == "ku")
                menu.FindItem(Resource.Id.IdKurdish).SetChecked( true);
            else
                menu.FindItem(Resource.Id.IdEnglish).SetChecked(true);
            return base.OnCreateOptionsMenu(menu);
        }
        private void SbSetPeriod_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            string strMinute, strMinutes;
            strMinute = Resources.GetString(Resource.String.minute);
            strMinutes = Resources.GetString(Resource.String.minutes);
            int int_Pos = sbSetPeriod.Progress + int_MinPeriod;
            string strMinutesLable = int_Pos == 1 ? strMinute : strMinutes;
            txt_Progress.Text = int_Pos.ToString()+ " " + strMinutesLable;

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
        public bool SavePresestingString(string Key, string Value)
        {
            try
            {
                ISharedPreferences settings = this.GetSharedPreferences("AutoLock", FileCreationMode.Private);
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString(Key,Value);
                editor.Commit();
                return true;
            }

            catch
            {
                return false;
            }
        }
        public string ReadPresestingString(string Key)
        {
            try
            {
                ISharedPreferences settings = this.GetSharedPreferences("AutoLock", FileCreationMode.Private);
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                return prefs.GetString(Key, "");
            }

            catch 
            {
                return null;
            }
        }
        public bool SavePresestingLong(string Key, long Value)
        {
            try
            {
                ISharedPreferences settings = this.GetSharedPreferences("AutoLock", FileCreationMode.Private);
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutLong(Key, Value);
                editor.Commit();
                return true;
            }

            catch 
            {
                return false;
            }
        }
        public long ReadPresestingLong(string Key)
        {
            try
            {
                ISharedPreferences settings = this.GetSharedPreferences("AutoLock", FileCreationMode.Private);
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                return prefs.GetLong(Key, 0);
            }

            catch 
            {
                return 0;
            }
        }
        public void RestartActivity()
        {
            Intent intThisApp= new Intent(this,this.Class);
            this.Finish();
            StartActivity(intThisApp);

        }
        private bool isServiceRunning()
        {
            ActivityManager manager = (ActivityManager)GetSystemService(ActivityService);
            foreach (ActivityManager.RunningServiceInfo service in manager.GetRunningServices(int.MaxValue))
            {
                Log.WriteLine(LogPriority.Debug, "service name", service.Service.PackageName);
                if ("Auto_Lock.Auto_Lock" == (service.Service.PackageName)) {
                return true;
            }
        }
  return false;
}
}
}


