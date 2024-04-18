namespace Minerals.AutoCommands.Generators
{
    public readonly struct CommandStatementObject : IEquatable<CommandStatementObject>
    {
        public static CommandStatementObject Empty { get; } = new CommandStatementObject();

        public string[] Modifiers { get; }
        public string Name { get; }
        public string Namespace { get; }
        public string[] Aliases { get; }

        public CommandStatementObject()
        {
            Modifiers = [];
            Name = string.Empty;
            Namespace = string.Empty;
            Aliases = [];
        }

        public CommandStatementObject(GeneratorSyntaxContext context)
        {
            Modifiers = GetModifiersOf(context);
            Name = GetNameOf(context);
            Namespace = GetNamespaceFrom(context.Node);
            Aliases = GetAliasesOf(context);
        }

        public bool Equals(CommandStatementObject other)
        {
            return other.Modifiers.SequenceEqual(Modifiers)
            && other.Name.Equals(Name)
            && other.Namespace.Equals(Namespace)
            && other.Aliases.SequenceEqual(Aliases);
        }

        public override bool Equals(object obj)
        {
            return obj is CommandStatementObject cmdObj
            && cmdObj.Modifiers.SequenceEqual(Modifiers)
            && cmdObj.Name.Equals(Name)
            && cmdObj.Namespace.Equals(Namespace)
            && cmdObj.Aliases.SequenceEqual(Aliases);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Modifiers, Name, Namespace, Aliases);
        }

        private string[] GetModifiersOf(GeneratorSyntaxContext context)
        {
            return ((MemberDeclarationSyntax)context.Node).Modifiers.Select(x => x.ValueText).ToArray();
        }

        private string GetNameOf(GeneratorSyntaxContext context)
        {
            return ((BaseTypeDeclarationSyntax)context.Node).Identifier.ValueText;
        }

        private static string GetNamespaceFrom(SyntaxNode from)
        {
            return from.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString() ?? string.Empty;
        }

        private string[] GetAliasesOf(GeneratorSyntaxContext context)
        {
            var syntax = ((TypeDeclarationSyntax)context.Node).Members.OfType<PropertyDeclarationSyntax>()
                .First(x =>
                {
                    return x.Identifier.ValueText.Equals("Aliases", StringComparison.Ordinal);
                });

            ExpressionSyntax? expression = syntax.ExpressionBody?.Expression ?? syntax.Initializer?.Value;
            IEnumerable<string> aliases = [];

            if (expression is ArrayCreationExpressionSyntax array)
            {
                aliases = array.Initializer!.Expressions.Select(x => ((LiteralExpressionSyntax)x).Token.ValueText);
            }
            else if (expression is CollectionExpressionSyntax collection)
            {
                aliases = collection.Elements.Select(x => ((LiteralExpressionSyntax)((ExpressionElementSyntax)x).Expression).Token.ValueText);
            }

            return aliases.ToArray();
        }
    }
}