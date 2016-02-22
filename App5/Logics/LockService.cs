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
using Android.Preferences;

namespace App1.Code
{
    [Service]
    class LockService : Service
    {
        bool bolIsCreated, bolIsStarted;
        DateTime dtStartTime;
        long lngStartTime;
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
            Toast.MakeText(this, "Service Started", ToastLength.Long).Show();
            return StartCommandResult.RedeliverIntent;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Toast.MakeText(this, "Service Stopped", ToastLength.Long).Show();
        }
    }
}