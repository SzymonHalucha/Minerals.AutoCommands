namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandValueException(ICommandPipeline pipeline, ICommand current, string value)
        : CommandExceptionBase(pipeline, current)
    {
        public override string Message => $"The value '{Value}' is not valid for the argument '{Pipeline.GetUsedAlias(Current)}'.";
        public string Value = value;
    }
}