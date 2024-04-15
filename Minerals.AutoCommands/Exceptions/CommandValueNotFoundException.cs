namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandValueNotFoundException(ICommandPipeline pipeline, ICommand current)
        : CommandExceptionBase(pipeline, current)
    {
        public override string Message => $"Value for the argument '{Pipeline.GetUsedAlias(Current)}' was not found.";
    }
}