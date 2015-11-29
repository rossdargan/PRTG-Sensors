namespace SensorHost.Serializer
{
    using SensorHost.DTOs;

    /// <summary>
    /// The Serializer interface.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Gets the content type of the conversion.
        /// </summary>
        string ContentType { get;  }

        /// <summary>
        /// Serialize a Reply DTO into a string.
        /// </summary>
        /// <param name="reply">
        /// The reply DTO object.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string Serialize(Reply reply);
    }
}
