namespace Minerals.AutoCommands.Exceptions
{
    public abstract class CommandExceptionBase(string message) : Exception(message)
    {
        public CommandExceptionBase AddData(params (string Key, object? Value)[] data)
        {
            foreach ((string Key, object? Value) in data)
            {
                Data.Add(Key, Value);
            }
            return this;
        }
    }
}