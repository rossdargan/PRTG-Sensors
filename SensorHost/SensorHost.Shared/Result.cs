namespace SensorHost.Shared
{
    using System.Security.AccessControl;

    /// <summary>
    /// This represents the result of a single channel.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Ini
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        public Result(string channel, double value)
        {
            this.Channel = channel;
            this.Value = value;
        }

        /// <summary>
        /// Name of the channel as displayed in user interfaces. This parameter is required and must be unique for the sensor.
        /// </summary>
        public string Channel { get; set; }



        /// <summary>
        /// The unit of the value. Default is Custom. Useful for PRTG to be able to convert volumes and times.
        /// </summary>
        public UnitTypes Unit { get; set; }

        /// <summary>
        /// If Custom is used as unit this is the text displayed behind the value.
        /// </summary>
        public string CustomUnit { get; set; }

        /// <summary>
        /// Size used for the display value. E.g. if you have a value of 50000 and use Kilo as size the display is 50 kilo #. Default is One (value used as returned). For the Bytes and Speed units this is overridden by the setting in the user interface.
        /// </summary>
        public SensorSpeedSize SpeedSize { get; set; }

        /// <summary>
        /// Size used for the display value. E.g. if you have a value of 50000 and use Kilo as size the display is 50 kilo #. Default is One (value used as returned). For the Bytes and Speed units this is overridden by the setting in the user interface.
        /// </summary>
        public SensorSpeedSize VolumeSize { get; set; }

        /// <summary>
        /// See above, used when displaying the speed. Default is Second.
        /// </summary>
        public SpeedTime SpeedTime { get; set; }

        /// <summary>
        /// Selects if the value is a absolute value or counter. Default is Absolute.
        /// </summary>
        public SensorModes Mode
        {
            get; set;
        }

        /// <summary>
        /// The value as integer or float. Please make sure the setting matches the kind of value provided. Otherwise PRTG will show 0 values.
        /// </summary>
        public double Value { get; set; }



        /// <summary>
        /// Init value for the Show in Chart option. Default is true
        /// </summary>
        public bool? ShowChart { get; set; }
        /// <summary>
        /// Init value for the Show in Table option. Default is true
        /// </summary>
        public bool? ShowTable { get; set; }
        /// <summary>
        /// Maximum value before the error message is shown.
        /// </summary>
        public double LimitMaxError { get; set; }
        /// <summary>
        /// The error mesage to show if the limit is reached.
        /// </summary>
        public string LimitErrorMsg { get; set; }
        /// <summary>
        /// Should the error, and wanring limites be used? Default is false.
        /// </summary>
        public bool? LimitMode { get; set; } 
        /// <summary>
        /// The maximum value before the warning message is shown
        /// </summary>
        public double LimitMaxWarning { get; set; }
        /// <summary>
        /// The warning message to display.
        /// </summary>
        public string LimitWarningMsg { get; set; }

        /// <summary>
        /// if enabled in at least one channel, the entire sensor is set to warning status. Default isfalse.
        /// </summary>
        public bool? Warning { get; set; }

        /// <summary>
        /// Define if the value is a float. Default is false. If set to true, use a dot as decimal seperator in values. Note: In the sensor's Channels tab, you can define the number of decimal places shown in tables.
        /// </summary>
        public bool? Float { get; set; }

    }
}
