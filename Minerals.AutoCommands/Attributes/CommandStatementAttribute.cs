namespace Minerals.AutoCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CommandStatementAttribute(params string[] aliases) : Attribute
    {
        public string[] Aliases { get; } = aliases;
    }
}