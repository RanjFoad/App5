using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using System.Timers;

namespace AutoLock.Logics
{
    [Service]
    class LockService : Service
    {
        bool bolIsCreated, bolIsStarted;
        DateTime dtStartTime;
        long lngStartTime;
        long lngDelay;
        private Timer timer;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }



        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            //ISharedPreferences prefData = PreferenceManager.GetDefaultSharedPreferences(this);
            //ISharedPreferencesEditor editor = prefData.Edit();
            //prefData.GetBoolean("IsStarted", bolIsStarted);
            //if (bolIsStarted)
            //{
            //    prefData.GetLong("StartTime", lngStartTime);
            //    dtStartTime = DateTime.FromBinary(lngStartTime);
            //}
            //else
            //{

            //}
           
            lngDelay = intent.GetLongExtra("Duration", 0);
            timer = new Timer(lngDelay);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            Toast.MakeText(this, "Timer Started", ToastLength.Long).Show();
            return StartCommandResult.RedeliverIntent;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            //Toast.MakeText(this, "Period elapsed.", ToastLength.Long).Show();
            this.StopSelf();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Toast.MakeText(this, "Timer Stopped", ToastLength.Long).Show();
            timer.Dispose();
        }
    }
}