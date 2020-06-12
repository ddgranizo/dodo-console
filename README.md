# dodo-console

Basic utilities for console aplications

## Parse input args

Parse args array into a typed class object. 
Usage:

``` [C#]
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
```
with

``` [C#]
namespace Dodo.Console.Test.Models
{
    public class MyTestModel
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsVisible { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<int, string> KeyValues { get; set; }
    }
}
```

and input arguemtns

``` [C#]
MyProgram.exe -Name "My name" -Order 10 -IsVisible -Tags "tech;dev;csharp" -KeyValues "1=First;2=Second"
```


### Allowed types

You can use string parse into the following types
| Type        |            Description                                           |
|-------------|:----------------------------------------------------------------:|
|String       |No parsed needed                                                  |
|Int          |Used int.Parse()                                                  |
|Decimal      |Used decimal.Parse() with replacement of points with commas before|
|Double       |Used double.Parse() with replacement of points with commas before |
|Bool         |[yes, true, 1] parse with true, [no, false, 0] parse with false   |
|Guid         |Used Guid.Parse()                                                 |
|List<>       |Parse list of any of types above                                  |
|Dictionary<,>|Parse dictionary with any of types above for key and values       |
|Enum         |To be developed                                                   |
