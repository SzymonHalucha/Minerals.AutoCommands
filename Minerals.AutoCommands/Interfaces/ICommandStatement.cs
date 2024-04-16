namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandStatement
    {
        public string[] Aliases { get; }
        public string Description { get; }
        public string Group { get; }
        public bool RequireValue { get; }

        public Type[] PossibleArguments { get; }
        public Type[] RequireArguments { get; }
        public List<ICommandStatement> Arguments { get; }

        public string? Value { get; }
        public ICommandStatement? Parent { get; set; }
        public ICommandWriter? Writer { get; set; }

        public void Execute();
        public void Evaluate(ICommandPipeline pipeline, string[] args, int index);
        public void DisplayHelp();
    }
}