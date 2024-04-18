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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Type[] PossibleArguments { get; private set; } = [];

        private string[] _args = [];
        private string[] _helpAliases = ["-h1", "--help1"]; //TODO: Change this to normal -h --help
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

        public ICommandPipeline UseCommandWriter<T>(int textIndentation = 2) where T : ICommandWriter, new()
        {
            Writer = new T() { TextIndentation = textIndentation };
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

        public ICommandPipeline UsePossibleArguments(params Type[] possibleArguments)
        {
            PossibleArguments = possibleArguments;
            return this;
        }

        public ICommandStatement? Evaluate(string[] args)
        {
            _args = args;
            try
            {
                if (args.Length is 0)
                {
                    throw new CommandNotFoundException(this);
                }
                else if (CheckCommandHelp(args, 0))
                {
                    Writer.WriteHelpForPipeline(this);
                    return null;
                }
                else if (Parser.Parse(args[0], Comparison) is ICommandStatement command)
                {
                    if (PossibleArguments.Contains(command.GetType()) is false)
                    {
                        throw new CommandNotSupportedException(this, command);
                    }
                    var state = command.Evaluate(null, this, args, 1);
                    return state ? command : null;
                }
                else
                {
                    throw new CommandNotFoundException(this);
                }
            }
            catch (Exception exception)
            {
                if (_handlers.TryGetValue(exception.GetType(), out var handler))
                {
                    handler(exception);
                }
                else if (exception is CommandException cmdException)
                {
                    Writer.WriteHelpForException(cmdException);
                }
                else
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
                }) is true;
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CheckDescendantCommandsHelp(string[] args, int index)
        {
            if (index >= args.Length)
            {
                return false;
            }
            return _helpAliases.Any(x => args.Skip(index + 1).Any(y => y.Equals(x, Comparison)));
        }
    }
}