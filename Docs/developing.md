# Developing

## Erros in compilation

If such error is thrown in Visual Studio

```text
 Error: MSB3073 The command "dotnet codegen "@obj\Debug\netcoreapp3.1\StaticTests.csproj.dotnet-codegen.rsp"" exited with code 3. StaticTests C:\Users\ZZZ\.nuget\packages\codegeneration.roslyn.buildtime\0.6.1\build\CodeGeneration.Roslyn.BuildTime.targets 84
 ```

Invoke `dotnet build` manually for the `StaticTests` project. There will be a stack trace of the execption.
