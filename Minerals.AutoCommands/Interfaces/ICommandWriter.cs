namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandWriter
    {
        public int TextIndentation { get; set; }

        public void WriteDebug(string message);
        public void WriteLineDebug(string message);
        public void WriteInfo(string message);
        public void WriteLineInfo(string message);
        public void WriteWarning(string message);
        public void WriteLineWarning(string message);
        public void WriteError(string message);
        public void WriteLineError(string message);

        public void WriteHelpForException(CommandException exception);
        public void WriteHelpForPipeline(ICommandPipeline pipeline);
        public void WriteHelpForCommand(ICommandPipeline pipeline, ICommandStatement command);
    }
}