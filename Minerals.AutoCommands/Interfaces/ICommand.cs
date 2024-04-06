namespace Minerals.AutoCommands.Interfaces
{
    public interface ICommand
    {
        public string Description { get; }
        public string Usage { get; }

        public ICommandStatement? Parent { get; set; }
        public bool Evaluate(string[] args, int index, StringComparison comparison);
    }
}