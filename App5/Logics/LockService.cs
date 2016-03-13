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
        DateTime dtStartTime;
        long lngStartTime;
        long lngDelay;
        private Timer timer;
        private long lngMiliseconds = 0;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }



        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
           
            lngDelay = intent.GetLongExtra("Duration", 0);
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            Toast.MakeText(this, Resources.GetString(Resource.String.TimerStarted), ToastLength.Long).Show();
            return StartCommandResult.RedeliverIntent;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lngMiliseconds+=1000;
            if (lngMiliseconds >= lngDelay)
            { 
            timer.Stop();
            DevicePolicyManager dpmDeviceLocker = (DevicePolicyManager)GetSystemService(Context.DevicePolicyService);
            dpmDeviceLocker.LockNow();
            //Toast.MakeText(this, "Period elapsed.", ToastLength.Long).Show();
            this.StopSelf();
            }
            else
            {
                long lngSecondsRemaining = lngDelay - lngMiliseconds;
                BroadCastMessage("RemainingSeconds", lngSecondsRemaining);

            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Toast.MakeText(this, "Timer Stopped", ToastLength.Long).Show();
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
            catch
            {

            }

        }
        private void BroadCastMessage(string Key, long Value)
        {
            Intent intBroadCast;
            try
            {
                intBroadCast = new Intent(Application.PackageName);
                intBroadCast.PutExtra(Key, Value);
                SendBroadcast(intBroadCast);
            }
            catch
            {

            }

        }
    }
}