namespace SensorHost.DTOs
{
    using System.Collections.Generic;

    using SensorHost.Shared;

    /// <summary>
    /// The DTO object containing the full response for PRTG.
    /// </summary>
    public class Reply
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reply"/> class.
        /// </summary>
        /// <param name="description">The description for the sensor</param>
        /// <param name="resultsList">
        /// The results list.
        /// </param>
        public Reply(string description, List<Result> resultsList)
        {
            Prtg = new Prtg(description, resultsList);
        }

        /// <summary>
        /// Gets the prtg DTO object.
        /// </summary>
        public Prtg Prtg { get; }
    }
}
