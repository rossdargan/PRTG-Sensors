

namespace SensorHost.Factories
{
    using System.IO;
    using System.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SensorHost.Shared;

    using UpsSensor;

    public class SensorFactory: ISensorFactory
    {
        private static readonly Lazy<IEnumerable<ISensor>> Sensors = new Lazy<IEnumerable<ISensor>>(InitSensors);

        private static IEnumerable<ISensor> InitSensors()
        {
            Console.WriteLine("Loading Sensors");
            List<ISensor> sensors = new List<ISensor>();
            sensors.Add(new UPSSensor());
            string exeLocation = Assembly.GetEntryAssembly().Location;
            string directory = Path.GetDirectoryName(exeLocation);
            string sensorsLocation = $"{directory}\\Sensors";
            var files = Directory.EnumerateFiles(sensorsLocation, "*.dll");

            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(file);
                foreach (Type type in assembly.GetExportedTypes())
                {
                    bool isSensor = type.GetInterfaces().Any(inter => typeof(ISensor).IsAssignableFrom(inter));
                    if (isSensor)
                    {
                        ISensor sensor = Activator.CreateInstance(type) as ISensor;

                        if (sensor != null)
                        {
                            sensors.Add(sensor);
                            Console.WriteLine($"Found {sensor.Name}");
                        }
                    }
                }
            }
            return sensors;
        }

        public IEnumerable<ISensor> GetSensors()
        {
            return Sensors.Value;
        }
    }
}
