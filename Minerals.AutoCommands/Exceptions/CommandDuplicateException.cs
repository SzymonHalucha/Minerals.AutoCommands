namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandDuplicateException : CommandExceptionBase
    {
        public CommandDuplicateException() : base(string.Empty) { }
        public CommandDuplicateException(string message) : base(message) { }
    }
}