namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandDuplicateException(ICommandPipeline pipeline, ICommandStatement current, ICommandStatement next)
        : CommandException(pipeline, current)
    {
        public override string Message => $"The '{Pipeline.GetUsedAlias(Next)}' command has already been used.";
        public ICommandStatement Next => next;
    }
}