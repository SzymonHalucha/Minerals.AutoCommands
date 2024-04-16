namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommandWriter
    {
        public void WriteDebug(string message);
        public void WriteInfo(string message);
        public void WriteWarning(string message);
        public void WriteError(string message);
    }
}