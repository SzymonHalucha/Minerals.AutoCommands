namespace Minerals.AutoCommands
{
    //TODO: Wirte Handlers implementation
    public static class CommandPipelineHandlers
    {
        public static void HandleException(Exception exception)
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

        public static void CommandDuplicateExceptionHandler(CommandDuplicateException exception)
        {
            Console.WriteLine(exception.Message);
        }

        public static void CommandNotFoundExceptionHandler(CommandNotFoundException exception)
        {
            Console.WriteLine(exception.Message);
        }

        public static void CommandNotSupportedExceptionHandler(CommandNotSupportedException exception)
        {
            Console.WriteLine(exception.Message);
        }

        public static void CommandOrderExceptionHandler(CommandOrderException exception)
        {
            Console.WriteLine(exception.Message);
        }

        public static void CommandRequiredExceptionHandler(CommandRequiredException exception)
        {
            Console.WriteLine(exception.Message);
        }

        public static void CommandValueExceptionHandler(CommandValueException exception)
        {
            Console.WriteLine(exception.Message);
        }

        public static void CommandValueNotFoundExceptionHandler(CommandValueNotFoundException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}