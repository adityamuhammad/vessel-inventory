using System.Windows;

namespace VesselInventory.Commons
{
    public static class UIHelper
    {
        public static MessageBoxResult DialogConfirmation(string title, string description)
        {
            return MessageBox.Show(title, description, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        }
    }
}
