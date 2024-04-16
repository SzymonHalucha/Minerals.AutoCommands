namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandNotFoundException(ICommandPipeline pipeline, ICommandStatement current, string alias)
        : CommandException(pipeline, current)
    {
        public override string Message => $"The command with name '{Alias}' was not found.";
        public string Alias => alias;
    }
}