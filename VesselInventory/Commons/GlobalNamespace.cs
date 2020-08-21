namespace VesselInventory.Commons
{
    public static class GlobalNamespace
    {
        public static string ItemDimensionAlreadyExist => "Item and Dimension already exist.";
        public static string SuccessLogin => "Login successfully.";
        public static string DraftExists => "There is a Draft Document, you can use the document.";
        public static string CanOnlyCreateDocumentPerThreeDays => "You can only  create one document per three days.";
        public static string FailedLogin => "Failed login, username and password does not match.";
        public static string Error => "Oops, Something went wrong.\n";
        public static string QtyCannotBeZero => "Please provide the quantity.";
        public static string StockIsNotAvailable => "Stock is not available.";
        public static string WrongInput => "Make sure your input is correct.";
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
        public static string AttachmentPathLocation => @"C:\\VesselInventory\\Attachments\\";
        public static string AttachmentNotUploaded => "There is no attachment uploaded.";
        public static string AttachmentMissing => "The attachment is missing on local machine.";
    }
}
