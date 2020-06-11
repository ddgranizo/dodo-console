using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dodo.Console.Utilities
{
    public static class ParseUtilities
    {
        private static string[] availableTrueValues = new string[] { "true", "yes", "1" };
        private static string[] availableFalseValues = new string[] { "false", "no", "0" };

        public static object ParseBasicValueString(string valueString, Type type)
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
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var dataType = type.GetGenericArguments().First();
                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(dataType);
                    var instance = Activator.CreateInstance(constructedListType);
                    var methodInfo = type.GetMethod("Add");
                    foreach (var item in valueString.Split(new char[] { ';' }))
                    {
                        var value = ParseBasicValueString(item, dataType);
                        methodInfo.Invoke(instance, new object[] { value });
                    }
                    return instance;
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

    }
}
