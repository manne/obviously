namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding.BlogPost
{
    public sealed class BlogPost
    {
        public BlogPost(BlogPostId id, string content)
        {
            Id = id;
            Content = content;
        }

        public BlogPostId Id { get; }

        public string Content { get; }
    }
}