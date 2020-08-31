using System;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace VesselInventory.Utility
{
    public class ToastNotification
    {
        private static ToastNotification _instance;

        private ToastNotification()
        {
        }

        public static ToastNotification Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new ToastNotification();
                return _instance = new ToastNotification();
            }
        }

        public Notifier GetInstance()
        {
           var _notifier = new Notifier(cfg => {
                cfg.PositionProvider = new PrimaryScreenPositionProvider(
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
           });
           using (_notifier) return _notifier;
        }
    }
}
