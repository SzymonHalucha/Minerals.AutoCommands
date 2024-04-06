namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandOrderException : CommandExceptionBase
    {
        public CommandOrderException() : base(string.Empty) { }
        public CommandOrderException(string message) : base(message) { }
    }
}