using ToastNotifications.Messages;
using VesselInventory.Utility;

namespace VesselInventory.Commons
{
    public static class ResponseMessage
    {
        public static void Success(string message)
        {
            ToastNotification.Instance.GetInstance().ShowSuccess(message);
        }

        public static void Error(string message)
        {
            ToastNotification.Instance.GetInstance().ShowError(message);
        }

        public static void Warning(string message)
        {
            ToastNotification.Instance.GetInstance().ShowWarning(message);
        }
        public static void Info(string message)
        {
            ToastNotification.Instance.GetInstance().ShowInformation(message);
        }
    }
}
