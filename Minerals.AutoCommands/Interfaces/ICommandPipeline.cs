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
        public ICommandWriter Writer { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public StringComparison Comparison { get; }

        public ICommandPipeline UseExceptionHandler<T>(Action<T> handler) where T : Exception, new();
        public ICommandPipeline UseCommandParser<T>() where T : ICommandParser, new();
        public ICommandPipeline UseCommandParser(ICommandParser parser);
        public ICommandPipeline UseCommandWriter<T>(int textIndentation = 2) where T : ICommandWriter, new();
        public ICommandPipeline UseCommandWriter(ICommandWriter writer);
        public ICommandPipeline UseCommandHelpAliases(string[] aliases);
        public ICommandPipeline UseStringComparison(StringComparison comparison);

        public ICommandStatement? Evaluate(string[] args);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetUsedCommandAlias(ICommandStatement? command);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CheckCommandHelp(string[] args, int index);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CheckDescendantCommandsHelp(string[] args, int index);
    }
}