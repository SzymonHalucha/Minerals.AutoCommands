namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandPipeline
    {
        public ICommand? Evaluate(string[] args, StringComparison comparison);
        public void CustomExceptionHandler<T>(Action<T> exceptionHandler) where T : CommandExceptionBase, new();
    }
}