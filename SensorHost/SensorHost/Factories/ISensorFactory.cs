namespace SensorHost.Factories
{
    using System.Collections.Generic;

    using SensorHost.Shared;

    /// <summary>
    /// Provides the core service with a list of sensors to report upon.
    /// </summary>
    public interface ISensorFactory
    {
        /// <summary>
        /// Returns all the sensors to be monitored
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     of sensors to monitor.
        /// </returns>
        IEnumerable<ISensor> GetSensors();
    }
}
