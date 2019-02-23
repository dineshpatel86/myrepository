using Autofac;
using SampleAPIProject.Data.Database;
using SampleAPIProject.Data.Repository;
using SimbaShoppe.Data.Databases;
using System;
using System.Collections.Generic;
using System.Text;


namespace SampleAPIProject.Services
{
   public static class ServiceDependencyRegister
    {
        public static void Resolve(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof( EfRepository<>)).As(typeof( IRepository<>)).InstancePerDependency();
            builder.RegisterType<SimbaShoppeEntities>().As<IDbContext>().InstancePerDependency();
            builder.RegisterType<User.UserService>().As<User.IUserService>().InstancePerDependency();
        }
    }
}
