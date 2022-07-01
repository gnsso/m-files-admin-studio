using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Generators
{
    public static class EnumToViewModelGenerator
    {
        public static string Generate<T>(string className, Func<T, string> nameGenerator = null, Func<T, int> valueGenerator = null, bool isViewModel = false)
        {
            var enumValues = (T[])Enum.GetValues(typeof(T));

            nameGenerator = nameGenerator ?? new Func<T, string>(e => enumValues.Single(s => s.Equals(e)).ToString());
            valueGenerator = valueGenerator ?? new Func<T, int>(e => (int)(object)enumValues.Single(s => s.Equals(e)));

            var sb = new StringBuilder();

            sb.AppendLine($"{2.Indent()}public class {className}{(isViewModel ? " : BindableBase" : "")}");
            sb.AppendLine($"{2.Indent()}{{");
            sb.AppendLine($"{3.Indent()}public static IDictionary<int, {className}> Values {{ get; }}");
            sb.AppendLine();
            AppendProperty("Name", "string", sb, isViewModel, 3);
            AppendProperty("Value", "int", sb, isViewModel, 3);
            sb.AppendLine();
            sb.AppendLine($"{3.Indent()}static {className}()");
            sb.AppendLine($"{3.Indent()}{{");
            sb.AppendLine($"{4.Indent()}if (Values == null)");
            sb.AppendLine($"{4.Indent()}{{");
            sb.AppendLine($"{5.Indent()}Values = new Dictionary<int, {className}>");
            sb.AppendLine($"{5.Indent()}{{");
            foreach (var value in enumValues)
            {
                sb.AppendLine($"{6.Indent()}[{valueGenerator(value)}] = new {className} {{ Name = \"{nameGenerator(value)}\", Value = {valueGenerator(value)},  }},");
            }
            sb.AppendLine($"{5.Indent()}}};");
            sb.AppendLine($"{4.Indent()}}}");
            sb.AppendLine($"{3.Indent()}}}");
            sb.Append($"{2.Indent()}}}");

            return sb.ToString();
        }

        private static void AppendProperty(string name, string type, StringBuilder sb, bool isViewModel, int indent)
        {
            if (isViewModel)
            {
                var lower = new string(name[0].ToString().ToLower().ToArray().Concat(name.Substring(1).ToArray()).ToArray());

                sb.AppendLine($"{indent.Indent()}private {type} {lower};");
                sb.AppendLine($"{indent.Indent()}public {type} {name}");
                sb.AppendLine($"{indent.Indent()}{{");
                sb.AppendLine($"{(indent + 1).Indent()}get {{ return {lower}; }}");
                sb.AppendLine($"{(indent + 1).Indent()}set {{ SetProperty(ref {(lower == "value" ? "this.value" : lower)}, value); }}");
                sb.AppendLine($"{indent.Indent()}}}");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine($"{indent.Indent()}public {type} {name}{{ get; set; }}");
            }
        }
    }
}
