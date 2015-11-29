namespace SensorHost.Shared
{
    using System.Collections.Generic;

    public interface ISensor
    {
        /// <summary>
        /// Gets the description of the sensor
        /// </summary>
        string Description { get; }

        /// <summary>
        /// This is used by the host to decide on the URL to listen on. For example if the name is "testsensor" then
        /// the service will listen on "http://localhost:1234/testsensor"
        /// </summary>
        string Name { get;}

        /// <summary>
        /// When queried you return the list of results (channels) using this method.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<Result> Results();
    }
}
