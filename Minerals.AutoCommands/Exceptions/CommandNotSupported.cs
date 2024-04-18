namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandNotSupportedException(ICommandPipeline pipeline, ICommandStatement? current)
        : CommandException(pipeline, current)
    {
        public override string Message => $"The command named '{Pipeline.GetUsedCommandAlias(Current)}' is not valid argument for this tool.";
    }
}