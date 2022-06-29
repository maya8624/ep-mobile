using Android.App;
using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Android.Droid.Services
{
    [BroadcastReceiver(Exported = true, Permission = "//receiver/@android:android.permission.SEND_SMS")]
    //[BroadcastReceiver(Exported = true)]
    public class SmsDeliveredReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string phone = intent.GetStringExtra("phone");
            switch ((int)ResultCode)
            {
                case (int)Result.Ok:
                    Toast.MakeText(Application.Context, "SMS Delivered" + phone + " success.", ToastLength.Short).Show();
                    break;
                case (int)Result.Canceled:
                    Toast.MakeText(Application.Context, "SMS not delivered", ToastLength.Short).Show();
                    break;
                default:
                    Toast.MakeText(Application.Context, ResultCode.ToString(), ToastLength.Short).Show();
                    break;
            }
        }
    }
}
