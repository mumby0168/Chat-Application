using Microsoft.Practices.Unity;

namespace Server.Extensions
{
    /// <summary>
    /// Extensions to the unity container.
    /// </summary>
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Generic Type registration.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="unityContainer"></param>
        public static void RegisterSingleton<I, C>(this IUnityContainer unityContainer)
        {
            unityContainer.RegisterType(typeof(I), typeof(C), null, new ContainerControlledLifetimeManager());
        }
    }
}