# Minerals.AutoCommands

![GitHub License](https://img.shields.io/github/license/SzymonHalucha/Minerals.AutoCommands?style=for-the-badge)
![NuGet Version](https://img.shields.io/nuget/v/Minerals.AutoCommands?style=for-the-badge)
![NuGet Downloads](https://img.shields.io/nuget/dt/Minerals.AutoCommands?style=for-the-badge)

[Package on nuget.org](https://github.com/SzymonHalucha/Minerals.AutoCommands)

This NuGet package simplifies development of console tools in C# by automating command parsing. It eliminates need to manually write code to handle arguments, commands and commands helps, allowing you to focus on the logic of the tool.

## Features

- Automatic recognition of arguments, commands and help commands
- Validation of arguments and display of error messages
- Easy definition of shortcuts and aliases
- Ability to define sub commands and arguments
- Automatic generation of usage description for each command
- Standardized output messages
- Compatibility with ```netstandard2.0``` and C# 7.3+

## Installation

Add the Minerals.AutoCommands nuget package to your C# project using the following methods:

### 1. Project file definition

```xml
<PackageReference Include="Minerals.AutoCommands" Version="0.2.1" />
```

### 2. dotnet command

```bat
dotnet add package Minerals.AutoCommands
```

## Usage

To define a new command, you must create a class that inherits from the ```CommandStatement``` base class provided by the package. This class must implement several core methods and properties to enable the parsing of commands and the execution of their logic.

```csharp
namespace Examples
{
    // The command class must inherit from the CommandStatement base class.
    public class TestCommand1 : Minerals.AutoCommands.CommandStatement
    {
        // The array must be initialized!
        // Array of names for this command.
        // Default value: null
        public override string[] Aliases { get; } = ["test1"];

        // The property must be initialized!
        // Short description of the command.
        // Default value: null
        public override string Description { get; } = "Lorem ipsum dolor sit amet 1.";

        // The array must be initialized!
        // Array of command types that can be used as arguments for this command.
        // Default value: null
        public override Type[] PossibleArguments { get; } = [typeof(TestCommand2)];

        public override bool Execute(Dictionary<object, object> data = null)
        {
            // Example code...
            if (success)
            {
                Writer.WriteLineInfo("Command executed successfully!");

                // THE DEVELOPER MUST MANUALLY TRIGGER THE EXECUTION OF THE NEXT COMMAND!
                Next?.Execute(new() { { "ExampleKey", "ExampleValue" } });
                return true;
            }
            else
            {
                Writer.WriteLineWarning("Command not executed!");
                return false;
            }
        }
    }
}
```

### Command requiring an argument

```csharp
namespace Examples
{
    public class TestCommand1 : Minerals.AutoCommands.CommandStatement
    {
        // ...

        // Requires from the user to provide an argument.
        // Default value: false
        public override bool ValueRequired { get; } = true;

        // Regular expression can be used to specify which values are allowed.
        // Default value: "." (Anything allowed)
        public override Regex PossibleValues { get; } = new Regex("[a-zA-Z]");

        // ...
    }
}
```

### Optional command class values

The ```CommandStatement``` base class provides a set of optional values that can be used to customize functionality of the command. These properties allows you to define additional information about the command, such as its group, usage and required arguments.

```csharp
namespace Examples
{
    public class TestCommand1 : Minerals.AutoCommands.CommandStatement
    {
        // ...

        // An array of argument types required by this command.
        // Default value: Array.Empty<Type>()
        public override Type[] ArgumentsRequired { get; } = [typeof(TestCommand2)];

        // The name of the group to which the command belongs.
        // Default value: "Options"
        public override string Group { get; } = "Test Commands";

        // The usage of the command, which is a description of how to invoke it correctly.
        // If the value is empty, CommandWriter will automatically generate the usage.
        // Default value: string.Empty
        public override string Usage { get; } = "[Command] [Options]";

        // ...
    }
}
```

### Obtaining values during command execution

The ``CommandStatement`` base class provides a set of values that can be used during command execution. They allow access to information about the context of the command execution, the values of the user-provided arguments and other data important for the logic of the command execution.

```csharp
// Stores the value of the argument provided by the user, if the command requires an argument.
public string? Value { get; protected set; }

// Stores the previously executed command, if any.
public ICommandStatement? Previous { get; protected set; }

// Stores the next command to be executed, if any. To execute it, you need to manually call its Execute() method.
public ICommandStatement? Next { get; protected set; }

// Provides access to the CommandWriter object, which is used to display command output messages.
// Use it instead of Console.WriteLine() to maintain a consistent and readable format for messages.
protected ICommandWriter Writer { get; set; }

// Returns a collection of all commands executed before the current command.
public virtual IEnumerable<ICommandStatement> AncestorCommands();

// Returns a collection of all commands to be executed after the current command.
public virtual IEnumerable<ICommandStatement> DescendantCommands();
```

### Running a command pipeline

To run the command pipeline created with this package, you need to perform the following steps:

```csharp
namespace Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Creates a command pipeline.
            // Arguments: title, version of the tool, main tool command (ToolCommandName in csproj file).
            var pipeline = new CommandPipeline("Test Command Line", "1.2.3", "cmd");

            // REQUIRED instruction to parse the commands written by the developer.
            pipeline.UseCommandParser<CommandParser>();

            // What commands can be executed directly after the main tool command (ToolCommandName in csproj file).
            pipeline.UsePossibleArguments(typeof(TestCommand1), typeof(TestCommand2), typeof(TestCommand3));

            // Creates a doubly linked list (Previous, Next) of commands and returns the first command in the pipeline.
            var command = pipeline.Evaluate(args);

            // Starts execution of the first command.
            command?.Execute();
        }
    }
}
```

### CommandWriter

A class provided by the package which is used to display command output messages. It enables a consistent and readable message format, and makes debugging and testing of your application easier. Instead of using ```Console.WriteLine()`` to display command output messages, use the methods available in the```CommandWriter``` class.

```csharp
Writer.WriteLineDebug("Example");
// or
Writer.WriteLineInfo("Example");
// or
Writer.WriteLineWarning("Example");
// or
Writer.WriteLineError("Example");

// Instead of

Console.WriteLine("Example");
// or
Console.Error.WriteLine("Example");
```

### Exceptions

This package has custom exceptions, by default the package automatically handles these exceptions, displaying the appropriate error messages. You can customize the default exceptions handlers or implement your own exception handling using the ```UseExceptionHandler()``` method on the ```CommandPipeline``` object. List of custom exceptions of the package:

- CommandArgumentNotFoundException
- CommandArgumentNotSupportedException
- CommandArgumentRequiredException
- CommandNotFoundException
- CommandNotSupported
- CommandValueNotFoundException
- CommandValueNotSupportedException
- CommandValueRequiredException

### Customizing the CommandPipeline

This package provides a set of methods to customize the functions of the command pipeline depending on your needs.

```csharp
// Enables you to connect custom exception handling mechanisms.
public ICommandPipeline UseExceptionHandler<T>(Action<T> handler) where T : Exception, new();

// REQUIRED instruction to parse the commands written by the developer, which defines how commands and their arguments are parsed.
// Default value: null
public ICommandPipeline UseCommandParser<T>() where T : ICommandParser, new();
// or
public ICommandPipeline UseCommandParser(ICommandParser parser);

// Allows you to attach a custom CommandWriter object which defines how command output messages are displayed.
// Default value: new CommandWriter();
public ICommandPipeline UseCommandWriter<T>(int textIndentation = 2) where T : ICommandWriter, new();
// or
public ICommandPipeline UseCommandWriter(ICommandWriter writer);

// Allows you to set custom aliases for help commands.
// Default value: ["--help", "-h"]
public ICommandPipeline UseCommandHelpAliases(string[] aliases);

// Allows you to set how the strings of command line are compared.
// Default value: StringComparison.Ordinal
public ICommandPipeline UseStringComparison(StringComparison comparison);

// Allows you to define a list of command types that can be executed directly after the main tool command (ToolCommandName in the csproj file).
// Default value: null
public ICommandPipeline UsePossibleArguments(params Type[] possibleArguments);
```

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [branches on this repository](https://github.com/SzymonHalucha/Minerals.AutoCommands/branches).

## Authors

- **Szymon Hałucha** - Maintainer

See also the list of [contributors](https://github.com/SzymonHalucha/Minerals.AutoCommands/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](./LICENSE) file for details.
