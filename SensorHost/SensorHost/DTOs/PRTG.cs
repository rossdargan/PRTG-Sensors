namespace SensorHost.DTOs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SensorHost.Extensions;
    using SensorHost.Shared;

    /// <summary>
    /// This is the Prtg DTO.
    /// </summary>
    public class Prtg
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Prtg"/> class.
        /// </summary>
        /// <param name="description">
        /// The description for the service
        /// </param>
        /// <param name="resultsList">
        /// The list of results
        /// </param>
        public Prtg(string description, List<Result> resultsList)
        {
            Text = description;
            Result = resultsList.ToArray();
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        public Result[] Result { get; }

        /// <summary>
        /// Gets the description for the sensor.
        /// </summary>
        public string Text { get; }

      
    }
}