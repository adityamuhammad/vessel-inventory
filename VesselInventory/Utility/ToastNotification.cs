﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace VesselInventory.Utility
{
    public class ToastNotification
    {
        private static ToastNotification _instance;
        private Notifier _notifier;

        private ToastNotification()
        {
            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new PrimaryScreenPositionProvider(
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
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
            return _notifier;
        }
    }
}
