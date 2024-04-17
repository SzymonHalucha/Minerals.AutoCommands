namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandValueRequiredException(ICommandPipeline pipeline, ICommandStatement? current)
        : CommandException(pipeline, current)
    {
        public override string Message => $"A value is required for the '{Pipeline.GetUsedCommandAlias(Current)}' command.";
    }
}