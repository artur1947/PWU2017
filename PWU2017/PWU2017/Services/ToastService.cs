using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PWU2017.Services
{
    class ToastService
    {
        public void ShowToast(Models.FileInfo file, string message = "Success")
        {
            var image = "https://www.funnypica.com/wp-content/uploads/2015/05/Funny-Cow-Pictures-2-570x499.jpg";

            var content = new NotificationsExtensions.Toasts.ToastContent()
            {
                Launch = file.Ref.Path,
                Visual = new NotificationsExtensions.Toasts.ToastVisual()
                {
                    TitleText = new NotificationsExtensions.Toasts.ToastText()
                    {
                        Text = message
                    },

                    BodyTextLine1 = new NotificationsExtensions.Toasts.ToastText()
                    {
                        Text = file.Name
                    },

                    AppLogoOverride = new NotificationsExtensions.Toasts.ToastAppLogo()
                    {
                        Crop = NotificationsExtensions.Toasts.ToastImageCrop.Circle,
                        Source = new NotificationsExtensions.Toasts.ToastImageSource(image)
                    }
                },
                Audio = new NotificationsExtensions.Toasts.ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.IM")
                }
            };

            var notification = new Windows.UI.Notifications.ToastNotification(content.GetXml());
            var notifier = Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier();
            notifier.Show(notification);
        }
    }
}
