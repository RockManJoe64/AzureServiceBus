using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusCommon
{
    public class Person
    {
        public string FullName { get; set; }

        public string Location { get; set; }

        public override string ToString()
        {
            return $"{FullName}@{Location}";
        }
    }
}
