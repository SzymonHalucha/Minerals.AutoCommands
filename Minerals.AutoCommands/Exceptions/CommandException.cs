namespace Minerals.AutoCommands.Exceptions
{
    public class CommandException(ICommandPipeline pipeline, ICommandStatement current) : Exception
    {
        public ICommandPipeline Pipeline => pipeline;
        public ICommandStatement Current => current;
    }
}