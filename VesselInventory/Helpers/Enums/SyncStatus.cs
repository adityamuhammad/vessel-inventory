using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.Helpers.Enums
{
    public enum SyncStatus
    {
        [Description("SYNC")]
        SYNC,
        [Description("NOT SYNC")]
        NOT_SYNC
    }
}
