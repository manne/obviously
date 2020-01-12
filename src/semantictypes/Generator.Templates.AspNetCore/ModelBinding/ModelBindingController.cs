using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Generator.Templates.AspNetCore.ModelBinding
{
    [Route("api/[controller]")]
    public class ModelBindingController
    {
        [HttpGet("{id}")]
        public IEnumerable<int> Get(AwesomeInt32SemanticType id)
        {
            return new[] { (int)id };
        }
    }
}
