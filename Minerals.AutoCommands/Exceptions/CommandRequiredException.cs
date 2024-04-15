namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandRequiredException(ICommandPipeline pipeline, ICommand current, Type type)
        : CommandExceptionBase(pipeline, current)
    {
        public override string Message => $"The '{Type.Name}' argument type is required.";
        public Type Type => type;
    }
}