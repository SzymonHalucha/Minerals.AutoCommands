namespace Minerals.AutoCommands.Generators
{
    [Generator]
    public class CommandStatementGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var statements = context.SyntaxProvider.ForAttributeWithMetadataName
            (
                "Minerals.AutoCommands.Attributes.CommandStatementAttribute",
                static (x, _) => true, //TODO: Optimize this for value types that matters
                static (x, _) => x
            );

            context.RegisterSourceOutput(statements, static (ctx, element) =>
            {
                string fileName = $"{element.GetTargetNodeName()}.g.cs";
                ctx.AddSource(fileName, GeneratePartialClass(element));
            });
        }

        private static SourceText GeneratePartialClass(GeneratorAttributeSyntaxContext statement)
        {
            var builder = new CodeBuilder();
            builder.AddAutoGeneratedHeader(Assembly.GetExecutingAssembly());
            builder.WriteLine("using global::System.Linq;");

            builder.NewLine();
            builder.AddNamespaceDeclarationHeader(statement.TargetNode);

            builder.AddAutoGeneratedAttributes(typeof(ClassDeclarationSyntax));
            builder.AddTypeDeclarationHeader(statement.TargetNode, ["global::Minerals.AutoCommands.Interfaces.ICommandStatement"]);

            builder.OpenBlock();
            GenerateArgumentsProperty(builder);
            builder.NewLine();
            GenerateParentProperty(builder);
            builder.NewLine();
            GenerateEvaluateMethod(builder);
            builder.NewLine();
            GenerateGetNextCommandMethod(builder);
            builder.NewLine();
            GenerateCheckIfRequiredArgumentsAreValidMethod(builder);

            builder.CloseAllBlocks();
            return SourceText.From(builder.ToString(), Encoding.UTF8);
        }

        private static void GenerateArgumentsProperty(CodeBuilder builder)
        {
            builder.WriteLine(@"public global::System.Collections.Generic.List<global::Minerals.AutoCommands.Interfaces.ICommand> Arguments { get; } = new global::System.Collections.Generic.List<global::Minerals.AutoCommands.Interfaces.ICommand>();");
        }

        private static void GenerateParentProperty(CodeBuilder builder)
        {
            builder.WriteBlock("""

            public global::Minerals.AutoCommands.Interfaces.ICommandStatement Parent
            {
                get
                {
                    return _parent;
                }
                set
                {
                    if (_parent != null)
                    {
                        _parent.Arguments.Remove(this);
                    }
                    _parent = value;
                    _parent.Arguments.Add(this);
                }
            }

            private global::Minerals.AutoCommands.Interfaces.ICommandStatement _parent = null;
            """);
        }

        private static void GenerateEvaluateMethod(CodeBuilder builder)
        {
            builder.WriteBlock("""

            public bool Evaluate(string[] args, int index, global::System.StringComparison comparison)
            {
                if (index >= args.Length) 
                {
                    return true;
                }

                var nextCommand = GetNextCommand(args, index, comparison);
                nextCommand.Parent = this;
                nextCommand.Evaluate(args, ++index, comparison);
                
                return CheckIfRequiredArgumentsAreValid();
            }
            """);
        }

        private static void GenerateGetNextCommandMethod(CodeBuilder builder)
        {
            builder.WriteBlock("""

            private global::Minerals.AutoCommands.Interfaces.ICommand GetNextCommand(string[] args, int index, global::System.StringComparison comparison)
            {
                var nextCommand = global::Minerals.AutoCommands.CommandParser.Parse(args[index], comparison);
                if (nextCommand == null)
                {
                    throw new global::Minerals.AutoCommands.Exceptions.CommandNotFoundException($"The command '{args[index]}' was not found.")
                        .AddData(("Parent", Parent), ("Index", index), ("Comparison", comparison));
                }
                if (!PossibleArguments.Contains(nextCommand.GetType()))
                {
                    throw new global::Minerals.AutoCommands.Exceptions.CommandNotSupportedException($"Command of the type '{nextCommand.GetType().Name}' is not supported.")
                        .AddData(("Command", nextCommand), ("Parent", Parent), ("Index", index), ("Comparison", comparison));
                }
                if (Arguments.Any(x => x.GetType() == nextCommand.GetType()))
                {
                    throw new global::Minerals.AutoCommands.Exceptions.CommandDuplicateException($"The '{nextCommand.GetType().Name}' command type has already been used.")
                        .AddData(("Command", nextCommand), ("Parent", Parent), ("Index", index), ("Comparison", comparison));
                }
                return nextCommand;
            }
            """);
        }

        private static void GenerateCheckIfRequiredArgumentsAreValidMethod(CodeBuilder builder)
        {
            builder.WriteBlock("""

            private bool CheckIfRequiredArgumentsAreValid()
            {
                foreach (var requiredArg in RequiredArguments)
                {
                    if (!Arguments.Any(x => x.GetType() == requiredArg))
                    {
                        throw new global::Minerals.AutoCommands.Exceptions.CommandRequiredException($"The '{requiredArg.Name}' argument type is required.")
                            .AddData(("CommandType", requiredArg), ("Parent", this));
                    }
                }
                return true;
            }
            """);
        }
    }
}