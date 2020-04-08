using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding;
using Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding.BlogPost;

namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) =>
            services
                .AddSingleton<IBlogPostRepository, BlogPostRepository>()
                .AddSingleton<IBlogPostService, BlogPostService>()
                .AddControllers(options =>
                {
                    options.ModelBinderProviders.Insert(0, new AwesomeGuidSemanticType.AspNetCoreMvcModelBinderProvider());
                    options.ModelBinderProviders.Insert(1, new AwesomeStringSemanticType.AspNetCoreMvcModelBinderProvider());
                    options.ModelBinderProviders.Insert(2, new ManualGuidSemanticTypeModelBinderProvider());
                    options.ModelBinderProviders.Insert(3, new AwesomeGuidSemanticType.AspNetCoreMvcModelBinderProvider());
                    options.ModelBinderProviders.Insert(4, new BlogPostId.AspNetCoreMvcModelBinderProvider());
                });

        public void Configure(IApplicationBuilder app) =>
            app
                .UseDeveloperExceptionPage()
                .UseHttpsRedirection()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
    }
}
