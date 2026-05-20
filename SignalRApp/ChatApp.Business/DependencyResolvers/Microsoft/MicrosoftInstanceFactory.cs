using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Business.DependencyResolvers.Microsoft
{
    public static class MicrosoftInstanceFactory
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Configure(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }


        public static T GetInstance<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        public static object GetInstance(Type type) 
        { 
        
            return ServiceProvider.GetRequiredService(type);
        }
    }
}
