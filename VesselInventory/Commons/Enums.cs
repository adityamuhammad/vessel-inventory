using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.Commons.Enums
{
    public enum Status
    {
        DRAFT,
        RELEASE
    }
    public enum SyncStatus
    {
        [Description("SYNC")]
        SYNC = 1,
        [Description("NOT SYNC")]
        NOT_SYNC = 0,
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
