namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandValueNotFoundException(ICommandPipeline pipeline, ICommandStatement? current)
        : CommandException(pipeline, current)
    {
        public override string Message => $"Value for the argument '{Pipeline.GetUsedCommandAlias(Current)}' was not found.";
    }
}