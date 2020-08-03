using System;
using System.ComponentModel;
using System.Reflection;

namespace VesselInventory.Commons.Enums
{
    public enum Status
    {
        [Description("DRAFT")]
        Draft,
        [Description("RELEASE")]
        Release
    }
    public enum SyncStatus
    {
        [Description("SYNC")]
        Sync,
        [Description("NOT SYNC")]
        Not_Sync
    }

    public enum ItemStatus
    {
        [Description("WAITING FOR SYNC")]
        Wait_Sync
    }


    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

}
