namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandArgumentRequiredException(ICommandPipeline pipeline, ICommandStatement? current, Type type)
        : CommandException(pipeline, current)
    {
        public override string Message => $"The '{((ICommandStatement)Activator.CreateInstance(type)).Aliases[0]}' argument is required.";
        public Type Type => type;
    }
}