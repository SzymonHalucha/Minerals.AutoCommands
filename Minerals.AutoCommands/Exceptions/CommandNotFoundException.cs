namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandNotFoundException(ICommandPipeline pipeline)
        : CommandException(pipeline, null)
    {
        public override string Message => $"No command was found.";
    }
}