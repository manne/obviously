# Obviously

## Semantic Types

[![NuGet](https://img.shields.io/nuget/v/Obviously.SemanticTypes.svg)](https://www.nuget.org/packages/Obviously.SemanticTypes/)

> Let's you easily create semantic types

### Installation

The following NuGet packages have to be added to the project
1. [CodeGeneration.Roslyn.Buildtime](https://www.nuget.org/packages/CodeGeneration.Roslyn.BuildTime/) as a package reference
2. [dotnet-codegen](https://www.nuget.org/packages/dotnet-codegen/) as .NET CLI tool reference
3. [Obviously.SemanticTypes](https://www.nuget.org/packages/Obviously.SemanticTypes) as a package reference (this package)

The project file should look like this

```XML
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="CodeGeneration.Roslyn.BuildTime" Version="0.6.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Obviously.SemanticTypes" Version="0.0.1-preview001" />
    <DotNetCliToolReference Include="dotnet-codegen" Version="0.5.13" />
  </ItemGroup>
</Project>
```

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
  * `System.IComparable<T>`
  * `System.IEquatable<T>`
  * `Equals(object)`
  * `GetHashCode()`
  * `equality operator`
  * `inequality operator`
  * `explicit operator` for the actual type
  * `ToString()`

> ℹ This package and the others are compile-time dependencies. So the compiled assembly does __not__ contain any references on one of the NuGet packages. Even the `SemanticType` attribute is __not__ in the compiled assembly

###### Example
```CSharp
public partial class EmailAddress : global::System.IComparable<EmailAddress>, global::System.IEquatable<EmailAddress>
{
    private readonly string _value;
    public EmailAddress(string value)
    {
        _value = value;
    }

    public int CompareTo(EmailAddress other)
    {
        // left out for readability
    }

    public bool Equals(EmailAddress other)
    {
        // left out for readability
    }

    public override bool Equals(object obj)
    {
        // left out for readability
    }

    public override int GetHashCode()
    {
        // left out for readability
    }

    public static bool operator ==(EmailAddress left, EmailAddress right)
    {
        // left out for readability
    }

    public static bool operator !=(EmailAddress left, EmailAddress right)
    {
        // left out for readability
    }

    public static explicit operator string(EmailAddress t)
    {
        // left out for readability
    }
    
    public override string ToString()
    {
        // left out for readability
    }
```

#### Advanced

##### Validation

The input value of the constructor can be validated.
Therefore a static method named `IsValid` has to be implemented.
This method must only have a single parameter of the actual type and must have the return type `bool`.

If the value is __not__ valid, an instance of the semantic type cannot be created.


###### Example

The example should the validation of an email address.

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

## Credits

* For the inspiration [github.com/mperdeck/semantictypes](https://github.com/mperdeck/semantictypes)
* For the code generator [github.com/AArnott/CodeGeneration.Roslyn](https://github.com/AArnott/CodeGeneration.Roslyn)
* For the examples [github.com/andrewlock/StronglyTypedId](https://github.com/andrewlock/StronglyTypedId)
* For making the creation of the generated code so easy [github.com/KirillOsenkov/RoslynQuoter](https://github.com/KirillOsenkov/RoslynQuoter)

## License
Is licensed under [MIT](License).