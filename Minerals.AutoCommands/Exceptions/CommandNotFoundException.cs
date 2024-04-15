namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandNotFoundException(ICommandPipeline pipeline, ICommand current, string alias)
        : CommandExceptionBase(pipeline, current)
    {
        public override string Message => $"The command with name '{Alias}' was not found.";
        public string Alias => alias;
    }
}