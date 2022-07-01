using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Generators
{
    internal static class Extensions
    {
        public static string Indent(this int count)
        {
            return Enumerable.Repeat("\t", count).Aggregate((a, b) => a + b);
        }
    }
}
