using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class TrivialController
    {
        [HttpGet("1/{name}")]
        public TrivialOptions Get(TrivialOptions options)
        {
            return options;
        }

        [HttpGet("2/{name}")]
        public TrivialOptions2 Get(TrivialOptions2 options)
        {
            return options;
        }

        [HttpGet("3/{name}")]
        public TrivialOptions3 Get(TrivialOptions3 options)
        {
            return options;
        }
    }

    public sealed class TrivialOptions
    {
        public TrivialOptions(string name, int pageNumber)
        {
            Name = name;
            PageNumber = pageNumber;
        }

        public string Name { get; }

        public int PageNumber { get; }
    }

    public sealed class TrivialOptions3
    {
        public TrivialOptions3(string name, int pageNumber)
        {
            Name = name;
            PageNumber = pageNumber;
        }

        public string Name { get; }

        [FromQuery(Name = "no")]
        public int PageNumber { get; }
    }

    public sealed class TrivialOptions2
    {
        public TrivialOptions2(string name, int pageNumber, string subName)
        {
            Name = name;
            PageNumber = pageNumber;
            SubName = subName;
        }

        public string Name { get; }

        public int PageNumber { get; }

        // the attribute is optional
        [FromQuery]
        public string SubName { get; }
    }
}
