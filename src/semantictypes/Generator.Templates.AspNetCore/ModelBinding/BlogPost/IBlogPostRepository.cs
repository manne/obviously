using System.Collections.Immutable;

namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding.BlogPost
{
    public interface IBlogPostRepository
    {
        IImmutableDictionary<BlogPostId, BlogPost> BlogPosts { get; }
    }
}