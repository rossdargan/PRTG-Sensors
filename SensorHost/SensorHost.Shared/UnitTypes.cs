using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorHost.Shared
{
    public enum UnitTypes
    {
        NotSet,

        BytesBandwidth,

        BytesMemory,

        BytesDisk,

        Temperature,

        Percent,

        TimeResponse,

        TimeSeconds,

        /// <summary>
        /// Specify the unit required in "CustomUnit"
        /// </summary>
        Custom,

        Count,

        CPU,

        BytesFile,

        SpeedDisk,

        SpeedNet,

        TimeHours
    }
}
