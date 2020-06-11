using Dodo.Console.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dodo.Console
{
    public static class ConsoleManager
    {
        public static T ParseArguments<T>(string[] args)
        {
            var argsList = args.ToList();
            var instance = (T)Activator.CreateInstance(typeof(T));
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var name = property.Name;
                var type = property.PropertyType;
                if (ParameterUtilities.IsPropertyInArgs(name, argsList))
                {
                    var parameterValue = ParameterUtilities.GetParameterValue(name, type, argsList);
                    property.SetValue(instance, parameterValue);
                }
            }
            return instance;
        }
    }
}
