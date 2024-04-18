namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandStatement
    {
        public string[] Aliases { get; }
        public string Description { get; }
        public string Usage { get; }
        public string Group { get; }
        public Regex PossibleValues { get; }
        public Type[] PossibleArguments { get; }

        public bool ValueRequired { get; }
        public Type[] ArgumentsRequired { get; }

        public ICommandStatement? Previous { get; }
        public ICommandStatement? Next { get; }
        public string? Value { get; }

        public bool Execute(Dictionary<object, object>? data = null);
        public bool Evaluate(ICommandStatement? previous, ICommandPipeline pipeline, string[] args, int index);
        public IEnumerable<ICommandStatement> AncestorCommands();
        public IEnumerable<ICommandStatement> DescendantCommands();
    }
}