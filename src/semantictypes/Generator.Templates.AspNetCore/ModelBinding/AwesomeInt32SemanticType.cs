using Obviously.SemanticTypes.Generator;

namespace Generator.Templates.AspNetCore.ModelBinding
{
    [SemanticType(typeof(int))]
    public partial class AwesomeInt32SemanticType { }

    // this type exists to verify the proper conversation inside the model binder.
    // If it was not successful, the compiler will raise an error "Error CS1503  Argument 1: cannot convert from 'int' to 'string'.
    [SemanticType(typeof(string))]
    public partial class AwesomeStringSemanticType { }
}
