
using Android.Telephony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ep.Android.Droid.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using ep.Mobile.Interfaces.IServices;

[assembly: Dependency(typeof(SmsService))]
namespace ep.Android.Droid.Services
{
    public class SmsService : ISmsService
    {
        public async Task SendMessageAsync(string mobile, string text)
        {
            //    var context = Android.App.Application.Context;
            //    var intent = new Intent("SEND_SMS");// Intent.ActionSend);
            //    var sentIntent = PendingIntent.GetBroadcast(context, 0, intent, 0);
            //    var permission = ContextCompat.CheckSelfPermission(context, Manifest.Permission.SendSms);

            //A PermissionException is thrown if the required permission is not declared.
            //Note, that on some platforms a permission request can only be activated a single time.
            //Further prompts must be handled by the developer to check
            //if a permission is in the Denied state and ask the user to manually turn it on.
            try
            {

                var status = await Permissions.CheckStatusAsync<Permissions.Sms>();
            
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Sms>();
                }

                if (status != PermissionStatus.Granted)
                {
                    return;
                }
                SmsManager.Default.SendTextMessage(mobile, null, text, null, null);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            //if (Permissions.ShouldShowRationale<Permissions.Sms>())
            //{

            //}

            //if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.SendSms) == Permission.Granted)
            //{
            //}
            //else
            //{
                
            //    var activity = new MainActivity();
            //    //ActivityCompat..RequestPermissions(activity, new string[] { Manifest.Permission.SendSms }, 1);
            //}
                //intent.PutExtra("phone", phone);
        }
    }
}
