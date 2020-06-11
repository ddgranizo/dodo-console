using System;
using System.Collections.Generic;
using System.Text;

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
