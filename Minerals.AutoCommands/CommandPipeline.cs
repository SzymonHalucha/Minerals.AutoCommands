namespace Minerals.AutoCommands
{
    public class CommandPipeline : ICommandPipeline
    {
        private readonly Dictionary<Type, Action<Exception>> _exceptionsHandlers = [];

        public ICommand? Evaluate(string[] args, StringComparison comparison)
        {
            try
            {
                ICommand? command = CommandParser.Parse(args[0], comparison);
                command?.Evaluate(args, 1, comparison);
                return command;
            }
            catch (Exception exception)
            {
                if (_exceptionsHandlers.TryGetValue(exception.GetType(), out var handler))
                {
                    handler(exception);
                }
                else
                {
                    DefaultExceptionHandler(exception);
                }
            }
            return null;
        }

        public void CustomExceptionHandler<T>(Action<T> exceptionHandler) where T : CommandExceptionBase, new()
        {
            _exceptionsHandlers.Add(typeof(T), exception => exceptionHandler((T)exception));
        }

        private void DefaultExceptionHandler(Exception exception)
        {
            switch (exception)
            {
                case CommandDuplicateException duplicate:
                    CommandDuplicateExceptionHandler(duplicate);
                    break;
                case CommandNotFoundException notFound:
                    CommandNotFoundExceptionHandler(notFound);
                    break;
                case CommandNotSupportedException notSupported:
                    CommandNotSupportedExceptionHandler(notSupported);
                    break;
                case CommandOrderException order:
                    CommandOrderExceptionHandler(order);
                    break;
                case CommandRequiredException required:
                    CommandRequiredExceptionHandler(required);
                    break;
                case CommandValueException value:
                    CommandValueExceptionHandler(value);
                    break;
                case CommandValueNotFoundException valueNotFound:
                    CommandValueNotFoundExceptionHandler(valueNotFound);
                    break;
            }
        }

        private void CommandDuplicateExceptionHandler(CommandDuplicateException exception)
        {
            Console.WriteLine(exception.Message);
        }

        private void CommandNotFoundExceptionHandler(CommandNotFoundException exception)
        {
            Console.WriteLine(exception.Message);
        }

        private void CommandNotSupportedExceptionHandler(CommandNotSupportedException exception)
        {
            Console.WriteLine(exception.Message);
        }

        private void CommandOrderExceptionHandler(CommandOrderException exception)
        {
            Console.WriteLine(exception.Message);
        }

        private void CommandRequiredExceptionHandler(CommandRequiredException exception)
        {
            Console.WriteLine(exception.Message);
        }

        private void CommandValueExceptionHandler(CommandValueException exception)
        {
            Console.WriteLine(exception.Message);
        }

        private void CommandValueNotFoundExceptionHandler(CommandValueNotFoundException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}