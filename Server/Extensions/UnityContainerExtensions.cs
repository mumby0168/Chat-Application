using Microsoft.Practices.Unity;

namespace Server.Extensions
{
    public static class UnityContainerExtensions
    {
        public static void RegisterSingleton<I, C>(this IUnityContainer unityContainer)
        {
            unityContainer.RegisterType(typeof(I), typeof(C), null, new ContainerControlledLifetimeManager());
        }
    }
}