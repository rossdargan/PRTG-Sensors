namespace SensorHost
{
    using System;
    using System.Linq;

    using Nancy;

    using SensorHost.DTOs;
    using SensorHost.Factories;
    using SensorHost.Shared;

    public class WebModule : NancyModule
    {
        /// <summary>
        /// Used to serialize the DTO objects to a string
        /// </summary>
        private readonly Serializer.ISerializer _serializer;

        /// <summary>
        /// Creates a webmodule listening to the get requests for all the sensors.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="sensorFactory"></param>
        public WebModule(Serializer.ISerializer serializer, ISensorFactory sensorFactory)
        {
            _serializer = serializer;
            foreach (var sensor in sensorFactory.GetSensors())
            {
                string getPath = $"/{sensor.Name}";
                Console.WriteLine($"Listening for get on {getPath}");
                Get[getPath] = paramaters => QuerySensor(sensor);
            }
        }
        /// <summary>
        /// Queries the sensor to get the results, and then uses the serializer to convert it to a string.
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        private Response QuerySensor(ISensor sensor)
        {
            try
            {
                var reply = new Reply(sensor.Description, sensor.Results().ToList());
                return Response.AsText(_serializer.Serialize(reply), _serializer.ContentType);
            }
            catch (Exception err)
            {
                Console.WriteLine("ERROR WITH REQUEST!");
                Console.WriteLine(err.ToString());
                return 500;
            }
          
        }
    }
}
