using Dodo.Console.Test.Models;
using System;

namespace Dodo.Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var myInputData = ConsoleManager.ParseArguments<MyTestModel>(args);

        }
    }
}
