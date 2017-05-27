using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisOfKeywordsBehaviour
{
    /// <summary>
    /// Предоставляет методы для работы с Unity-контейнером.
    /// </summary>
    public static class UnityFactory
    {
        private static IUnityContainer _container;

        public static void Initialize()
        {
            _container = new UnityContainer();
            RegisterTypes();
        }

        public static void RegisterTypes()
        {
            _container.RegisterType<IResultWriter, ResultWriterToMSOffice>();
            _container.RegisterType<MainForm>();
        }

        public static T ResolveObject<T>(string name = null)
        {
            return _container.Resolve<T>(name);
        }
    }
}
