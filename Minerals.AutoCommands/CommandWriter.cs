namespace Minerals.AutoCommands
{
    public sealed class CommandWriter : ICommandWriter
    {
        public int TextIndentation { get; set; } = 2;

        public void WriteDebug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(message);
            Console.ResetColor();
        }

        public void WriteLineDebug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WriteInfo(string message)
        {
            Console.Write(message);
            Console.ResetColor();
        }

        public void WriteLineInfo(string message)
        {
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);
            Console.ResetColor();
        }

        public void WriteLineWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
            Console.ResetColor();
        }

        public void WriteLineError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void WriteHelpForException(CommandException exception)
        {
            var command = exception is not CommandNotSupportedException ? exception.Current : null;
            WriteLineError($"ERROR: {exception.Message}");
            WriteLineInfo($"{exception.Pipeline.Title} {exception.Pipeline.Version}");
            Console.WriteLine();
            WriteCommandUsageInfo(exception.Pipeline, command);
            Console.WriteLine();
            WriteHelpMessageForCommand(exception.Pipeline, command);
            Console.WriteLine();
        }

        public void WriteHelpForPipeline(ICommandPipeline pipeline)
        {
            WriteToolHeader(pipeline);
            Console.WriteLine();
            WriteAllCommandsGroupsFormatedDescription(pipeline.PossibleArguments);
            WriteHelpMessageForCommandList(pipeline, null);
            Console.WriteLine();
        }

        public void WriteHelpForCommand(ICommandPipeline pipeline, ICommandStatement command)
        {
            WriteToolHeader(pipeline);
            Console.WriteLine();
            WriteCommandUsageInfo(pipeline, command);
            Console.WriteLine();
            WriteAllCommandsGroupsFormatedDescription(command.PossibleArguments);
            WriteHelpMessageForCommandList(pipeline, command);
            Console.WriteLine();
        }

        private void WriteToolHeader(ICommandPipeline pipeline)
        {
            WriteLineInfo($"{pipeline.Title} {pipeline.Version}");
        }

        private void WriteCommandUsageInfo(ICommandPipeline pipeline, ICommandStatement? command)
        {
            if (command is null)
            {
                WriteLineInfo($"Usage: {pipeline.MainCommand} [Options]");
                return;
            }
            if (command.Usage.Equals(string.Empty) is false)
            {
                WriteLineInfo($"USAGE: {command.Usage}");
                return;
            }
            var aliases = GetAncestorCommandsAliases(pipeline, command);
            var useValue = command.ValueRequired ? "[Value] " : string.Empty;
            WriteLineInfo($"Usage: {pipeline.MainCommand}{aliases} {useValue}[Options]");
        }

        private void WriteAllCommandsGroupsFormatedDescription(Type[] possibleArguments)
        {
            var groups = possibleArguments.Select(x => (ICommandStatement)Activator.CreateInstance(x)).ToArray();
            var longestAlias = groups.Max(x => x.Aliases.Sum(y => y.Length) + x.Aliases.Length - 1);
            foreach (var group in groups.GroupBy(x => x.Group))
            {
                WriteCommandGroupFormatedDescription(group, longestAlias);
                Console.WriteLine();
            }
        }

        private void WriteCommandGroupFormatedDescription(IGrouping<string, ICommandStatement> group, int longestAlias)
        {
            WriteLineInfo($"{group.Key}:");
            foreach (var cmd in group)
            {
                WriteCommandFormatedDescription(cmd, longestAlias);
            }
        }

        private void WriteCommandFormatedDescription(ICommandStatement command, int longestAlias)
        {
            var leftPad = string.Empty.PadLeft(TextIndentation);
            var formated = string.Join("|", command.Aliases);
            var rightPad = string.Empty.PadRight(longestAlias - formated.Length + TextIndentation);
            Console.Write(leftPad);
            WriteInfo(formated);
            Console.Write(rightPad);
            WriteLineInfo(command.Description);
        }

        private void WriteHelpMessageForCommand(ICommandPipeline pipeline, ICommandStatement? command)
        {
            if (command is null)
            {
                WriteLineInfo($"Use '{pipeline.MainCommand} --help' for more information about this tool.");
                return;
            }
            var aliases = GetAncestorCommandsAliases(pipeline, command);
            WriteLineInfo($"Use '{pipeline.MainCommand}{aliases} --help' for more information about the selected command.");
        }

        private void WriteHelpMessageForCommandList(ICommandPipeline pipeline, ICommandStatement? command)
        {
            var aliases = GetAncestorCommandsAliases(pipeline, command);
            WriteLineInfo($"Use '{pipeline.MainCommand}{aliases} [command] --help' for more information about the selected command.");
        }

        private string GetAncestorCommandsAliases(ICommandPipeline pipeline, ICommandStatement? command)
        {
            ICommandStatement? current = command;
            string text = string.Empty;
            while (current is not null)
            {
                text = $" {pipeline.GetUsedCommandAlias(current)}{text}";
                current = current.Previous;
            }
            return text;
        }

        // private void ShowHelpForCommand(CommandException exception)
        // {
        //     ShowErrorAndUsage(exception);

        //     Writer.WriteInfo("");
        // }

        // private void ShowHelpForTool(Exception exception)
        // {
        //     ShowErrorAndUsage(exception);
        //     Writer.WriteInfo($"Use '{MainCommand} --help' to get more information about this tool.");
        //     Writer.WriteInfo("");
        // }

        // private void ShowErrorAndUsage(Exception exception)
        // {
        //     Writer.WriteError($"ERROR: {exception.Message}");
        //     Writer.WriteInfo($"{Title} {Version}");
        //     Writer.WriteInfo("");
        //     Writer.WriteInfo($"USAGE: {MainCommand} [Command] [Options] [Arguments]");
        //     Writer.WriteInfo("");
        // }

        // [EditorBrowsable(EditorBrowsableState.Never)]
        // public void DisplayHelpForCommand(ICommandStatement command)
        // {
        //     var groups = command.PossibleArguments.Select(x => (ICommandStatement)Activator.CreateInstance(x)).ToArray();
        //     var longestAlias = groups.Max(x => x.Aliases.Sum(y => y.Length) + x.Aliases.Length);
        //     var builder = new StringBuilder();
        //     AppendToolHeader(builder);
        //     AppendCommand(builder, command);
        //     foreach (var group in groups.GroupBy(x => x.Group))
        //     {
        //         AppendCommandGroup(builder, longestAlias, group);
        //     }
        //     Writer.WriteInfo(builder.ToString());
        // }

        // private void AppendToolHeader(StringBuilder builder)
        // {
        //     builder.Append(Title);
        //     builder.Append(" ");
        //     builder.Append(Version);
        //     builder.AppendLine("\n");
        // }

        // private void AppendCommand(StringBuilder builder, ICommandStatement command)
        // {
        //     builder.Append(nameof(command.Description));
        //     builder.Append(':');
        //     builder.AppendLine();
        //     builder.Append(' ', TextIndentation);
        //     builder.Append(command.Description);
        //     builder.AppendLine();
        // }

        // private void AppendCommandGroup(StringBuilder builder, int longestAlias, IGrouping<string, ICommandStatement> group)
        // {
        //     builder.AppendLine();
        //     builder.Append(group.Key);
        //     builder.Append(':');
        //     foreach (var cmd in group)
        //     {
        //         builder.AppendLine();
        //         builder.Append(' ', TextIndentation);
        //         builder.Append(cmd.Aliases[0]);
        //         foreach (var alias in cmd.Aliases.Skip(1))
        //         {
        //             builder.Append('|');
        //             builder.Append(alias);
        //         }
        //         var size = cmd.Aliases.Sum(x => x.Length) + cmd.Aliases.Length - TextIndentation;
        //         builder.Append(' ', longestAlias - size);
        //         builder.Append(cmd.Description);
        //     }
        //     builder.AppendLine();
        // }
    }
}