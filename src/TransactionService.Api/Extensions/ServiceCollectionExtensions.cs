using TransactionService.Application.Managers.Interfaces;
using TransactionService.Application.Mappings;
using TransactionService.Infrastructure.Repositories.Implementations;
using TransactionService.Infrastructure.Repositories.Interfaces;
using TransactionService.Infrastructure.Extensions;
using TransactionService.Application.Managers.Implementations;

namespace TransactionService.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddAutoMapper(
               config => { },
               typeof(MappingProfile));

            services.AddInfrastructure(configuration);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ITransactionManager, TransactionManager>();

            return services;
        }
    }
}
