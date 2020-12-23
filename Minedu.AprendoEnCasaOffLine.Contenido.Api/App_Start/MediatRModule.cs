﻿using Autofac;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public class MediatRModule : Autofac.Module
    {
        public IConfiguration Configuration;

        public MediatRModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Core")))
                .Where(t => t.IsClosedTypeOf(typeof(IRequest<>)))
                .AsImplementedInterfaces();


            builder
                .RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Core")))
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
            //.Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<,>)))
            //.AsImplementedInterfaces();

            //Fluente Validation
            builder
                .RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Core")))
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
