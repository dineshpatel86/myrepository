using Autofac.Integration.Mvc;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace SampleAPIProject.Common
{
    public static class EngineContext
    {
        public static T Resolve<T>()
        {
            return AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<T>();
            //return DependencyResolver.Current.GetService<T>();



                


          }
    }
}
        