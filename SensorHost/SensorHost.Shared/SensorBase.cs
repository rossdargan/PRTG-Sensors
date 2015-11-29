using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorHost.Shared
{
    /// <summary>
    /// A base class to help make a sensor easier.
    /// </summary>
    public abstract class SensorBase : ISensor
    {
        /// <summary>
        /// Gets the description of the sensor
        /// </summary>
        public virtual string Description { get; } = string.Empty;

        /// <summary>
        /// This is used by the host to decide on the URL to listen on. For example if the name is "testsensor" then
        /// the service will listen on "http://localhost:1234/testsensor"       
        /// This defaults to the objects type name.
        /// </summary>
        public virtual string Name
        {
            get { return this.GetType().Name; }
        }

        /// <summary>
        /// Used to return the results to the sensor host.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public abstract IEnumerable<Result> Results();                  
    }
}
