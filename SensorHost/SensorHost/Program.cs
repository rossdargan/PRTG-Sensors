namespace SensorHost
{
    using System.Configuration;

    using Topshelf;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">
        /// The first arguments can be used to specify the base url.
        /// </param>
        public static void Main(string[] args)
        {
            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            
            HostFactory.Run(x =>                                    
            {
                x.Service<NancySelfHost>(s =>                       
                {
                    s.ConstructUsing(name => new NancySelfHost(baseUrl));  
                    s.WhenStarted(tc => tc.Start());               
                    s.WhenStopped(tc => tc.Stop());                
                });

                x.RunAsLocalSystem();                              
                x.SetDescription("A microservice to provide PRTG with information from the various sensors in the sensors folder");          
                x.SetDisplayName("PRTG Sensor Host");                         
                x.SetServiceName("SensorHost");                         
            });
        }        
    }
}
