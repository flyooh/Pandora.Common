using System;
using System.IO;

namespace Pandora.Common
{
    public static class Ensure
    {
        public static void ArgumentIsNotNull(object value, string argName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argName);
            }
        }

        public static void ArgumentIsNotNullOrEmpty(string value, string argName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{argName} is null or empty");
            }
        }

        public static void ArgumentIsNotNullOrWhiteSpace(string value, string argName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{argName} is null or whitespace");
            }
        }

        public static void FileExists(string file, string argName)
        {
            Ensure.ArgumentIsNotNullOrEmpty(file, argName);
            if (!File.Exists(file))
            {
                throw new FileNotFoundException(file);
            }
        }

        public static void DirectoryExists(string directory, string argName)
        {
            Ensure.ArgumentIsNotNullOrEmpty(directory, argName);
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
