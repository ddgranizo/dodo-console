using Dodo.Console.Test.Models;

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
