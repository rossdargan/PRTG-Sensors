namespace SensorHost
{
    using Microsoft.Practices.Unity;

    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Bootstrappers.Unity;

    using SensorHost.Factories;

    /// <summary>
    /// The bootstrapper for Nancy.
    /// </summary>
    public class Bootstrapper : UnityNancyBootstrapper
    {
        /// <summary>
        /// No registrations should be performed in here, however you may resolve things that are needed during application startup.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="pipelines">
        /// The pipelines.
        /// </param>
        protected override void ApplicationStartup(IUnityContainer container, IPipelines pipelines)
        {
        }

        /// <summary>
        /// Perform registrations that should have an application lifetime
        /// </summary>
        /// <param name="existingContainer">
        /// The existing container.
        /// </param>
        protected override void ConfigureApplicationContainer(IUnityContainer existingContainer)
        {
            existingContainer.RegisterType<ISensorFactory, SensorFactory>();
            existingContainer.RegisterType<Serializer.ISerializer, Serializer.XmlSerializer>();
        }

        /// <summary>
        /// Perform registrations that should have a request lifetime
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void ConfigureRequestContainer(IUnityContainer container, NancyContext context)
        {
        }

        /// <summary>
        /// No registrations should be performed in here, however you may
        /// resolve things that are needed during request startup.</summary>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="pipelines">
        /// The pipelines.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override void RequestStartup(IUnityContainer container, IPipelines pipelines, NancyContext context)
        {
        }
    }
}