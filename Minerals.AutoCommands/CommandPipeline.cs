namespace Minerals.AutoCommands
{
    public sealed class CommandPipeline : ICommandPipeline
    {
        public string Title { get; private set; }
        public string Version { get; private set; }
        public string MainCommand { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommandParser Parser { get; private set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public StringComparison Comparison { get; private set; }

        private string[] Args { get; set; }
        private Dictionary<Type, Action<Exception>> ExceptionHandlers { get; set; }

        public CommandPipeline(string title, string version, string mainCommand, StringComparison comparison = StringComparison.Ordinal)
        {
            Title = title;
            Version = version;
            MainCommand = mainCommand;
            Comparison = comparison;
            Parser = null!;
            Args = [];
            ExceptionHandlers = [];
        }

        public ICommandPipeline UseExceptionHandler<T>(Action<T> handler) where T : Exception, new()
        {
            ExceptionHandlers.Add(typeof(T), x => handler((T)x));
            return this;
        }

        public ICommandPipeline UseCommandParser<T>() where T : ICommandParser, new()
        {
            Parser = new T();
            return this;
        }

        public ICommandStatement? Evaluate(string[] args)
        {
            try
            {
                Args = args;
                var command = Parser.Parse(args[0], Comparison);
                command?.Evaluate(this, args, 1);
                return command as ICommandStatement;
            }
            catch (Exception exception)
            {
                if (ExceptionHandlers.TryGetValue(exception.GetType(), out var handler))
                {
                    handler(exception);
                }
                else
                {
                    if (!HandleCommandException(exception))
                    {
                        throw;
                    }
                }
            }
            return null;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetUsedAlias(ICommand command)
        {
            return Args.FirstOrDefault(x =>
            {
                return command.Aliases.Any(y =>
                {
                    return y.Equals(x, Comparison);
                });
            }) ?? string.Empty;
        }

        //TODO: Write Tool Help
        public void Help()
        {
            Console.WriteLine($"{GetType().Name}: Pipeline Help");
        }

        private bool HandleCommandException(Exception exception)
        {
            if (exception is CommandExceptionBase cmdException)
            {
                if (cmdException is CommandDuplicateException or CommandValueNotFoundException or CommandNotFoundException)
                {
                    ShowHelpForTool(cmdException);
                }
                else
                {
                    ShowHelpForCommand(cmdException);
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private void ShowHelpForCommand(CommandExceptionBase exception)
        {
            ShowErrorAndUsage(exception);
            Console.WriteLine($"Use '{MainCommand}{GetNestedParentHelp(exception.Current)} --help' to get more information about this command.");
            Console.WriteLine();
        }

        private string GetNestedParentHelp(ICommand? command, string text = "")
        {
            if (command != null)
            {
                text = $"{text} {GetUsedAlias(command)}";
                return GetNestedParentHelp(command.Parent, text);
            }
            return text;
        }

        private void ShowHelpForTool(CommandExceptionBase exception)
        {
            ShowErrorAndUsage(exception);
            Console.WriteLine($"Use '{MainCommand} --help' to get more information about this tool.");
            Console.WriteLine();
        }

        private void ShowErrorAndUsage(CommandExceptionBase exception)
        {
            Console.WriteLine($"ERROR: {exception.Message}");
            Console.WriteLine($"{Title} {Version}");
            Console.WriteLine();
            Console.WriteLine($"USAGE: {MainCommand} [Command] [Options] [Arguments]");
            Console.WriteLine();
        }
    }
}