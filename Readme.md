# Obviously

## Semantic Types

> Let's you easily create semantic types

### Installation

The following NuGet packages have to be added to the project
1. [CodeGeneration.Roslyn.Buildtime](https://www.nuget.org/packages/CodeGeneration.Roslyn.BuildTime/)
2. [dotnet-codegen](https://www.nuget.org/packages/dotnet-codegen/)
3. [Obviously.SemanticTypes](https://www.nuget.org/packages/Obviously.SemanticTypes) 

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
* The implementation of `System.IComparable<T>`
* The implementation of `System.IEquatable<T>`
* The implementation of `Equals(object)`
* The implementation of `GetHashCode()`
* The implementation of the `equality operator`
* The implementation of the `inequality operator`

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

## License
Is licensed under [MIT](License).