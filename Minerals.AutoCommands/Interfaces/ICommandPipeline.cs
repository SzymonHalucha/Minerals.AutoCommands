namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandPipeline
    {
        public string Title { get; }
        public string Version { get; }
        public string MainCommand { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommandParser Parser { get; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StringComparison Comparison { get; }

        public ICommandPipeline UseExceptionHandler<T>(Action<T> handler) where T : Exception, new();
        public ICommandPipeline UseCommandParser<T>() where T : ICommandParser, new();
        public ICommandStatement? Evaluate(string[] args);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetUsedAlias(ICommand command);
        public void Help();
    }
}