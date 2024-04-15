namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommand
    {
        public string[] Aliases { get; }
        public string Description { get; }
        public string Group { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommand? Parent { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Evaluate(ICommandPipeline pipeline, string[] args, int index);
        public string GetHelp();
    }
}