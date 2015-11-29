namespace SensorHost
{
    using System;

    using Nancy.Hosting.Self;    

    /// <summary>
    /// Responsible for starting and stopping Nancy.
    /// </summary>
    public class NancySelfHost
    {
        /// <summary>
        /// The base url for the service.
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// The nancy host.
        /// </summary>
        private NancyHost _nancyHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="NancySelfHost"/> class.
        /// </summary>
        /// <param name="baseUrl">
        /// The base url.
        /// </param>
        public NancySelfHost(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Starts Nancy listening for requests
        /// </summary>
        public void Start()
        {
            _nancyHost = new NancyHost(new Uri(_baseUrl));
            _nancyHost.Start();
            Console.WriteLine($"PRTG service host is now listening - {_baseUrl}. Press ctrl-c to stop");
        }

        /// <summary>
        /// Stops Nancy from listening for requests.
        /// </summary>
        public void Stop()
        {
            _nancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}
