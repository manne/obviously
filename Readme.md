# Obviously

## Semantic Types

[![NuGet](https://img.shields.io/nuget/v/Obviously.SemanticTypes.svg?color=blue&style=flat-square)](https://www.nuget.org/packages/Obviously.SemanticTypes/) [![GitHub license](https://img.shields.io/github/license/manne/obviously?color=blue&style=flat-square)](https://github.com/manne/obviously/blob/master/LICENSE)

> Create semantic types in seconds

### Installation

Install this NuGet package [Obviously.SemanticTypes](https://www.nuget.org/packages/Obviously.SemanticTypes) (currently in preview).

</details>

### Usage

#### Basic

The functionality can be easily used.
Declare a partial class and add the attribute `SemanticType`.
The only parameter of this attribute is the actual type of the semantic type. Here it is `string`.

```CSharp
[SemanticType(typeof(string))]
public partial class EmailAddress { }
```

> ⚠ The class must have the `partial` modifier  
> ℹ The class can be `sealed`

##### What's getting generated

This generator creates

* The public constructor with a single parameter of the actual type
* The implementations of
  * the `comparable` and  `equatable` pattern
  * `explicit operator` for the actual type.

> ℹ This and the others packages are compile-time dependencies. So the compiled assembly does __not__ contain any references on one of the NuGet packages. Even the `SemanticType` attribute is __not__ in the compiled assembly

###### Code Generation Example

For details what is getting generated, see [Docs/code_generation.md](Docs/code_generation.md)

#### Advanced

##### Validation

The input value of the constructor can be validated.
Therefore a static method named `IsValid` has to be implemented.
This method must only have a single parameter of the actual type and must have the return type `bool`.

If the value is __not__ valid, an instance of the semantic type cannot be created.

###### Validation Example

The example shows the validation of an email address.

```CSharp
[SemanticType(typeof(string))]
public partial class EmailAddress
{
    public static bool IsValid(string value)
    {
        return value.Contains('@');
    }
}
```

## Contribution

* Create a fork and make a Pull Request
* Submit a bug
* Submit an idea

## Credits

* For the inspiration [github.com/mperdeck/semantictypes](https://github.com/mperdeck/semantictypes)
* For the code generator [github.com/AArnott/CodeGeneration.Roslyn](https://github.com/AArnott/CodeGeneration.Roslyn)
* For the examples [github.com/andrewlock/StronglyTypedId](https://github.com/andrewlock/StronglyTypedId)
* For making the creation of the generated code so easy [github.com/KirillOsenkov/RoslynQuoter](https://github.com/KirillOsenkov/RoslynQuoter)

## License

This project is licensed under the MIT License - see the [MIT](License) file for details
