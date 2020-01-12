using CodeGeneration.Roslyn;
using FluentAssertions;
using Obviously.SemanticTypes.Generator;
using Xunit;

namespace Obviously.SemanticTypes.StaticTests
{
    public class DependencyTests
    {
        [Fact]
        public void GivenTheCompiledAssembly_WhenCheckingItsDependencies_ThenThese_ShouldNotContainTheGeneratorAssemblies()
        {
            var notReferencedAssemblyNames = new[]
            {
                typeof(SemanticTypeGenerator).Assembly.GetName(),
                typeof(IRichCodeGenerator).Assembly.GetName()
            };
            typeof(DependencyTests).Assembly.GetReferencedAssemblies().Should().NotContain(notReferencedAssemblyNames);
        }
    }
}
