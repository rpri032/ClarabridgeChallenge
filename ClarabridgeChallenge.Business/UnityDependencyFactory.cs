using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace ClarabridgeChallenge.Business
{
    public class UnityDependencyFactory
    {
        private static IUnityContainer CreateUnityContainer()
        {
            IUnityContainer container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(container);
            return container;
        }

        public static T ResolveUnityContainer<T>()
        {
            return CreateUnityContainer().Resolve<T>();
        }

    }
}
