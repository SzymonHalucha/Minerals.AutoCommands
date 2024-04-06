namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandRequiredException : CommandExceptionBase
    {
        public CommandRequiredException() : base(string.Empty) { }
        public CommandRequiredException(string message) : base(message) { }
    }
}