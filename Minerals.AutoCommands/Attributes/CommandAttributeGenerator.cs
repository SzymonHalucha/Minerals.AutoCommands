namespace Minerals.AutoCommands.Attributes
{
    [Generator]
    public class CommandAttributeGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(static (context) =>
            {
                context.AddSource("CommandAttribute.g.cs", GenerateAttribute());
            });
        }

        public static SourceText GenerateAttribute()
        {
            const string source = """
            #pragma warning disable CS9113
            namespace Minerals.AutoCommands
            {
                [global::System.Diagnostics.DebuggerNonUserCode]
                [global::System.Runtime.CompilerServices.CompilerGenerated]
                [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
                [global::System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
                public sealed class CommandAttribute : global::System.Attribute
                {
                    public CommandAttribute(params string[] aliases)
                    {
                    }
                }
            }
            #pragma warning restore CS9113
            """;
            return SourceText.From(source, Encoding.UTF8);
        }
    }
}