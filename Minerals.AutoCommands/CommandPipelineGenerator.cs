namespace Minerals.AutoCommands
{
    [Generator]
    public class CommandPipelineGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var commands = context.SyntaxProvider.ForAttributeWithMetadataName
            (
                "Minerals.AutoCommands.CommandAttribute",
                static (x, _) => true, //TODO: Optimize this for value types that matters
                static (x, _) => x
            );

            var pipelines = context.SyntaxProvider.ForAttributeWithMetadataName
            (
                "Minerals.AutoCommands.CommandPipelineAttribute",
                static (x, _) => true, //TODO: Optimize this for value types that matters
                static (x, _) => x
            );

            var combined = pipelines.Combine(commands.Collect());
            context.RegisterSourceOutput(combined, static (ctx, element) =>
            {
                string fileName = $"{element.Left.GetTargetNodeName()}.g.cs";
                ctx.AddSource(fileName, BuildCommandPipelinePartialClass
                (
                    element.Left,
                    element.Right
                ));
            });
        }

        private static SourceText BuildCommandPipelinePartialClass
        (
            GeneratorAttributeSyntaxContext pipeline,
            ImmutableArray<GeneratorAttributeSyntaxContext> commands
        )
        {
            var commandsClasses = commands.Select(x => (ClassDeclarationSyntax)x.TargetNode);
            var builder = new CodeBuilder();

            builder.AddAutoGeneratedHeader(Assembly.GetExecutingAssembly());
            builder.AddAllUsings(pipeline.TargetNode, out bool hasUsings);
            if (hasUsings)
            {
                builder.WriteLine("");
            }
            builder.AddNamespace(pipeline.TargetNode, out bool hasNamespace);

            builder.AddAutoGeneratedAttributes<ClassDeclarationSyntax>();
            GeneratePartialClass(builder, pipeline);
            GenerateParseCommandArgumentMethod(builder, commandsClasses);
            GenerateEvaluateCommandLineMethod(builder);
            builder.CloseBlock();
            if (hasNamespace)
            {
                builder.CloseBlock();
            }

            return SourceText.From(builder.ToString(), Encoding.UTF8);
        }

        private static void GeneratePartialClass(CodeBuilder builder, GeneratorAttributeSyntaxContext pipeline)
        {
            var cls = (ClassDeclarationSyntax)pipeline.TargetNode;
            builder.Write(cls.Modifiers.ToString())
                .Write(" class ")
                .Write(cls.Identifier.ValueText)
                .WriteLine(" : global::Minerals.AutoCommands.ICommandPipeline")
                .OpenBlock();
        }

        private static void GenerateParseCommandArgumentMethod(CodeBuilder builder, IEnumerable<ClassDeclarationSyntax> commands)
        {
            builder.WriteLine("public global::Minerals.AutoCommands.ICommand ParseCommandArgument(string argument)").OpenBlock();
            builder.WriteLine("switch (argument)").OpenBlock();
            GenerateCasesForCommands(builder, commands);
            builder.WriteLine("default:").WriteLine("return new global::Minerals.AutoCommands.InvalidCommand();");
            builder.CloseBlock();
            builder.CloseBlock(true);
        }

        private static void GenerateCasesForCommands(CodeBuilder builder, IEnumerable<ClassDeclarationSyntax> commands)
        {
            foreach (var item in commands)
            {
                foreach (var list in item.AttributeLists)
                {
                    foreach (var attr in list.Attributes)
                    {
                        if ((attr.Name.ToString().Equals("Command")
                            || attr.Name.ToString().Equals("CommandAttribute"))
                            && attr.ArgumentList != null)
                        {
                            foreach (var arg in attr.ArgumentList!.Arguments)
                            {
                                builder.Write("case ").Write(arg.Expression.ToString()).WriteLine(":");
                                builder.Write("return new ").Write(item.Identifier.ValueText).WriteLine("();");
                            }
                        }
                    }
                }
            }
        }

        private static void GenerateEvaluateCommandLineMethod(CodeBuilder builder)
        {
            builder.WriteLine("protected global::Minerals.AutoCommands.ICommand EvaluateCommandLine(string[] arguments)").OpenBlock();
            builder.WriteLine("global::Minerals.AutoCommands.ICommand entryCommand = ParseCommandArgument(arguments[0]);");
            builder.WriteLine("entryCommand.Evaluate(this, arguments, 1);");
            builder.WriteLine("return entryCommand;").CloseBlock();
        }
    }
}