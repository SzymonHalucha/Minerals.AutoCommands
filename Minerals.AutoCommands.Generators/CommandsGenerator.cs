namespace Minerals.AutoCommands.Generators
{
    [Generator]
    public class CommandsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var values = context.SyntaxProvider.ForAttributeWithMetadataName
            (
                "Minerals.AutoCommands.Attributes.CommandValueAttribute",
                static (x, _) => x is ClassDeclarationSyntax,
                static (x, _) => new CommandObject(x)
            );

            var statements = context.SyntaxProvider.ForAttributeWithMetadataName
            (
                "Minerals.AutoCommands.Attributes.CommandStatementAttribute",
                static (x, _) => x is ClassDeclarationSyntax,
                static (x, _) => new CommandObject(x)
            );

            context.RegisterSourceOutput(values, static (ctx, element) =>
            {
                string fileName = $"{element.Name}.g.cs";
                ctx.AddSource(fileName, CommandValueGenerator.AppendPartialClass(element));
            });

            context.RegisterSourceOutput(statements, static (ctx, element) =>
            {
                string fileName = $"{element.Name}.g.cs";
                ctx.AddSource(fileName, CommandStatementGenerator.AppendPartialClass(element));
            });

            var combined = statements.Collect().Combine(values.Collect());
            context.RegisterSourceOutput(combined, static (ctx, element) =>
            {
                string fileName = "CommandParser.g.cs";
                ctx.AddSource(fileName, CommandParserGenerator.AppendPartialClass(element.Left, element.Right));
            });
        }
    }
}