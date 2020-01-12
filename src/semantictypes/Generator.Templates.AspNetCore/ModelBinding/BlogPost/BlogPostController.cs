using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding.BlogPost
{
    [Route("api/[controller]")]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;
        private readonly ILogger<BlogPostController> _logger;

        public BlogPostController(IBlogPostService blogPostService, ILogger<BlogPostController> logger)
        {
            _blogPostService = blogPostService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(BlogPostId id)
        {
            ActionResult<string> result;
            try
            {
                var blogPost = _blogPostService.Get(id);
                result = Ok(blogPost.Content);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation(e, "Blog post (id: {0}) not found", id);
                result = NotFound(id);
            }

            return result;
        }
    }
}
