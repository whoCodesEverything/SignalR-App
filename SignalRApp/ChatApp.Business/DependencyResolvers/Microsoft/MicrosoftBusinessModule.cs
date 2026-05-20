
using ChatApp.Business.Abstract;
using ChatApp.Business.Concete;
using ChatApp.DataAccess.Abstract;
using ChatApp.DataAccess.Concrete;
using ChatApp.DataAccess.Concrete.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Business.DependencyResolvers.Microsoft
{
    public static class MicrosoftBusinessModule
    {
        public static IServiceCollection AddRegisterServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\Trusted_Connection=True;");
            });

            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IChatManager, ChatManager>();


            return services;

        }

    }
}
