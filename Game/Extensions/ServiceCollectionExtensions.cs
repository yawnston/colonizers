using Game.ActionGetters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Game.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required Dependency Injection for the Colonizers game engine
        /// </summary>
        public static IServiceCollection AddColonizersGame(this IServiceCollection services)
        {
            return services.AddMediatR(typeof(Resolver).GetTypeInfo().Assembly)
              .AddScoped<IMediator, Mediator>()
              .AddScoped<IColonistPickGetter, ColonistPickGetter>()
              .AddScoped<IDrawGetter, DrawGetter>()
              .AddScoped<IDiscardGetter, DiscardGetter>()
              .AddScoped<IPowerGetter, PowerGetter>()
              .AddScoped<IBuildGetter, BuildGetter>()
              .AddScoped<IGameEndGetter, GameEndGetter>()
              .AddScoped<Resolver>()
              .AddScoped<GameEngine>();
        }
    }
}
