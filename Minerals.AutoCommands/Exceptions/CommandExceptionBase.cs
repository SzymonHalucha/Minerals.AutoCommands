namespace Minerals.AutoCommands.Exceptions
{
    public abstract class CommandExceptionBase(ICommandPipeline pipeline, ICommand current) : Exception
    {
        public ICommandPipeline Pipeline => pipeline;
        public ICommand Current => current;
    }
}