using System;

namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding.BlogPost
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostService(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public BlogPost Get(BlogPostId id)
        {
            if (_blogPostRepository.BlogPosts.TryGetValue(id, out var post))
            {
                return post;
            }

            throw new InvalidOperationException("Blog post not found");
        }
    }
}
