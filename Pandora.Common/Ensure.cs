namespace Pandora.Common
{
    public static class Ensure
    {
        public static void ArgumentIsNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void ArgumentIsNotNullOrEmpty(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{paramName} is null or empty");
            }
        }

        public static void ArgumentIsNotNullOrWhiteSpace(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{paramName} is null or whitespace");
            }
        }

        public static void FileExists(string file, string paramName)
        {
            Ensure.ArgumentIsNotNullOrEmpty(file, paramName);
            if (!File.Exists(file))
            {
                throw new FileNotFoundException(file);
            }
        }

        public static void DirectoryExists(string directory, string paramName)
        {
            Ensure.ArgumentIsNotNullOrEmpty(directory, paramName);
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(directory);
            }
        }

        public static void Assert(bool condition, string format, params object[] args)
        {
            if (!condition)
            {
                throw new ArgumentOutOfRangeException(string.Format(format, args));
            }
        }
    }
}
