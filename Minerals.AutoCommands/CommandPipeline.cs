namespace Minerals.AutoCommands
{
    public sealed class CommandPipeline : ICommandPipeline
    {
        public string Title { get; }
        public string Version { get; }
        public string MainCommand { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommandWriter Writer { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ICommandParser Parser { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public StringComparison Comparison { get; private set; }

        private string[] Args { get; set; }
        private Dictionary<Type, Action<Exception>> ExceptionHandlers { get; set; }

        public CommandPipeline(string title, string version, string mainCommand)
        {
            Title = title;
            Version = version;
            MainCommand = mainCommand;
            Writer = new CommandWriter();
            Parser = null!;
            Comparison = StringComparison.Ordinal;
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

        public ICommandPipeline UseCommandWriter<T>() where T : ICommandWriter, new()
        {
            Writer = new T();
            return this;
        }

        public ICommandPipeline UseStringComparison(StringComparison comparison)
        {
            Comparison = comparison;
            return this;
        }

        public ICommandStatement Evaluate(string[] args)
        {
            try
            {
                Args = args;
                var command = Parser.Parse(args[0], Comparison);
                command.Evaluate(this, args, 1);
                return command;
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
            return null!;
        }

        //TODO: Write Tool Help
        public void DisplayHelp()
        {
            Writer.WriteInfo($"{GetType().Name}: Pipeline Help");
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetUsedAlias(ICommandStatement command)
        {
            return Args.FirstOrDefault(x =>
            {
                return command.Aliases.Any(y =>
                {
                    return y.Equals(x, Comparison);
                });
            }) ?? string.Empty;
        }

        private bool HandleCommandException(Exception exception)
        {
            if (exception is CommandException cmdException)
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

        private void ShowHelpForCommand(CommandException exception)
        {
            ShowErrorAndUsage(exception);
            Writer.WriteInfo($"Use '{MainCommand}{GetNestedParentHelp(exception.Current)} --help' to get more information about this command.");
            Writer.WriteInfo("");
        }

        private string GetNestedParentHelp(ICommandStatement? command, string text = "")
        {
            if (command != null)
            {
                text = $" {GetUsedAlias(command)}{text}";
                return GetNestedParentHelp(command.Parent, text);
            }
            return text;
        }

        private void ShowHelpForTool(CommandException exception)
        {
            ShowErrorAndUsage(exception);
            Writer.WriteInfo($"Use '{MainCommand} --help' to get more information about this tool.");
            Writer.WriteInfo("");
        }

        private void ShowErrorAndUsage(CommandException exception)
        {
            Writer.WriteError($"ERROR: {exception.Message}");
            Writer.WriteInfo($"{Title} {Version}");
            Writer.WriteInfo("");
            Writer.WriteInfo($"USAGE: {MainCommand} [Command] [Options] [Arguments]");
            Writer.WriteInfo("");
        }
    }
}