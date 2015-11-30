

namespace SensorHost.Factories
{
    using System.IO;
    using System.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.ObjectBuilder2;

    using SensorHost.Shared;


    public class SensorFactory: ISensorFactory
    {
        private static readonly Lazy<IEnumerable<ISensor>> Sensors = new Lazy<IEnumerable<ISensor>>(InitSensors);

        private static IEnumerable<ISensor> InitSensors()
        {
            Console.WriteLine("Loading Sensors");
            List<ISensor> sensors = new List<ISensor>();
            string exeLocation = Assembly.GetEntryAssembly().Location;
            string directory = Path.GetDirectoryName(exeLocation);
            string sensorsLocation = $"{directory}\\Sensors";
            var files = Directory.EnumerateFiles(sensorsLocation, "*.dll");
            var assemblies = LoadAssemblies(files);
            foreach (var assembly in assemblies)
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.IsAbstract)
                    {
                        continue;
                    }

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

        private static List<Assembly> LoadAssemblies(IEnumerable<string> files)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p=>p.IsDynamic==false).ToList();                       
            foreach (var file in files)
            {
                try
                {
                    var assemblyName = AssemblyName.GetAssemblyName(file);
                    loadedAssemblies.Add(AppDomain.CurrentDomain.Load(assemblyName));
                }
                catch (BadImageFormatException error)
                {
                             
                }
                
            }
            return loadedAssemblies;
        }

       

        public IEnumerable<ISensor> GetSensors()
        {
            return Sensors.Value;
        }
    }
}
