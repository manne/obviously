# ASP.NET Core Model Binding

This module enables using the semantic types in [ASP.NET Core Model Binding](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-3.1).

## Introduction

Instead of manually creating an instance of the semantic type, the module create the binding mechanism.

### Before

```CSharp
    [Route("api/[controller]")]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            var blogPostId = new BlogPostId(id);
            var blogPost = _blogPostService.Get(blogPostId);
            return Ok(blogPost.Content);
        }
    }
```

### After

```CSharp
    [Route("api/[controller]")]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(BlogPostId id)
        {
            var blogPost = _blogPostService.Get(id);
            return Ok(blogPost.Content);
        }
    }
```

## Prerequisites

The project in which the semantic types resides, must have a reference of the assembly `Microsoft.AspNetCore.Mvc`.

## Integration

In the `Startup` class the __Binding Provider__ has to be manually added.  
E.g.

```CSharp
public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.ModelBinderProviders.Insert(0, new BlogPostId.AspNetCoreMvcModelBinderProvider());
                });
            services.AddMvcCore(options =>
                {
                    options.EnableEndpointRouting = false;
                });
        }
```

## Generated Code

This module generates two nested classes inside the semantic type declaration:

* `AspNetCoreMvcModelBinder`
* `AspNetCoreMvcModelBinderProvider`
