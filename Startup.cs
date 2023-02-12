using AspNetCore.Authentication.Basic;
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // It requires Realm to be set in the options if SuppressWWWAuthenticateHeader is not set.
        // If an implementation of IBasicUserValidationService interface is used as well as options.Events.OnValidateCredentials delegate is also set then this delegate will be used first.

        services.AddAuthentication(BasicDefaults.AuthenticationScheme)

            // The below AddBasic without type parameter will require options.Events.OnValidateCredentials delegete to be set.
            //.AddBasic(options => { options.Realm = "My App"; });

            // The below AddBasic with type parameter will add the BasicUserValidationService to the dependency container. 
            .AddBasic<BasicUserValidationService>(options => { options.Realm = "My App"; });

        services.AddControllers();

        //// By default, authentication is not challenged for every request which is ASP.NET Core's default intended behaviour.
        //// So to challenge authentication for every requests please use below FallbackPolicy option.
        //services.AddAuthorization(options =>
        //{
        //	options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        //});
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseHttpsRedirection();

        // The below order of pipeline chain is important!
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}