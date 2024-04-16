namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandNotSupportedException(ICommandPipeline pipeline, ICommandStatement current, ICommandStatement next)
        : CommandException(pipeline, current)
    {
        public override string Message => $"The argument named '{Pipeline.GetUsedAlias(Next)}' is not valid for the command named '{Pipeline.GetUsedAlias(Current)}'.";
        public ICommandStatement Next => next;
    }
}