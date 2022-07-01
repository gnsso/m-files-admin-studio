using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Generators
{
    public static class EnumToEnumGenerator
    {
        public static string Generate<T>(string generatedEnumName, Func<T, string> fieldNameGenerator = null, Func<T, string> fieldDescriptionGenerator = null)
        {
            var enumValues = (T[])Enum.GetValues(typeof(T));

            fieldNameGenerator = fieldNameGenerator ?? new Func<T, string>(e => enumValues.Single(s => s.Equals(e)).ToString());

            var hasDescriptions = fieldDescriptionGenerator != null;

            var sb = new StringBuilder();

            sb.AppendLine($"{2.Indent()}public enum {generatedEnumName}");
            sb.AppendLine($"{2.Indent()}{{");
            foreach (var value in enumValues)
            {
                if (hasDescriptions)
                {
                    sb.AppendLine($"{3.Indent()}[Description(\"{fieldDescriptionGenerator(value)}\")]");
                }

                sb.AppendLine($"{3.Indent()}{fieldNameGenerator(value)} = {(int)(object)value},");

                if (hasDescriptions)
                {
                    sb.AppendLine();
                }
            }

            sb.Append($"{2.Indent()}}}");

            return sb.ToString();
        }
    }
}
