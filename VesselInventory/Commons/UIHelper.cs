using System.Windows;

namespace VesselInventory.Commons
{
    public static class UIHelper
    {
        public static MessageBoxResult DialogConfirmation(string title, string description)
        {
            return MessageBox.Show(description,title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        }
    }
}
