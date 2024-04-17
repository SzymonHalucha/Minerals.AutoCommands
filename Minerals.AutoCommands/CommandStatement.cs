namespace Minerals.AutoCommands
{
    public abstract class CommandStatement : ICommandStatement
    {
        public abstract string[] Aliases { get; }
        public abstract string Description { get; }
        public virtual string Group { get; } = "Options";
        public virtual Regex PossibleValues { get; } = new Regex(".");
        public abstract Type[] PossibleArguments { get; }

        public virtual bool ValueRequired { get; } = false;
        public virtual Type[] ArgumentsRequired { get; } = [];

        public ICommandStatement? Previous { get; protected set; }
        public ICommandStatement? Next { get; protected set; }
        public string? Value { get; protected set; }

        protected ICommandWriter Writer { get; set; } = null!;

        public abstract bool Execute(Dictionary<object, object>? data = null);

        public virtual bool Evaluate(ICommandStatement? previous, ICommandPipeline pipeline, string[] args, int index)
        {
            Previous = previous;
            Writer ??= pipeline.Writer;
            if (pipeline.CheckCommandHelp(args, index))
            {
                DisplayHelp();
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

        //TODO: Write help
        public virtual void DisplayHelp()
        {
            Writer.WriteInfo($"{GetType().Name}: Help");
        }

        protected bool CheckCommandValue(ICommandPipeline pipeline, string[] args, int index)
        {
            if (index >= args.Length)
            {
                throw new CommandValueNotFoundException(pipeline, this);
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