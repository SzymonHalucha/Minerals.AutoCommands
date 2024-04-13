namespace Minerals.AutoCommands.Generators
{
    public readonly struct CommandObject : IEquatable<CommandObject>
    {
        public string[] Modifiers { get; }
        public string Name { get; }
        public string Namespace { get; }
        public string[] Aliases { get; }

        public CommandObject(GeneratorAttributeSyntaxContext context)
        {
            Modifiers = GetModifiersOf(context);
            Name = GetNameOf(context);
            Namespace = GetNamespaceFrom(context.TargetNode);
            Aliases = GetAliasesOf(context);
        }

        public bool Equals(CommandObject other)
        {
            return other.Modifiers.SequenceEqual(Modifiers)
            && other.Name.Equals(Name)
            && other.Namespace.Equals(Namespace)
            && other.Aliases.SequenceEqual(Aliases);
        }

        public override bool Equals(object obj)
        {
            return obj is CommandObject cmdObj
            && cmdObj.Modifiers.SequenceEqual(Modifiers)
            && cmdObj.Name.Equals(Name)
            && cmdObj.Namespace.Equals(Namespace)
            && cmdObj.Aliases.SequenceEqual(Aliases);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Modifiers, Name, Namespace, Aliases);
        }

        private string[] GetModifiersOf(GeneratorAttributeSyntaxContext context)
        {
            return ((MemberDeclarationSyntax)context.TargetNode).Modifiers.Select(x => x.ValueText).ToArray();
        }

        private string GetNameOf(GeneratorAttributeSyntaxContext context)
        {
            return ((BaseTypeDeclarationSyntax)context.TargetNode).Identifier.ValueText;
        }

        private static string GetNamespaceFrom(SyntaxNode from)
        {
            return from.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString() ?? string.Empty;
        }

        private string[] GetAliasesOf(GeneratorAttributeSyntaxContext context)
        {
            var types = context.Attributes.SelectMany(x => x.ConstructorArguments.SelectMany(y => y.Values));
            return types.Select(x => x.Value?.ToString() ?? string.Empty).ToArray();
        }
    }
}