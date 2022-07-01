using Humanizer;
using MFilesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Generators
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventHandlerTypeGenerated = EnumToEnumGenerator.Generate<MFEventHandlerType>(
                generatedEnumName: "EventHandlerType",
                fieldNameGenerator: e => e.ToString().Substring("MFEventHandler".Length),
                fieldDescriptionGenerator: e => e.ToString().Substring("MFEventHandler".Length).Humanize(LetterCasing.Title));
        }
    }
}
