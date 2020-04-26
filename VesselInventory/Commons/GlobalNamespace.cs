using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.Commons
{
    public static class GlobalNamespace
    {
        public static string ItemDimensionAlreadyExist => "Item and Dimension already exist.";
        public static string Error => "Oops, Something went wrong.";
        public static string ErrorSave => "Data failed to save.";
        public static string SuccessSave => "Data saved successfully.";
        public static string SuccessUpdate => "Data updated successfully.";
        public static string SuccessDelete => "Data deleted successfully.";
        public static string SuccesRelease => "Data released successfully.";
        public static string ReleaseConfirmation => "Release Confirmation";
        public static string ReleaseConfirmationDescription => "Are you sure want to release this data?";
        public static string DeleteConfirmation => "Delete Confirmation";
        public static string DeleteConfirmationDescription => "Are you sure want to delete this data?";
        public static string ShipDoesNotMatch => "Cannot proccess, The Ship is invalid.";
        public static string AttachmentPathLocation = @"C:\\VesselInventory\\Attachments\\";
    }
}
