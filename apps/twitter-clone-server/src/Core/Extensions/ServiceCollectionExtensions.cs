using TwitterClone.APIs;

namespace TwitterClone;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add services to the container.
    /// </summary>
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IFollowersService, FollowersService>();
        services.AddScoped<ILikesService, LikesService>();
        services.AddScoped<IRetweetsService, RetweetsService>();
        services.AddScoped<ITweetsService, TweetsService>();
        services.AddScoped<IUsersService, UsersService>();
    }
}
