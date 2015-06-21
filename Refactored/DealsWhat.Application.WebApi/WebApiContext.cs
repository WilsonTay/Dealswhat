﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using DealsWhat.Application.WebApi.Controllers;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Infrastructure.DataAccess;

namespace DealsWhat.Application.WebApi
{
    public class WebApiContext
    {
        private static IDependencyResolver resolver;

        public static IDependencyResolver DefaultResolver
        {
            get
            {
                if (resolver == null)
                {
                    resolver = CreateResolver();
                }

                return resolver;
            }
            set { resolver = value; }
        }


        private static IDependencyResolver CreateResolver()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IRepositoryFactory>(new EFRepositoryFactory(new DealsWhatUnitOfWork()));
            builder.RegisterApiControllers(typeof(DealsController).Assembly);

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);

            return resolver;
        }
    }
}