using Minerals.AutoCommands;

namespace Minerals.Tests
{
    [global::System.Diagnostics.DebuggerNonUserCode]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    public partial class PipelineTestClass : global::Minerals.AutoCommands.ICommandPipeline
    {
        public global::Minerals.AutoCommands.ICommand ParseCommandArgument(string argument)
        {
            switch (argument)
            {
                default:
                return new global::Minerals.AutoCommands.InvalidCommand();
            }
        }
        
        protected global::Minerals.AutoCommands.ICommand EvaluateCommandLine(string[] arguments)
        {
            global::Minerals.AutoCommands.ICommand entryCommand = ParseCommandArgument(arguments[0]);
            entryCommand.Evaluate(this, arguments, 1);
            return entryCommand;
        }
    }
}
