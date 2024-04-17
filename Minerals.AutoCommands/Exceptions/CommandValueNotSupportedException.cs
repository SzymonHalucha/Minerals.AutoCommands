namespace Minerals.AutoCommands.Exceptions
{
    public sealed class CommandValueNotSupportedException(ICommandPipeline pipeline, ICommandStatement? current, string value)
        : CommandException(pipeline, current)
    {
        public override string Message => $"The value '{Value}' is not valid for the argument '{Pipeline.GetUsedCommandAlias(Current)}'.";
        public string Value = value;
    }
}