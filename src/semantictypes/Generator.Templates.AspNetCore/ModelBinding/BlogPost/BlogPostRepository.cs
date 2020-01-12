using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding.BlogPost
{
    public class BlogPostRepository : IBlogPostRepository
    {
        public BlogPostRepository()
        {
            var blogPosts =  new Dictionary<BlogPostId, BlogPost>();
            var b1 = new BlogPostId(Guid.Parse("1B22072E-F3F1-445B-98C3-6CD4E21E5B12"));
            var b2 = new BlogPostId(Guid.Parse("DE9BEE94-39CB-4BE9-A021-82354AAAF3D3"));
            blogPosts[b1] = new BlogPost(b1, "Fancy content");
            blogPosts[b2] = new BlogPost(b2, "Even fancier content");
            BlogPosts = blogPosts.ToImmutableDictionary();
        }

        public IImmutableDictionary<BlogPostId, BlogPost> BlogPosts { get; }
    }
}