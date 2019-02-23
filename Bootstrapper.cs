using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using SampleAPIProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SampleAPIProject1
{
    public static class Bootstrapper
    {
        public static void Resolve(ContainerBuilder builder)
        {




            //// Get your HttpConfiguration.
            //var config = GlobalConfiguration.Configuration;

            //// Register your Web API controllers.
            //builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            ////builder.RegisterControllers(typeof(WebApiApplication).Assembly);


            //// OPTIONAL: Register the Autofac filter provider.
            //builder.RegisterWebApiFilterProvider(config);

            //// OPTIONAL: Register the Autofac model binder provider.
            //builder.RegisterWebApiModelBinderProvider();

            //ServiceDependencyRegister.Resolve(builder);
            //// Set the dependency resolver to be Autofac.
            //var container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

           

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            ServiceDependencyRegister.Resolve(builder);
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);


        }
    }
}