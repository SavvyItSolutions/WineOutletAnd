using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Gcm;
using Android.Util;
using Java.Util;
using System.Diagnostics;

namespace WineHangouts
{
    
    [Service (Exported = false), IntentFilter (new [] { "com.google.android.c2dm.intent.RECEIVE" })]
    public class MyGcmListenerService : GcmListenerService
    {
        public override void OnMessageReceived (string from, Bundle data)
        {
            // Extract the message received from GCM:
            var message = data.GetString("message");
            string WineBarcode = data.GetString("barcode");
            Log.Debug("MyGcmListenerService", "From:    " + from);
            Log.Debug("MyGcmListenerService", "Message: " + message);
            //var intent = new Intent(this, typeof(detailViewActivity));
            //intent.PutExtra("WineID", wineId);
            // Forward the received message in a local notification:
            SendNotification(message, WineBarcode);
        }

        // Use Notification Builder to create and launch the notification:
        void SendNotification(string message, string WineBarcode)
        {
            try { 
            Stopwatch st1=new Stopwatch();
            var intent = new Intent(this, typeof(DetailViewActivity));
            intent.PutExtra("WineBarcode", WineBarcode);
            intent.AddFlags(ActivityFlags.ClearTop);
            //System.TimeSpan ts = new System.TimeSpan((System.Int64)10e12+3456789);
            st1.Start();
            int requestid = st1.Elapsed.Milliseconds;

            var pendingIntent = PendingIntent.GetActivity(this, requestid, intent, PendingIntentFlags.OneShot);
                LoggingClass.LogInfo("Notification Sent\n" + intent+"Notification Sent\n" + pendingIntent, 1);
                var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.logo5)
                .SetContentTitle("Wine Outlet")
                .SetContentText(message)
                .SetAutoCancel(false)
                .SetContentIntent(pendingIntent);
            notificationBuilder.SetAutoCancel(true);
                //int notificationId = new Random().NextInt();
                // System.TimeSpan ts = new System.TimeSpan((System.Int64)10e12 + 3456789);
                LoggingClass.LogInfo("Notification Sent" + WineBarcode, 1);
                int notificationId = new Random().NextInt();
            var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(notificationId, notificationBuilder.Build());
            }catch(System.Exception ex) {
                LoggingClass.LogError(ex.Message, 1, ex.StackTrace);
            }
        }
       
    }
}
