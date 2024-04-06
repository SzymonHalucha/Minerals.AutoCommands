namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandNotSupportedException : CommandExceptionBase
    {
        public CommandNotSupportedException() : base(string.Empty) { }
        public CommandNotSupportedException(string message) : base(message) { }
    }
}