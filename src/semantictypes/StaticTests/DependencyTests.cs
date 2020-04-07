using System;
using FluentAssertions;
using Xunit;

namespace Obviously.SemanticTypes.StaticTests
{
    public class DependencyTests
    {
        [Fact]
        public void GivenTheCompiledAssembly_WhenCheckingItsDependencies_ThenThese_ShouldNotContainTheGeneratorAssemblies()
        {
            typeof(DependencyTests).Assembly.GetReferencedAssemblies()
                .Should()
                .NotContain(an => string.Equals(an.Name, "CodeGeneration.Roslyn", StringComparison.Ordinal))
                .And.NotContain(an => string.Equals(an.Name, "Obviously.SemanticTypes.Generator", StringComparison.Ordinal));
        }
    }
}
