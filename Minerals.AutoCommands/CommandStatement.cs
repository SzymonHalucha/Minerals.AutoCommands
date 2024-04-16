namespace Minerals.AutoCommands
{
    public abstract class CommandStatement : ICommandStatement
    {
        public abstract string[] Aliases { get; }
        public abstract string Description { get; }
        public virtual string Group { get; } = "Commands";
        public virtual bool RequireValue { get; } = false;

        public virtual Regex PossibleValues { get; } = new Regex(".");
        public virtual Type[] RequireArguments { get; } = [];
        public abstract Type[] PossibleArguments { get; }

        public List<ICommandStatement> Arguments { get; } = [];

        public string? Value
        {
            get
            {
                return _value;
            }
            protected set
            {
                _value = value;
            }
        }

        public ICommandStatement? Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent?.Arguments.Remove(this);
                _parent = value;
                _parent?.Arguments.Add(this);
            }
        }

        public ICommandWriter? Writer
        {
            get
            {
                return _writer;
            }
            set
            {
                _writer = value;
            }
        }

        private string? _value = null;
        private ICommandStatement? _parent = null;
        private ICommandWriter? _writer = null;

        public abstract void Execute();

        public virtual void Evaluate(ICommandPipeline pipeline, string[] args, int index)
        {
            Writer ??= pipeline.Writer;
            if (RequireValue)
            {
                Value = GetCommandValue(pipeline, args, index);
                index++;
            }
            if (index >= args.Length)
            {
                CheckRequireArguments(pipeline);
                return;
            }
            var next = GetNextCommand(pipeline, args, index);
            next.Evaluate(pipeline, args, ++index);
            CheckRequireArguments(pipeline);
        }

        public virtual void DisplayHelp()
        {
            Writer?.WriteInfo($"{GetType().Name}: Help");
        }

        protected string GetCommandValue(ICommandPipeline pipeline, string[] args, int index)
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
                throw new CommandValueInvalidException(pipeline, this, args[index]);
            }
            return args[index];
        }

        protected ICommandStatement GetNextCommand(ICommandPipeline pipeline, string[] args, int index)
        {
            var next = pipeline.Parser.Parse(args[index], pipeline.Comparison);
            if (next == null)
            {
                throw new CommandNotFoundException(pipeline, this, args[index]);
            }
            if (!PossibleArguments.Contains(next.GetType()))
            {
                throw new CommandNotSupportedException(pipeline, this, next);
            }
            if (Arguments.Any(x => x.GetType() == next.GetType()))
            {
                throw new CommandDuplicateException(pipeline, this, next);
            }
            next.Parent = this;
            return next;
        }

        protected void CheckRequireArguments(ICommandPipeline pipeline)
        {
            foreach (var arg in RequireArguments)
            {
                if (!Arguments.Any(x => x.GetType() == arg))
                {
                    throw new CommandRequiredException(pipeline, this, arg);
                }
            }
        }
    }
}