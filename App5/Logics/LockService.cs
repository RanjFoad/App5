using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using System.Timers;
using Android.App.Admin;

namespace AutoLock.Logics
{
    [Service]
    class LockService : Service
    {
        DateTime dtEndTime;
        double dblDelay;
        private Timer timer;
        private long lngMiliseconds = 0;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        //[return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            try {
                dblDelay = intent.GetDoubleExtra("Duration", 0);
                long lngStartTime = intent.GetLongExtra("StartTime", 0);
                dtEndTime = DateTime.FromBinary(lngStartTime).AddMilliseconds(dblDelay);
                timer = new Timer(1000);
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;
                Toast.MakeText(this, Resources.GetString(Resource.String.TimerStarted), ToastLength.Long).Show();
                return StartCommandResult.RedeliverIntent;
            }
            catch(Exception ex)
            {
                Toast.MakeText(this, "Error:" + ex.Message, ToastLength.Long).Show();
                return StartCommandResult.RedeliverIntent;
            }
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try {
                DateTime dtNow = DateTime.Now;
                lngMiliseconds += 1000;
                if (dtNow >= dtEndTime)
                {
                    timer.Stop();
                    DevicePolicyManager dpmDeviceLocker = (DevicePolicyManager)GetSystemService(Context.DevicePolicyService);
                    dpmDeviceLocker.LockNow();
                    this.StopSelf();
                }
                else
                {
                    double lngSecondsRemaining = (dtEndTime - dtNow).TotalMilliseconds;
                    BroadCastMessage("RemainingSeconds", lngSecondsRemaining);
                }
            }
            catch(Exception ex)
            { Toast.MakeText(this, "Error:" + ex.Message, ToastLength.Long).Show(); }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            timer.Dispose();
        }
        private void BroadCastMessage(string Key, string Value)
        {
            Intent intBroadCast;
            try
            {
                intBroadCast = new Intent(Application.PackageName);
                intBroadCast.PutExtra(Key,Value);
                SendBroadcast(intBroadCast);
            }
            catch (Exception ex)
            { Toast.MakeText(this, "Error:" + ex.Message, ToastLength.Long).Show(); }

        }
        private void BroadCastMessage(string Key, double Value)
        {
            Intent intBroadCast;
            try
            {
                intBroadCast = new Intent(Application.PackageName);
                intBroadCast.PutExtra(Key, Value);
                SendBroadcast(intBroadCast);
            }
            catch (Exception ex)
            { Toast.MakeText(this, "Error:" + ex.Message, ToastLength.Long).Show(); }

        }
    }
}