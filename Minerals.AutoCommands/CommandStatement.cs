namespace Minerals.AutoCommands
{
    public abstract class CommandStatement : ICommandStatement
    {
        public abstract string[] Aliases { get; }
        public abstract string Description { get; }
        public virtual string Usage { get; } = string.Empty;
        public virtual string Group { get; } = "Options";
        public virtual Regex PossibleValues { get; } = new Regex(".");
        public abstract Type[] PossibleArguments { get; }

        public virtual bool ValueRequired { get; } = false;
        public virtual Type[] ArgumentsRequired { get; } = [];

        public ICommandStatement? Previous { get; protected set; } = null;
        public ICommandStatement? Next { get; protected set; } = null;
        public string? Value { get; protected set; } = null;

        protected ICommandWriter Writer { get; set; } = null!;

        public abstract bool Execute(Dictionary<object, object>? data = null);

        public virtual bool Evaluate(ICommandStatement? previous, ICommandPipeline pipeline, string[] args, int index)
        {
            Previous = previous;
            Writer ??= pipeline.Writer;
            if (pipeline.CheckCommandHelp(args, index))
            {
                Writer.WriteHelpForCommand(pipeline, this);
                return false;
            }
            if (ValueRequired && CheckCommandValue(pipeline, args, index))
            {
                Value = args[index];
                index++;
            }
            if (index >= args.Length && CheckArgumentsRequired(pipeline))
            {
                return true;
            }
            var next = GetNextCommand(pipeline, args, index);
            var state = next.Evaluate(this, pipeline, args, ++index);
            return state && CheckArgumentsRequired(pipeline);
        }

        public virtual IEnumerable<ICommandStatement> AncestorCommands()
        {
            ICommandStatement? current = Previous;
            while (current != null)
            {
                yield return current;
                current = current.Previous;
            }
        }

        public virtual IEnumerable<ICommandStatement> DescendantCommands()
        {
            ICommandStatement? current = Next;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        protected bool CheckCommandValue(ICommandPipeline pipeline, string[] args, int index)
        {
            if (index >= args.Length)
            {
                throw new CommandValueNotFoundException(pipeline, this);
            }
            if (pipeline.CheckDescendantCommandsHelp(args, index))
            {
                return false;
            }
            if (pipeline.Parser.IsAlias(args[index], pipeline.Comparison))
            {
                throw new CommandValueRequiredException(pipeline, this);
            }
            if (!PossibleValues.IsMatch(args[index]))
            {
                throw new CommandValueNotSupportedException(pipeline, this, args[index]);
            }
            return true;
        }

        protected ICommandStatement GetNextCommand(ICommandPipeline pipeline, string[] args, int index)
        {
            var next = pipeline.Parser.Parse(args[index], pipeline.Comparison);
            if (next == null)
            {
                throw new CommandArgumentNotFoundException(pipeline, this, args[index]);
            }
            if (!PossibleArguments.Contains(next.GetType()))
            {
                throw new CommandArgumentNotSupportedException(pipeline, this, next);
            }
            Next = next;
            return next;
        }

        protected bool CheckArgumentsRequired(ICommandPipeline pipeline)
        {
            foreach (var arg in ArgumentsRequired)
            {
                if (!DescendantCommands().Any(x => x.GetType() == arg))
                {
                    throw new CommandArgumentRequiredException(pipeline, this, arg);
                }
            }
            return true;
        }
    }
}