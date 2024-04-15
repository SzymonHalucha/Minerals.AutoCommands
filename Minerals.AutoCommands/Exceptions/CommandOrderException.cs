namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandOrderException(ICommandPipeline pipeline, ICommand current, ICommand next)
        : CommandExceptionBase(pipeline, current)
    {
        public override string Message => $"The command statement '{Pipeline.GetUsedAlias(Next)}' cannot occur after the command argument ('{Pipeline.GetUsedAlias(Current)}').";
        public ICommand Next => next;
    }
}