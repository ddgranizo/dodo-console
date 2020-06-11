using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dodo.Console.Utilities
{
    public static class ParameterUtilities
    {
        public static object GetParameterValue(string parameter, Type type, List<string> args)
        {
            if (ParameterHasValue(parameter, args))
            {
                var stringValue = GetStringParameterValue(parameter, args);
                return ParseUtilities.ParseBasicValueString(stringValue, type);
            }
            else if (type == typeof(bool))
            {
                return true;
            }
            throw new InvalidOperationException($"Can't find value for parameter {GetNormalizedParameterName(parameter)}");
        }

        private static string GetStringParameterValue(string parameter, List<string> args)
        {
            var item = args.FirstOrDefault(k => GetNormalizedParameterName(k) == GetNormalizedParameterName(parameter));
            var index = args.IndexOf(item);
            return args[index + 1];
        }

        private static bool ParameterHasValue(string parameter, List<string> args)
        {
            var item = args.FirstOrDefault(k => GetNormalizedParameterName(k) == GetNormalizedParameterName(parameter));
            if (string.IsNullOrEmpty(item))
            {
                return false;
            }
            var index = args.IndexOf(item);
            return index < args.Count
                && !IsKeyCandidate(args[index + 1]);
        }

        private static string GetNormalizedParameterName(string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            var builder = new StringBuilder();
            if (parameter.First() != '-')
            {
                builder.Append("-");
            }
            builder.Append(parameter.ToLower());
            return builder.ToString();
        }

        private static bool IsKeyCandidate(string parameter) =>
            !string.IsNullOrEmpty(parameter)
                && parameter.First() == '-'
                && parameter.Length > 1;

        public static bool IsPropertyInArgs(string propertyName, List<string> args) =>
            args.Any(k =>
                IsKeyCandidate(k)
                && GetNormalizedParameterName(k) == GetNormalizedParameterName(propertyName));
    }
}
