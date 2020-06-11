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
                    return GetGenericListImplementation(valueString, type);
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return GetGenericDictionaryImplementation(valueString, type);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing '{valueString}' into a '{type.Name}' value. Error info: {ex.Message}");
            }
        }

        private static object GetGenericDictionaryImplementation(string valueString, Type type)
        {
            var dataKeyType = type.GetGenericArguments().First();
            var dataValueType = type.GetGenericArguments().Last();
            var dictionaryType = typeof(Dictionary<,>);
            var constructedDictionaryType = dictionaryType.MakeGenericType(dataKeyType, dataValueType);
            var instance = Activator.CreateInstance(constructedDictionaryType);
            var methodInfo = type.GetMethod("Add");
            foreach (var item in valueString.Split(new char[] { ';' }))
            {
                var keyVaulueArray = item.Split(new char[] { '=' });
                if (keyVaulueArray.Length != 2)
                {
                    throw new ArgumentException("Use paris of 'Key1=Value1;Key2=Value2' for implement dictionary data");
                }
                var keyPosition = keyVaulueArray[0];
                var valuePosition = keyVaulueArray[1];
                var key = ParseBasicValueString(keyPosition, dataKeyType);
                var value = ParseBasicValueString(valuePosition, dataValueType);
                methodInfo.Invoke(instance, new object[] { key, value });
            }
            return instance;
        }

        private static object GetGenericListImplementation(string valueString, Type type)
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
    }
}
