namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandArgumentNotSupportedException(ICommandPipeline pipeline, ICommandStatement? current, ICommandStatement? next)
        : CommandException(pipeline, current)
    {
        public override string Message => $"The argument named '{Pipeline.GetUsedCommandAlias(Next)}' is not valid for the command named '{Pipeline.GetUsedCommandAlias(Current)}'.";
        public ICommandStatement? Next => next;
    }
}