namespace Pandora.Common
{
    /// <summary>
    /// Represents arguments of command line.
    /// </summary>
    public class CommandArguments
    {
        private readonly Dictionary<string, string> _arguments;

        private CommandArguments()
        {
            _arguments = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Creates an instance of <see cref="CommandArguments"/> class from text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>CommandArguments object.</returns>
        public static CommandArguments Create(string text)
        {
            string[] args = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return Create(args);
        }

        /// <summary>
        /// Creates an instance of <see cref="CommandArguments"/> class from argument array.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>CommandArguments object.</returns>
        public static CommandArguments Create(string[] args)
        {
            Ensure.ArgumentIsNotNull(args, nameof(args));

            if ((args.Length % 2) != 0)
            {
                throw new Exception("Invalid arguments");
            }

            CommandArguments output = new CommandArguments();
            for (int i = 0; i < args.Length; i += 2)
            {
                string option = args[i];
                if (!IsOption(option))
                {
                    throw new ArgumentException("usage: -<option>[ <value>] or /<option>[ <value>]");
                }

                string value = args[i + 1];
                output._arguments.Add(option.Substring(1), value);
            }

            return output;
        }

        /// <summary>
        /// Gets option value.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="option">The option name.</param>
        /// <param name="defaultValue">The default value if option is not found.</param>
        /// <returns>Option value.</returns>
        public T GetValue<T>(string option, T defaultValue)
        {
            if (_arguments.TryGetValue(option, out string value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets required option value.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="option">The option name.</param>
        /// <returns>Option value.</returns>
        public T GetValue<T>(string option)
        {
            if (_arguments.TryGetValue(option, out string value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                throw new Exception($"{option} doesn't exist");
            }
        }

        /// <summary>
        /// Gets required option value.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="option">The option name.</param>
        /// <param name="value">The option value.</param>
        /// <returns>True or false.</returns>
        public bool TryGet<T>(string option, out T value)
        {
            if (_arguments.TryGetValue(option, out string text))
            {
                value = (T)Convert.ChangeType(text, typeof(T));
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        /// <summary>
        /// Determines if a text is an option.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>True or false.</returns>
        private static bool IsOption(string text)
        {
            return text[0] == '/' || text[0] == '-';
        }
    }
}
