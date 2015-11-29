using System;
using System.Collections.Generic;
using SensorHost.Shared;

namespace TestSensor
{
    public class TestSensor : SensorBase
    {
        public override string Description
        {
            get { return "Demo values. OS: %OS%"; }
        }

        public override IEnumerable<Result> Results()
        {
            return new[]
            {
                new Result("Demo Minimum Example", 3),
                new Result("Demo Disk Free", 38.4487)
                {
                    Unit = UnitTypes.Percent,
                    Mode = SensorModes.Absolute,
                    ShowChart = true,
                    ShowTable = true,
                    Warning = false,
                    Float = true,
                    LimitMaxError = 80,
                    LimitMaxWarning = 37,
                    LimitWarningMsg = "My custom note for errors",
                    LimitErrorMsg = "My custom note for errors",
                    LimitMode = true
                },
                new Result("Demo Network Speed", 124487000)
                {
                    Unit = UnitTypes.SpeedNet,
                    VolumeSize = SensorSpeedSize.MegaBit,
                    Mode = SensorModes.Absolute,
                    ShowChart = true,
                    ShowTable = true,
                    Warning = false,
                    Float = false,
                },
                new Result("Demo Custom", 855)
                {
                    Unit = UnitTypes.Custom,
                    CustomUnit = "Pieces",
                    Mode = SensorModes.Absolute,
                    ShowChart = true,
                    ShowTable = true,
                    Warning = false,
                    Float = false
                }
            };
        }
    }
}
