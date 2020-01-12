using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Obviously.SemanticTypes.Generator.Templates.AspNetCore.ModelBinding
{
    [Route("api/[controller]")]
    public class ModelBindingController
    {
        [HttpGet("int/{id}")]
        public IEnumerable<int> GetInt32(AwesomeInt32SemanticType id)
        {
            return new[] { (int)id };
        }

        [HttpGet("string/{id}")]
        public IEnumerable<string> GetString(AwesomeStringSemanticType id)
        {
            return new[] { (string)id };
        }

        [HttpGet("manual/{id}")]
        public Guid GetGuidManual(ManualGuidSemanticType id)
        {
            return id.Value;
        }

        [HttpGet("automatic/{id}")]
        public Guid GetGuidAutomatically(AwesomeGuidSemanticType id)
        {
            return (Guid) id;
        }
    }
}
