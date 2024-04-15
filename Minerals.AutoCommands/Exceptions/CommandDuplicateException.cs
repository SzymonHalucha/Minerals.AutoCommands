namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandDuplicateException(ICommandPipeline pipeline, ICommand current, ICommand next)
        : CommandExceptionBase(pipeline, current)
    {
        public override string Message => $"The '{Pipeline.GetUsedAlias(Next)}' command has already been used.";
        public ICommand Next => next;
    }
}