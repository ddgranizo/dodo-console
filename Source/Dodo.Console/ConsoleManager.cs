using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dodo.Console
{
    public static class ConsoleManager
    {

        private static string[] availableTrueValues = new string[] { "true", "yes", "1" };
        private static string[] availableFalseValues = new string[] { "false", "no", "0" };

        public static T GetInitialData<T>(params string[] args)
        {
            var argsList = args.ToList();
            var instance = (T)Activator.CreateInstance(typeof(T));
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var name = property.Name;
                var type = property.PropertyType;
                if (IsPropertyInArgs(name, argsList))
                {
                    var parameterValue = GetParameterValue(name, type, argsList);
                    property.SetValue(instance, parameterValue);
                }
            }
            return instance;
        }

        private static object GetParameterValue(string parameter, Type type, List<string> args)
        {
            if (ParameterHasValue(parameter, args))
            {
                var stringValue = GetStringParameterValue(parameter, args);
                return ParseString(stringValue, type);
            }
            else if (type == typeof(bool))
            {
                return true;
            }
            throw new InvalidOperationException($"Can't find value for parameter {GetNormalizedParameterName(parameter)}");
        }

        private static object ParseString(string valueString, Type type)
        {
            try
            {
                if (type == typeof(string))
                {
                    return valueString;
                }
                else if (type == typeof(bool))
                {
                    if (availableTrueValues.ToList().IndexOf(valueString.ToLower()) > -1)
                    {
                        return true;
                    }
                    else if (availableFalseValues.ToList().IndexOf(valueString.ToLower()) > -1)
                    {
                        return false;
                    }
                    throw new InvalidCastException();
                }
                else if (type == typeof(int))
                {
                    return int.Parse(valueString);
                }
                else if (type == typeof(decimal))
                {
                    return decimal.Parse(valueString.Replace(".", ","));
                }
                else if (type == typeof(double))
                {
                    return double.Parse(valueString.Replace(".", ","));
                }
                else if (type == typeof(Guid))
                {
                    return Guid.Parse(valueString);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception)
            {
                throw new Exception($"Error parsing '{valueString}' into a '{type.Name}' value");
            }
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

        private static bool IsPropertyInArgs(string propertyName, List<string> args) =>
            args.Any(k =>
                IsKeyCandidate(k)
                && GetNormalizedParameterName(k) == GetNormalizedParameterName(propertyName));

    }
}
