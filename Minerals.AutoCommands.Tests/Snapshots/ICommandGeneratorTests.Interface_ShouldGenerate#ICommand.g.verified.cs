namespace Minerals.AutoCommands
{
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    public interface ICommand
    {
        void Evaluate(global::Minerals.AutoCommands.ICommandPipeline pipeline, string[] arguments, int index);
        void ShowHelp();
        bool Execute();
    }
}
