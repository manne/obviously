using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class InvalidModelWithTwoConstructorsController
    {
        [HttpGet("{name}")]
        public InvalidModelWithTwoConstructors Get(InvalidModelWithTwoConstructors options)
        {
            return options;
        }
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class InvalidModelWithTwoConstructors
    {
        public InvalidModelWithTwoConstructors(string foo, string bar)
        {
            Foo = foo;
            Bar = bar;
        }

        public InvalidModelWithTwoConstructors(string foo)
        {
            Foo = foo;
        }

        public string Foo { get; }
        public string Bar { get; }
    }
}
