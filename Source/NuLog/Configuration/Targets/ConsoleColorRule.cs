using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuLog.Configuration.Targets
{
    public class ConsoleColorRule
    {
        public ICollection<string> Tags { get; set; }
        public ConsoleColor? Color { get; set; }
        public ConsoleColor? BackgroundColor { get; set; }
    }
}
