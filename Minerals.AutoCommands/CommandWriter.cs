namespace Minerals.AutoCommands
{
    public sealed class CommandWriter : ICommandWriter
    {
        public void WriteDebug(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteInfo(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }
}