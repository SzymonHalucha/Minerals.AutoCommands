#pragma warning disable CS9113
namespace Minerals.AutoCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CommandStatementAttribute(params string[] aliases) : Attribute;
}
#pragma warning restore CS9113