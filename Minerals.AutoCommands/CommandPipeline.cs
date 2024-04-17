namespace Minerals.AutoCommands
{
    public sealed class CommandPipeline(string title, string version, string mainCommand) : ICommandPipeline
    {
        public string Title { get; } = title;
        public string Version { get; } = version;
        public string MainCommand { get; } = mainCommand;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommandParser Parser { get; private set; } = null!;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommandWriter Writer { get; private set; } = new CommandWriter();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public StringComparison Comparison { get; private set; } = StringComparison.Ordinal;

        private string[] _args = [];
        private string[] _helpAliases = ["-h1", "--help1"]; //TODO: Change this to normal
        private readonly Dictionary<Type, Action<Exception>> _handlers = [];

        public ICommandPipeline UseExceptionHandler<T>(Action<T> handler) where T : Exception, new()
        {
            _handlers.Add(typeof(T), x => handler((T)x));
            return this;
        }

        public ICommandPipeline UseCommandParser<T>() where T : ICommandParser, new()
        {
            Parser = new T();
            return this;
        }

        public ICommandPipeline UseCommandParser(ICommandParser parser)
        {
            Parser = parser;
            return this;
        }

        public ICommandPipeline UseCommandWriter<T>() where T : ICommandWriter, new()
        {
            Writer = new T();
            return this;
        }

        public ICommandPipeline UseCommandWriter(ICommandWriter writer)
        {
            Writer = writer;
            return this;
        }

        public ICommandPipeline UseCommandHelpAliases(string[] aliases)
        {
            _helpAliases = aliases;
            return this;
        }

        public ICommandPipeline UseStringComparison(StringComparison comparison)
        {
            Comparison = comparison;
            return this;
        }

        //TODO: Write Tool Help
        public ICommandPipeline DisplayCommandList()
        {
            Writer.WriteInfo($"{GetType().Name}: Pipeline Help");
            return this;
        }

        public ICommandStatement? Evaluate(string[] args)
        {
            _args = args;
            try
            {
                if (args.Length <= 0)
                {
                    throw new CommandNotFoundException(this);
                }

                if (CheckCommandHelp(args, 0))
                {
                    DisplayCommandList();
                    return null;
                }

                if (Parser.Parse(args[0], Comparison) is ICommandStatement command)
                {
                    var state = command.Evaluate(null, this, args, 1);
                    return state ? command : null;
                }

                throw new CommandNotFoundException(this);
            }
            catch (Exception exception)
            {
                if (_handlers.TryGetValue(exception.GetType(), out var handler))
                {
                    handler(exception);
                }

                if (!HandleCommandException(exception))
                {
                    throw;
                }
            }

            return null;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetUsedCommandAlias(ICommandStatement? command)
        {
            return _args.FirstOrDefault(x =>
            {
                return command?.Aliases.Any(y =>
                {
                    return y.Equals(x, Comparison);
                }) == true;
            }) ?? string.Empty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CheckCommandHelp(string[] args, int index)
        {
            if (index >= args.Length)
            {
                return false;
            }
            return _helpAliases.Any(y => y.Equals(args[index], Comparison));
        }

        private bool HandleCommandException(Exception exception)
        {
            if (exception is CommandNotFoundException)
            {
                ShowHelpForTool(exception);
                return true;
            }

            if (exception is CommandException cmdException)
            {
                ShowHelpForCommand(cmdException);
                return true;
            }

            return false;
        }

        private void ShowHelpForCommand(CommandException exception)
        {
            ShowErrorAndUsage(exception);
            var aliases = GetAncestorCommandsAliases(exception.Current);
            Writer.WriteInfo($"Use '{MainCommand}{aliases} --help' to get more information about this command.");
            Writer.WriteInfo("");
        }

        private void ShowHelpForTool(Exception exception)
        {
            ShowErrorAndUsage(exception);
            Writer.WriteInfo($"Use '{MainCommand} --help' to get more information about this tool.");
            Writer.WriteInfo("");
        }

        private void ShowErrorAndUsage(Exception exception)
        {
            Writer.WriteError($"ERROR: {exception.Message}");
            Writer.WriteInfo($"{Title} {Version}");
            Writer.WriteInfo("");
            Writer.WriteInfo($"USAGE: {MainCommand} [Command] [Options] [Arguments]");
            Writer.WriteInfo("");
        }

        private string GetAncestorCommandsAliases(ICommandStatement? command, string text = "")
        {
            ICommandStatement? current = command;
            while (current != null)
            {
                text = $" {GetUsedCommandAlias(current)}{text}";
                current = current.Previous;
            }
            return text;
        }
    }
}