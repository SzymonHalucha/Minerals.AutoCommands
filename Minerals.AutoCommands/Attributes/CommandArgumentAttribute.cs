namespace Minerals.AutoCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CommandArgumentAttribute(params string[] aliases) : Attribute
    {
        public string[] Aliases { get; } = aliases;
    }
}